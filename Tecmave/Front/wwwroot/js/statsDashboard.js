
document.addEventListener("DOMContentLoaded", () => {
    try{
        const root = document.querySelector(".stats-page");
        if (!root) return;

        loadStatsDashboard();
        initStatsCardsHover();
    }catch(e){
        console.error("Error inicializando estadísticas:", e);
    }
});

const metaApi = document.querySelector('meta[name="api-base"]');
let STATS_API_BASE = "";
if (metaApi && metaApi.content) {
    STATS_API_BASE = metaApi.content;
} else if (typeof API !== "undefined" && API) {
    STATS_API_BASE = API;
} else {
    STATS_API_BASE = window.location.origin.replace(":7190", ":7096");
}


// Animación suave de contador
function statsAnimateCounter(elementId, targetValue, options = {}) {
    const el = document.getElementById(elementId);
    if (!el) return;
    const prefix = options.prefix || "";
    const duration = options.duration || 800;
    const start = 0;
    const startTime = performance.now();

    function frame(now) {
        const progress = Math.min((now - startTime) / duration, 1);
        const current = Math.floor(start + (targetValue - start) * progress);
        el.textContent = prefix + current.toLocaleString();
        if (progress < 1) {
            requestAnimationFrame(frame);
        }
    }

    requestAnimationFrame(frame);
}

function statsFormatDate(dateStr) {
    if (!dateStr) return "-";
    return dateStr.substring(0, 10);
}

function statsBuildUserMap(users) {
    const map = {};
    for (const u of users || []) {
        map[u.id] = u;
    }
    return map;
}

function statsGetUserDisplayName(user, fallbackId) {
    if (!user) return "Cliente " + (fallbackId ?? "");
    const full = (user.nombre_completo || user.nombreCompleto || "").trim();
    if (full) return full;
    if (user.userName) return user.userName;
    if (user.UserName) return user.UserName;
    if (user.email) return user.email;
    return "Cliente " + (fallbackId ?? "");
}

let statsCharts = [];
let statsChartByName = {};

async function loadStatsDashboard() {
    try {

        async function safeJson(url){
            try{
                const resp = await fetch(url);
                if(!resp.ok) return [];
                return await resp.json();
            }catch{
                console.error("Error al cargar", url);
                return [];
            }
        }


        
        const [
            fact,
            citas,
            users,
            estados,
            detalles,
            tiposServicios,
            servicios,
            serviciosRevision,
            revisiones,
            vehiculos
        ] = await Promise.all([
            safeJson(`${STATS_API_BASE}/facturas`),
            safeJson(`${STATS_API_BASE}/Agendamiento`),
            safeJson(`${STATS_API_BASE}/api/usuarios`),
            safeJson(`${STATS_API_BASE}/Estados`),
            safeJson(`${STATS_API_BASE}/DetalleFactura`),
            safeJson(`${STATS_API_BASE}/TipoServicios`),
            safeJson(`${STATS_API_BASE}/Servicios`),
            safeJson(`${STATS_API_BASE}/ServiciosRevision`),
            safeJson(`${STATS_API_BASE}/Revision`),
            safeJson(`${STATS_API_BASE}/vehiculos`)
        ]);
        const userMap = statsBuildUserMap(users);
        const meses = ["Ene","Feb","Mar","Abr","May","Jun","Jul","Ago","Sep","Oct","Nov","Dic"];

        const rangoSel = document.getElementById("statsRangoFechas");
        const tipoServSel = document.getElementById("statsTipoServicio");
        const btnActualizar = document.getElementById("statsBtnActualizar");

        // llenar select de tipos de servicio
        if (tipoServSel && tiposServicios && tiposServicios.length) {
            for (const ts of tiposServicios) {
                const opt = document.createElement("option");
                opt.value = String(ts.id_tipo_servicio || ts.idTipoServicio || ts.id);
                opt.textContent = ts.nombre;
                tipoServSel.appendChild(opt);
            }
        }

        const serviciosMap = {};
        for (const s of servicios || []) {
            const key = s.id_servicio || s.idServicio || s.id;
            serviciosMap[String(key)] = s.nombre;
        }

        function aplicarFiltros() {
            statsCharts.forEach(c => { try { c.destroy(); } catch(e){} });
            statsCharts = [];

            const dias = rangoSel ? Number(rangoSel.value || 30) : 30;
            const tipoServicioId = tipoServSel ? tipoServSel.value : "todos";

            const desde = new Date();
            desde.setDate(desde.getDate() - dias);
            const desdeStr = desde.toISOString().substring(0, 10);

            // ---- filtrar facturas por fecha
            const factFiltradas = (fact || []).filter(f => {
                if (!f.fecha_emision) return false;
                const fecha = f.fecha_emision.substring(0,10);
                return fecha >= desdeStr;
            });

            // ---- filtrar citas por fecha
            const citasFiltradas = (citas || []).filter(c => {
                if (!c.fecha_agregada) return true; // por si vienen nulas
                const fecha = c.fecha_agregada.substring(0,10);
                return fecha >= desdeStr;
            });

            // ---- detalle filtrado por servicio
            let detallesFiltrados = detalles || [];
            if (tipoServicioId && tipoServicioId !== "todos") {
                detallesFiltrados = detallesFiltrados.filter(d => String(d.servicio_id) === String(tipoServicioId));
            }

            // ========== KPIs ==========

            const totalIngresos = factFiltradas.reduce((s,f) => s + (Number(f.total) || 0), 0);
            statsAnimateCounter("statsTotalIngresos", Math.round(totalIngresos), { prefix: "₡" });

            const citasCompletadas = citasFiltradas.length;
            statsAnimateCounter("statsCitasCompletadas", citasCompletadas);

            const clientesActivos = (users || []).filter(u => u.estado === 1 || u.estado === "1");
            statsAnimateCounter("statsNuevosClientes", clientesActivos.length);

            const clientesVarEl = document.getElementById("statsClientesVariacion");
            if (clientesVarEl) {
                clientesVarEl.textContent = `${clientesActivos.length} activos`;
            }

            // tiempo promedio entre fecha_agregada y fecha_estimada
            let totalHoras = 0;
            let countHoras = 0;
            for (const c of citasFiltradas) {
                if (c.fecha_agregada && c.fecha_estimada) {
                    const ini = new Date(c.fecha_agregada);
                    const fin = new Date(c.fecha_estimada);
                    const diff = fin - ini;
                    if (!isNaN(diff) && diff > 0) {
                        totalHoras += diff / (1000*60*60);
                        countHoras++;
                    }
                }
            }
            const tiempoPromEl = document.getElementById("statsTiempoPromedio");
            if (tiempoPromEl) {
                if (countHoras > 0) {
                    const prom = totalHoras / countHoras;
                    tiempoPromEl.textContent = prom.toFixed(1) + "h";
                } else {
                    tiempoPromEl.textContent = "-";
                }
            }

            // ========== GRAFICO: Flujo de servicios (citas por estado) ==========
            const mapEstados = {};
            for (const e of estados || []) {
                mapEstados[e.id_estado] = e.nombre || e.nombreEstado || ("Estado " + e.id_estado);
            }
            const conteoEstados = {};
            for (const c of citasFiltradas) {
                const idEst = c.id_estado;
                const nombre = mapEstados[idEst] || ("Estado " + idEst);
                conteoEstados[nombre] = (conteoEstados[nombre] || 0) + 1;
            }
            const flujoLabels = Object.keys(conteoEstados);
            const flujoData = Object.values(conteoEstados);
            const canvasFlujo = document.getElementById("statsFlujoServiciosChart");
            if (canvasFlujo && flujoLabels.length && window.Chart) {
                const ch = new Chart(canvasFlujo, {
                    type: "bar",
                    data: {
                        labels: flujoLabels,
                        datasets: [{
                            label: "Citas",
                            data: flujoData,
                            backgroundColor: "#ffe0e5",
                            borderColor: "#e60023",
                            borderWidth: 2,
                            borderRadius: 6,
                            hoverBackgroundColor: "#ffccd6"
                        }]
                    },
                    options: {
                        animation:{duration:800,easing:'easeOutCubic'},
                        responsive:true,
                        maintainAspectRatio:false,
                        scales:{ y:{ beginAtZero:true } },
                        plugins:{legend:{display:true}}
                    }
                });
                statsCharts.push(ch);
                statsChartByName["flujo"] = ch;
            }

            // ========== GRAFICO: Retención de clientes ==========
            const clientesConFactura = new Set(factFiltradas.map(f => f.cliente_id).filter(id => id != null));
            let retenidos = 0;
            let perdidos = 0;
            for (const c of clientesActivos) {
                if (clientesConFactura.has(c.id)) retenidos++;
                else perdidos++;
            }

            const canvasRet = document.getElementById("statsRetencionClientesChart");
            if (canvasRet && window.Chart) {
                const ch = new Chart(canvasRet, {
                    type: "doughnut",
                    data: {
                        labels: ["Clientes retenidos","Clientes perdidos"],
                        datasets: [{
                            data: [retenidos, perdidos],
                            backgroundColor:["#e60023","#ffe6ea"],
                            borderColor:"#ffffff",
                            borderWidth:2
                        }]
                    },
                    options:{
                        animation:{duration:800,easing:'easeOutCubic'},
                        responsive:true,
                        maintainAspectRatio:false,
                        plugins:{ legend:{ position:"bottom" } }
                    }
                });
                statsCharts.push(ch);
                statsChartByName["retencion"] = ch;
            }


            // ========== GRAFICO: Volumen de transacciones (por mes) ==========
            const porMes = {};
            for (const f of factFiltradas) {
                if (!f.fecha_emision) continue;
                const d = new Date(f.fecha_emision);
                if (isNaN(d)) continue;
                const key = meses[d.getMonth()];
                const monto = Number(f.total) || 0;
                porMes[key] = (porMes[key] || 0) + monto;
            }
            const mesLabels = Object.keys(porMes);
            const mesData = mesLabels.map(k => porMes[k]);
            const canvasVol = document.getElementById("statsVolumenTransaccionesChart");
            if (canvasVol && mesLabels.length && window.Chart) {
                const ch = new Chart(canvasVol, {
                    type:"line",
                    data:{
                        labels: mesLabels,
                        datasets:[{
                            label:"Ingresos",
                            data: mesData,
                            fill:true,
                            borderColor:"#e60023",
                            backgroundColor:"rgba(230,0,35,0.08)",
                            borderWidth:3,
                            pointRadius:4,
                            pointHoverRadius:6,
                            pointBackgroundColor:"#ffffff",
                            pointBorderColor:"#e60023",
                            tension:0.35
                        }]
                    },
                    options:{
                        animation:{duration:900,easing:'easeOutCubic'},
                        responsive:true,
                        maintainAspectRatio:false,
                        scales:{ y:{ beginAtZero:false } },
                        plugins:{legend:{display:true,position:"top"}}
                    }
                });
                statsCharts.push(ch);
                statsChartByName["volumen"] = ch;
            }

            // ========== GRAFICO: Servicios populares ==========
            
            
            
            const conteoServicios = {};

            // 1) Inicializar todos los servicios en 0 usos (API /Servicios)
            for (const s of (servicios || [])) {
                const keyServ = s.id_servicio || s.idServicio || s.id;
                const nombreServ = s.nombre || ("Servicio " + keyServ);
                if (!nombreServ) continue;
                conteoServicios[nombreServ] = 0;
            }

            // 2) Determinar de dónde salen los usos:
            //    Prioridad: ServiciosRevision -> DetalleFactura -> Citas(Agendamiento)
            let origenServicios = (serviciosRevision && serviciosRevision.length)
                ? serviciosRevision
                : (detallesFiltrados && detallesFiltrados.length
                    ? detallesFiltrados
                    : []);

            if (!origenServicios.length && (citasFiltradas && citasFiltradas.length)) {
                // Detectar dinámicamente una propiedad relacionada con "servicio" en las citas
                const muestra = citasFiltradas.find(c => c && typeof c === "object");
                if (muestra) {
                    const keys = Object.keys(muestra);
                    const keyServicioEnCita = keys.find(k => k.toLowerCase().includes("servicio"));
                    if (keyServicioEnCita) {
                        origenServicios = citasFiltradas
                            .map(c => {
                                const val = c[keyServicioEnCita];
                                if (val === null || val === undefined) return null;
                                return { servicio_id: val };
                            })
                            .filter(x => x !== null);
                    }
                }
            }

            // 3) Aumentar el conteo según origen detectado
            for (const d of origenServicios) {
                const idServ = d.servicio_id || d.id_servicio || d.idServicio || d.id;
                const nombre = serviciosMap[String(idServ)] || ("Servicio " + idServ);
                if (!nombre) continue;
                conteoServicios[nombre] = (conteoServicios[nombre] || 0) + 1;
            }

const servLabels = Object.keys(conteoServicios);
            const servData = Object.values(conteoServicios);
            const canvasServ = document.getElementById("statsServiciosPopularesChart");
            if (canvasServ && servLabels.length && window.Chart) {
                const ch = new Chart(canvasServ, {
                    type:"bar",
                    data:{
                        labels: servLabels,
                        datasets:[{
                            label:"Uso",
                            data: servData,
                            backgroundColor:"#ffe0e5",
                            borderColor:"#e60023",
                            borderWidth:2,
                            borderRadius:6,
                            hoverBackgroundColor:"#ffccd6"
                        }]
                    },
                    options:{
                        animation:{duration:800,easing:'easeOutCubic'},
                        responsive:true,
                        maintainAspectRatio:false,
                        scales:{ y:{ beginAtZero:true } },
                        plugins:{legend:{display:true,position:"top"}}
                    }
                });
                statsCharts.push(ch);
                statsChartByName["servicios"] = ch;
            }

            statsWireChartActions();

            // ========== TOP CLIENTES ==========
            const totalPorCliente = {};
            for (const f of factFiltradas) {
                if (!f.cliente_id) continue;
                const idCli = f.cliente_id;
                const monto = Number(f.total) || 0;
                totalPorCliente[idCli] = (totalPorCliente[idCli] || 0) + monto;
            }
            const topClientes = Object.entries(totalPorCliente)
                .sort((a,b) => b[1]-a[1])
                .slice(0,5);

            const ulTop = document.getElementById("statsTopClientesLista");
            if (ulTop) {
                ulTop.innerHTML = "";
                for (const [idCli, monto] of topClientes) {
                    const li = document.createElement("li");
                    const user = userMap[idCli];
                    const nombre = statsGetUserDisplayName(user, idCli);
                    li.innerHTML = `<span>${nombre}</span><span>₡${Math.round(monto).toLocaleString()}</span>`;
                    ulTop.appendChild(li);
                }
            }

            // ========== ACTIVIDAD RECIENTE ==========
            const actividades = [];
            const hoyStr = new Date().toISOString().substring(0,10);

            for (const c of citasFiltradas) {
                const user = userMap[c.cliente_id];
                actividades.push({
                    fecha: (c.fecha_agregada || "").substring(0,10) || hoyStr,
                    accion:"Cita agendada",
                    usuario: statsGetUserDisplayName(user, c.cliente_id),
                    detalle:"Nueva cita registrada en el sistema"
                });
            }

            for (const f of factFiltradas) {
                const user = userMap[f.cliente_id];
                const total = Number(f.total) || 0;
                const estado = (f.estado_pago || "").toString();
                actividades.push({
                    fecha:(f.fecha_emision || "").substring(0,10) || hoyStr,
                    accion: estado === "Pagada" ? "Factura pagada" : "Factura generada",
                    usuario: statsGetUserDisplayName(user, f.cliente_id),
                    detalle:`Monto: ₡${Math.round(total).toLocaleString()}`
                });
            }

            actividades.sort((a,b) => (a.fecha < b.fecha ? 1 : -1));
            const ultimas = actividades.slice(0,5);
            const tbodyAct = document.getElementById("statsActividadRecienteBody");
            if (tbodyAct) {
                tbodyAct.innerHTML = "";
                for (const ac of ultimas) {
                    const tr = document.createElement("tr");
                    tr.innerHTML = `<td>${ac.fecha}</td><td>${ac.accion}</td><td>${ac.usuario}</td><td>${ac.detalle}</td>`;
                    tbodyAct.appendChild(tr);
                }
            }

            // ========== DATOS DETALLADOS ==========
            const tbodyDet = document.getElementById("statsDetalleTableBody");
            if (tbodyDet) {
                tbodyDet.innerHTML = "";

                // Mapear un servicio principal por factura
                const porIdFacturaServicio = {};
                for (const d of detalles || []) {
                    if (!porIdFacturaServicio[d.factura_id]) {
                        const nombre = serviciosMap[String(d.servicio_id)] || ("Servicio " + d.servicio_id);
                        porIdFacturaServicio[d.factura_id] = nombre;
                    }
                }

                const factOrdenadas = [...factFiltradas].sort((a,b) => {
                    const fa = (a.fecha_emision || "").substring(0,10);
                    const fb = (b.fecha_emision || "").substring(0,10);
                    return fa < fb ? 1 : -1;
                }).slice(0, 10);

                for (const f of factOrdenadas) {
                    const fecha = (f.fecha_emision || "").substring(0,10);
                    const user = userMap[f.cliente_id];
                    const clienteNombre = statsGetUserDisplayName(user, f.cliente_id);
                    const servicioNombre = porIdFacturaServicio[f.id_factura] || "Servicio general";
                    const ingreso = Math.round(Number(f.total) || 0);
                    const estado = (f.estado_pago || "").toString() || "Pendiente";

                    const tr = document.createElement("tr");
                    tr.innerHTML = `
                        <td>${fecha}</td>
                        <td>${servicioNombre}</td>
                        <td>${clienteNombre}</td>
                        <td>-</td>
                        <td>₡${ingreso.toLocaleString()}</td>
                        <td><span class="estado-pill">${estado}</span></td>`;
                    tbodyDet.appendChild(tr);
                }
            }

        } // fin aplicarFiltros

        if (btnActualizar) {
            btnActualizar.addEventListener("click", e => {
                e.preventDefault();
                aplicarFiltros();
            });
        }

        aplicarFiltros();

    } catch (e) {
        console.error("Error cargando estadísticas:", e);
    }
}

function initStatsCardsHover(){
    const cards = document.querySelectorAll(".admin-card");
    cards.forEach(card => {
        card.addEventListener("mouseenter", () => card.classList.add("card-hover"));
        card.addEventListener("mouseleave", () => card.classList.remove("card-hover"));
    });
}



function statsWireChartActions(){
    const downloads = document.querySelectorAll(".chart-btn-download");
    const expands = document.querySelectorAll(".chart-btn-expand");
    const modal = document.getElementById("chartModal");
    const modalImg = document.getElementById("chartModalImage");
    const btnClose = document.getElementById("chartModalClose");

    // Descargar PNG del gráfico
    downloads.forEach(btn => {
        btn.onclick = () => {
            const key = btn.dataset.chart;
            const chart = statsChartByName[key];
            if (!chart) return;
            try{
                const link = document.createElement("a");
                link.href = chart.toBase64Image();
                link.download = (key || "grafico") + ".png";
                document.body.appendChild(link);
                link.click();
                document.body.removeChild(link);
            }catch(e){
                console.error("No se pudo descargar el gráfico", e);
            }
        };
    });

    function closeModal(){
        if (!modal) return;
        modal.classList.remove("show");
        if (modalImg){
            modalImg.src = "";
        }
    }

    // Expandir en modal usando imagen
    expands.forEach(btn => {
        btn.onclick = () => {
            if (!modal || !modalImg) return;
            const key = btn.dataset.chart;
            const chart = statsChartByName[key];
            if (!chart) return;
            try{
                const imgUrl = chart.toBase64Image();
                modalImg.src = imgUrl;
                modal.classList.add("show");
            }catch(e){
                console.error("No se pudo expandir el gráfico", e);
            }
        };
    });

    if (btnClose){
        btnClose.onclick = closeModal;
    }
    if (modal){
        modal.addEventListener("click", (ev) => {
            if (ev.target === modal){
                closeModal();
            }
        });
    }
}

