(function () {
    /**
     * Initializes the carousel functionality.
     */
    function initCarousel() {
        const container = document.querySelector(".carousel-container");
        const track = document.querySelector(".carousel-track");
        const nextBtn = document.querySelector(".carousel-next");
        const prevBtn = document.querySelector(".carousel-prev");

        if (!container || !track) return;

        const slideWidth = container.offsetWidth;
        nextBtn?.addEventListener("click", () => track.scrollBy({ left: slideWidth, behavior: "smooth" }));
        prevBtn?.addEventListener("click", () => track.scrollBy({ left: -slideWidth, behavior: "smooth" }));
    }

    /**
     * Displays a toast message.
     * @param {string} message The message to display.
     * @param {number} duration The duration in milliseconds.
     */
    function toast(message, duration = 3000) {
        const toast = document.createElement("div");
        toast.className = "toast";
        toast.textContent = message;
        document.body.appendChild(toast);

        requestAnimationFrame(() => {
            toast.style.opacity = "1";
        });

        setTimeout(() => {
            toast.style.opacity = "0";
            setTimeout(() => document.body.removeChild(toast), 300);
        }, duration);
    }

    // Expose toast to the global window object
    window.toast = toast;

    /**
     * Initializes the modal functionality.
     */
    function initModals() {
        const modalButtons = document.querySelectorAll("[data-modal]");
        modalButtons.forEach(button => {
            button.addEventListener("click", () => {
                const modalId = button.getAttribute("data-modal");
                const modal = document.getElementById(modalId);
                if (modal) {
                    modal.style.display = "block";
                }
            });
        });

        const closeButtons = document.querySelectorAll(".close-button");
        closeButtons.forEach(button => {
            button.addEventListener("click", () => {
                const modal = button.closest(".modal");
                if (modal) {
                    modal.style.display = "none";
                }
            });
        });

        window.addEventListener("click", (event) => {
            if (event.target.classList.contains("modal")) {
                event.target.style.display = "none";
            }
        });
    }

    /**
     * Main initialization function on DOMContentLoaded.
     */
    document.addEventListener("DOMContentLoaded", () => {
        initCarousel();
        initModals();
    });

})();

// === Facturas module (merged) ===
(function () {
    const $ = window.jQuery;
    function onFacturaPage() { const el = document.getElementById("tablaFacturas"); if (el) return el; return (location.pathname || "").toLowerCase().includes("/factura"); }
    if (!onFacturaPage()) return;

    const API_BASE = (document.querySelector('meta[name="api-base"]')?.content) || `${location.protocol}//${location.host}`;
    const URL_FACTURAS = `${API_BASE}/facturas`;
    const URL_USUARIOS = `${API_BASE}/api/usuarios`;
    const URL_UPDATE_ESTADO = id => `${API_BASE}/facturas/${id}/estado`;

    let facturas = [], usuarios = [];

    function nombreClienteById(id) {
        const u = usuarios.find(x => Number(x.id || x.Id || x.usuario_id) === Number(id));
        if (!u) return { nombre: "Desconocido", correo: "-", telefono: "-" };
        const nombre = [u.nombre || u.Nombre, u.apellido || u.Apellido].filter(Boolean).join(" ").trim() || (u.user_name || u.UserName || "Usuario");
        return { nombre, correo: u.email || u.Email || u.correo || "-", telefono: u.phone_number || u.PhoneNumber || "-" };
    }
    const fmtFecha = d => { const dt = new Date(d); return isNaN(dt) ? (d || "") : dt.toLocaleDateString(); };
    const fmtMoney = n => Number(n || 0).toLocaleString('en-US', { style: 'currency', currency: 'USD' });
    const pillEstado = e => {
        const map = { pagada: "green", pendiente: "yellow", anulada: "red" }; const k = (e || "Pendiente").toString();
        const cls = map[k.toLowerCase()] || "yellow"; return `<span class="pill ${cls}">${k}</span>`;
    };
    function uniqueBy(arr, key) { const s = new Set(), o = []; for (const it of arr) { const k = key(it); if (!s.has(k)) { s.add(k); o.push(it) } } return o; }

    async function cargarDatos() {
        const [fRes, uRes] = await Promise.all([$.get(URL_FACTURAS), $.get(URL_USUARIOS)]);
        facturas = Array.isArray(fRes) ? fRes : (fRes?.data || []);
        usuarios = Array.isArray(uRes) ? uRes : (uRes?.data || []);
        popularClientesExport();
        renderTabla(facturas);
    }

    function popularClientesExport() {
        const $sel = $("#selClienteExport"); if (!$sel.length) return;
        $sel.empty().append(`<option value="">-- Seleccione cliente --</option>`);
        const items = uniqueBy(
            facturas.map(f => {
                const id = Number(f.cliente_id || f.ClienteId || f.clienteId);
                const info = nombreClienteById(id);
                return { id, nombre: info.nombre };
            }).filter(x => x.id),
            x => x.id
        ).sort((a, b) => a.nombre.localeCompare(b.nombre));
        for (const c of items) { $sel.append(`<option value="${c.id}">${c.nombre}</option>`); }
    }

    function aplicarFiltros() {
        const nameQ = ($("#filtroNombre").val() || "").toLowerCase().trim();
        const estadoQ = ($("#filtroEstado").val() || "").trim();
        const fd = $("#filtroDesde").val(), fh = $("#filtroHasta").val();
        const desde = fd ? new Date(fd) : null, hasta = fh ? new Date(fh) : null;
        let data = [...facturas];
        if (nameQ) {
            data = data.filter(f => { const idCli = Number(f.cliente_id || f.ClienteId || f.clienteId); return nombreClienteById(idCli).nombre.toLowerCase().includes(nameQ); });
        }
        if (estadoQ) {
            data = data.filter(f => (f.estado_pago || f.EstadoPago || f.estado || "").toString().toLowerCase() === estadoQ.toLowerCase());
        }
        if (desde || hasta) {
            data = data.filter(f => { const dt = new Date(f.fecha_emision || f.FechaEmision || f.fecha || f.Fecha); if (desde && dt < desde) return false; if (hasta && dt > hasta) return false; return true; });
        }
        renderTabla(data);
    }

    async function cambiarEstado(id, nuevo, $sel) {
        try {
            await $.ajax({ url: URL_UPDATE_ESTADO(id), method: "PUT", contentType: "application/json", data: JSON.stringify({ estado: nuevo }) });
            const it = facturas.find(x => Number(x.id_factura || x.id || x.Id) === Number(id)); if (it) it.estado_pago = nuevo;
            aplicarFiltros();
        } catch (e) { console.error(e); alert("No se pudo actualizar el estado."); if ($sel) $sel.val($sel.data("prev")); }
    }

    function renderTabla(data) {
        const $tb = $("#tablaFacturas"), $badge = $("#badge");
        $tb.empty();
        if (!data.length) { $tb.append(`<tr><td colspan="9" class="fx-center">No se encontraron facturas.</td></tr>`); if ($badge.length) { $badge.text(`Mostrando 0 de ${facturas.length}`) }; return; }
        for (const f of data) {
            const id = f.id_factura || f.id || f.Id;
            const idCli = Number(f.cliente_id || f.ClienteId || f.clienteId);
            const info = nombreClienteById(idCli);
            const fecha = fmtFecha(f.fecha_emision || f.FechaEmision || f.fecha || f.Fecha);
            const total = fmtMoney(f.total || f.Total);
            const metodo = f.metodo_pago || f.MetodoPago || "-";
            const estado = (f.estado_pago || f.EstadoPago || "Pendiente").toString();
            const sel = `<select class="input select-estado" data-prev="${estado}" data-id="${id}">
        <option ${estado === 'Pagada' ? 'selected' : ''}>Pagada</option>
        <option ${estado === 'Pendiente' ? 'selected' : ''}>Pendiente</option>
        <option ${estado === 'Anulada' ? 'selected' : ''}>Anulada</option>
      </select>`;
            $tb.append(`<tr>
        <td>${id}</td><td>${info.nombre}</td><td>${info.correo}</td><td>${info.telefono}</td>
        <td>${fecha}</td><td>${metodo}</td><td>${pillEstado(estado)}</td><td>${total}</td><td>${sel}</td>
      </tr>`);
        }
        $("#tablaFacturas .select-estado").off("change").on("change", function () {
            const $s = $(this); cambiarEstado($s.data("id"), $s.val(), $s);
        });
        if ($badge.length) { $badge.text(`Mostrando ${data.length} de ${facturas.length}`); }
    }

    function flatRow(f) {
        const id = f.id_factura || f.id || f.Id; const idCli = Number(f.cliente_id || f.ClienteId || f.clienteId);
        const info = nombreClienteById(idCli);
        return {
            "ID Factura": id,
            "Cliente": info.nombre,
            "Correo": info.correo,
            "Teléfono": info.telefono,
            "Fecha emisión": fmtFecha(f.fecha_emision || f.FechaEmision || f.fecha || f.Fecha),
            "Método pago": f.metodo_pago || f.MetodoPago || "-",
            "Estado": (f.estado_pago || f.EstadoPago || "Pendiente").toString(),
            "Total": Number(f.total || f.Total || 0)
        };
    }
    function exportToXlsx(rows, filename) {
        if (!window.XLSX) { alert("Biblioteca XLSX no disponible."); return; }
        const ws = XLSX.utils.json_to_sheet(rows), wb = XLSX.utils.book_new(); XLSX.utils.book_append_sheet(wb, ws, "Facturas");
        XLSX.writeFile(wb, filename.endsWith(".xlsx") ? filename : filename + ".xlsx");
    }
    function exportMes() {
        const mm = $("#filtroMesExport").val(); if (!mm) { alert("Seleccione el mes a exportar."); return; }
        const [y, m] = mm.split("-").map(Number); const d = new Date(y, m - 1, 1), h = new Date(y, m, 0, 23, 59, 59, 999);
        exportToXlsx(facturas.filter(f => { const dt = new Date(f.fecha_emision || f.FechaEmision || f.fecha || f.Fecha); return dt >= d && dt <= h; }).map(flatRow), `facturas_${mm}.xlsx`);
    }
    function exportCliente() {
        const id = Number($("#selClienteExport").val()); if (!id) { alert("Seleccione un cliente."); return; }
        exportToXlsx(facturas.filter(f => Number(f.cliente_id || f.ClienteId || f.clienteId) === id).map(flatRow), `facturas_cliente_${id}.xlsx`);
    }
    function exportPendientes() {
        exportToXlsx(facturas.filter(f => ((f.estado_pago || f.EstadoPago || "Pendiente") + "").toLowerCase() === "pendiente").map(flatRow), "facturas_pendientes.xlsx");
    }

    $(function () {
        cargarDatos();
        $("#btnFiltrar").on("click", aplicarFiltros);
        $("#filtroNombre,#filtroEstado,#filtroDesde,#filtroHasta").on("input change", aplicarFiltros);
        $("#btnExportMes").on("click", exportMes);
        $("#btnExportCliente").on("click", exportCliente);
        $("#btnExportPendientes").on("click", exportPendientes);
    });
})();


// === Facturas: modal, creación manual y automática ===
(function () {
    const $ = window.jQuery;
    function onFacturaPage() { return document.getElementById("tablaFacturas"); }
    if (!onFacturaPage()) return;

    const API_BASE = (document.querySelector('meta[name="api-base"]')?.content) || `${location.protocol}//${location.host}`;
    const URL_FACTURAS = `${API_BASE}/facturas`;
    const URL_USUARIOS = `${API_BASE}/api/usuarios`;
    const URL_SERVICIOS = `${API_BASE}/Servicios`;

    // Helpers de modal
    function openModal(id) { document.getElementById(id)?.classList.remove("hidden"); }
    function closeModal(id) { document.getElementById(id)?.classList.add("hidden"); }
    $(document).on("click", "[data-close]", function () { closeModal($(this).data("close")); });

    // Popular selects en modales
    async function cargarListasModal() {
        const [u, s] = await Promise.all([$.get(URL_USUARIOS), $.get(URL_SERVICIOS)]).catch(() => [[], []]);
        const usuarios = Array.isArray(u) ? u : (u?.data || []);
        const servicios = Array.isArray(s) ? s : (s?.data || []);
        const cliOpts = usuarios.map(u => `<option value="${u.id || u.Id}">${(u.nombre || u.Nombre || u.user_name || u.UserName || "Usuario")}</option>`).join("");
        $("#crear_cliente,#auto_cliente").html(cliOpts);
        const srvOpts = servicios.map(s => `<option data-precio="${s.precio}" value="${s.id_servicio || s.id || s.Id}">${s.nombre} — ${Number(s.precio || 0).toFixed(2)}</option>`).join("");
        $("#auto_servicio").html(srvOpts);
    }

    // Abrir modales
    $("#btnAbrirModalCrear").on("click", async function () {
        await cargarListasModal();
        const today = new Date(); const iso = today.toISOString().slice(0, 10);
        $("#crear_fecha").val(iso); $("#crear_total").val("");
        openModal("modalCrearFactura");
    });
    $("#btnAbrirModalAuto").on("click", async function () {
        await cargarListasModal();
        openModal("modalAutoFactura");
    });

    // Crear factura manual
    $("#btnCrearFactura").on("click", async function () {
        const cliente = Number($("#crear_cliente").val());
        const metodo = $("#crear_metodo").val();
        const fecha = $("#crear_fecha").val();
        const total = Number($("#crear_total").val());
        if (!cliente || !total) { alert("Cliente y Total son requeridos."); return; }
        try {
            await $.ajax({
                url: URL_FACTURAS,
                method: "POST",
                contentType: "application/json",
                data: JSON.stringify({
                    cliente_id: cliente,
                    fecha_emision: fecha ? new Date(fecha).toISOString() : null,
                    total: total,
                    metodo_pago: metodo,
                    estado_pago: "Pendiente"
                })
            });
            closeModal("modalCrearFactura");
            // recargar data global que ya existe en el módulo de facturas (si está)
            $("#btnFiltrar").trigger("click"); // fuerza refresco con filtros actuales
            location.reload(); // fallback simple para asegurar la nueva factura visible
        } catch (e) { console.error(e); alert("No se pudo crear la factura."); }
    });

    // Crear factura automática desde servicio
    $("#btnCrearAuto").on("click", async function () {
        const cliente = Number($("#auto_cliente").val());
        const servicio = Number($("#auto_servicio").val());
        const metodo = $("#auto_metodo").val();
        if (!cliente || !servicio) { alert("Cliente y Servicio son requeridos."); return; }
        try {
            await $.ajax({
                url: `${URL_FACTURAS}/generar-automatico`,
                method: "POST",
                contentType: "application/json",
                data: JSON.stringify({
                    cliente_id: cliente,
                    servicio_id: servicio,
                    metodo_pago: metodo
                })
            });
            closeModal("modalAutoFactura");
            $("#btnFiltrar").trigger("click");
            location.reload();
        } catch (e) { console.error(e); alert("No se pudo generar la factura automáticamente (¿servicio válido?)."); }
    });
})();


// === Facturas: búsqueda por número + descarga PDF + enviar correo + recordatorios ===
(function () {
    const $ = window.jQuery;
    function onFacturaPage() { return document.getElementById("tablaFacturas"); }
    if (!onFacturaPage()) return;

    const API_BASE = (document.querySelector('meta[name="api-base"]')?.content) || `${location.protocol}//${location.host}`;
    const URL_FACTURAS = `${API_BASE}/facturas`;

    function getFacturaRowData($tr) {
        const tds = $tr.children('td');
        return {
            id: Number($(tds[0]).text() || 0),
            cliente: $(tds[1]).text(),
            correo: $(tds[2]).text(),
            telefono: $(tds[3]).text(),
            fecha: $(tds[4]).text(),
            metodo: $(tds[5]).text(),
            estado: $(tds[6]).text(),
            total: $(tds[7]).text()
        };
    }

    // Descarga PDF simple usando jsPDF
    function descargarPDF($tr) {
        const { jsPDF } = window.jspdf || {};
        if (!jsPDF) { alert("jsPDF no está disponible."); return; }
        const data = getFacturaRowData($tr);
        const doc = new jsPDF();
        doc.setFontSize(16);
        doc.text("Factura", 20, 20);
        doc.setFontSize(11);
        const lines = [
            `ID: ${data.id}`,
            `Cliente: ${data.cliente}`,
            `Correo: ${data.correo}  Tel: ${data.telefono}`,
            `Fecha emisión: ${data.fecha}`,
            `Método pago: ${data.metodo}`,
            `Estado: ${data.estado}`,
            `Total: ${data.total}`
        ];
        let y = 35;
        lines.forEach(l => { doc.text(l, 20, y); y += 8; });
        doc.save(`factura_${data.id}.pdf`);
    }

    // Enviar correo: usa stub de API
    async function enviarCorreo(id) {
        try {
            const res = await $.ajax({ url: `${URL_FACTURAS}/${id}/enviar-email`, method: "POST" });
            alert("Factura enviada al correo (encolada).");
        } catch (e) { console.error(e); alert("No se pudo enviar por correo."); }
    }

    // Agregar botones por fila
    function addRowActions() {
        $("#tablaFacturas tr").each(function () {
            const $tr = $(this);
            if ($tr.find(".btnDescargar").length) return;
            const $last = $tr.children("td").last();
            const id = Number($tr.children("td").first().text() || 0);
            const $wrap = $("<div class='fx-actions'></div>");
            const $btnD = $('<button class="btn btn-secondary btnDescargar">Descargar</button>').on("click", () => descargarPDF($tr));
            const $btnE = $('<button class="btn btn-secondary btnEnviar">Enviar</button>').on("click", () => enviarCorreo(id));
            $wrap.append($btnD, $btnE);
            $last.append($wrap);
        });
    }

    // Extender filtros: número de factura
    const __aplicarFiltros = window.__aplicarFiltrosFacturas || function () { };
    function aplicarFiltrosWrap() {
        // Intenta usar la función ya registrada del módulo de facturas; en su defecto rehace un click que refresque
        if (typeof window.__facturas_aplicarFiltros === 'function') { window.__facturas_aplicarFiltros(); }
        $("#tablaFacturas").trigger("fx:afterRender");
    }

    // Hook después del render de tabla (lo disparamos tras render en módulo base)
    $(document).on("fx:afterRender", function () { addRowActions(); });

    // Interceptar el filtro por número
    $("#filtroNumero").on("input", function () {
        const q = ($(this).val() || "").trim();
        if (!q) { $("#btnFiltrar").trigger("click"); return; }
        $("#tablaFacturas tr").each(function () {
            const id = ($(this).children("td").first().text() || "").trim();
            $(this).toggle(id.indexOf(q) !== -1);
        });
    });

    // Recordatorios de pendientes
    $("#btnEnviarRecordatorios").on("click", async function () {
        try {
            const res = await $.ajax({ url: `${URL_FACTURAS}/recordatorios/pendientes`, method: "POST" });
            alert(`Recordatorios enviados: ${res?.total ?? 0}`);
        } catch (e) { console.error(e); alert("No se pudieron enviar los recordatorios."); }
    });

    // Disparar addRowActions después de cada render (simple fallback)
    setInterval(() => { addRowActions(); }, 1200);

})(); 
