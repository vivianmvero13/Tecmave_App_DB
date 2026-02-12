
document.addEventListener("DOMContentLoaded", () => { loadAdminDashboard(); initAdminCardsHover(); });

const API = document.querySelector('meta[name="api-base"]').content;

// Animación suave de contador
function animateCounter(elementId, targetValue, options = {}) {
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

function formatDate(dateStr) {
    if (!dateStr) return "-";
    // soporta "2025-11-25T00:00:00" o "2025-11-28"
    return dateStr.substring(0, 10);
}

function buildUserMap(users) {
    const map = {};
    for (const u of users) {
        map[u.id] = u;
    }
    return map;
}

function getUserDisplayName(user, fallbackId) {
    if (!user) return fallbackId ? `Usuario #${fallbackId}` : "Desconocido";
    const nombre = (user.nombre || "").trim();
    const apellidos = (user.apellidos || "").trim();
    const full = `${nombre} ${apellidos}`.trim();
    return full || user.userName || `Usuario #${fallbackId}`;
}

async function loadAdminDashboard(){
 try{
   // ==== FACTURAS ====
   const fact = await fetch(`${API}/facturas`).then(r=>r.json());
   const totalIngresos = fact.reduce((s,f)=> s + (Number(f.total) || 0), 0);

   // animar ingresos
   animateCounter("totalIngresos", Math.round(totalIngresos), { prefix: "₡" });

   // ==== AGENDAMIENTO ====
   const citas = await fetch(`${API}/Agendamiento`).then(r=>r.json());
   const hoy = new Date().toISOString().substring(0,10);
   const nuevasHoy = citas.filter(c => c.fecha_agregada === hoy).length;

   // animar citas totales
   animateCounter("citasPendientes", citas.length);
   const citasNuevasEl = document.getElementById("citasNuevas");
   if (citasNuevasEl) {
       citasNuevasEl.textContent = `${nuevasHoy} hoy`;
   }

   // ==== USUARIOS ====
   const users = await fetch(`${API}/api/usuarios`).then(r=>r.json());
   const clientes = users.filter(u => u.estado === 1);

   // animar clientes
   animateCounter("nuevosClientes", clientes.length);

   const clientesVarEl = document.getElementById("clientesVariacion");
   if (clientesVarEl) {
       clientesVarEl.textContent = "";
   }

   const userMap = buildUserMap(users);

   // ========= GRAFICO 1: Ingresos por estado de factura =========
   const ingresosPorEstado = {};
   for (const f of fact) {
       const estado = (f.estado_pago || "Sin estado").toString();
       const monto = Number(f.total) || 0;
       ingresosPorEstado[estado] = (ingresosPorEstado[estado] || 0) + monto;
   }
   const estadoLabels = Object.keys(ingresosPorEstado);
   const estadoData = Object.values(ingresosPorEstado);

   const canvasEstado = document.getElementById("ingresosEstadoChart");
   if (canvasEstado && estadoLabels.length && window.Chart) {
       new Chart(canvasEstado, {
           type: "bar",
           data: {
               labels: estadoLabels,
               datasets: [{
                   label: "Ingresos por estado de factura",
                   data: estadoData,
                   backgroundColor: "rgba(230,0,35,0.25)",
                   borderColor: "#e60023",
                   borderWidth: 2
               }]
           },
           options: {animation:{duration:800,easing:'easeOutCubic'},
               responsive: true,
               maintainAspectRatio: false,
               scales: {
                   y: { beginAtZero: true }
               }
           }
       });
   }

   // ========= GRAFICO 2: Ingresos por día (últimos 7 días) =========
   const ingresosPorDia = {};
   for (const f of fact) {
       if (!f.fecha_emision) continue;
       const fecha = f.fecha_emision.substring(0,10);
       const monto = Number(f.total) || 0;
       ingresosPorDia[fecha] = (ingresosPorDia[fecha] || 0) + monto;
   }
   let diaLabels = Object.keys(ingresosPorDia).sort();
   if (diaLabels.length > 7) {
       diaLabels = diaLabels.slice(-7);
   }
   const diaData = diaLabels.map(d => ingresosPorDia[d]);

   const canvasDia = document.getElementById("ingresosDiaChart");
   if (canvasDia && diaLabels.length && window.Chart) {
       new Chart(canvasDia, {
           type: "line",
           data: {
               labels: diaLabels,
               datasets: [{
                   label: "Ingresos por día",
                   data: diaData,
                   tension: 0.3,
                   borderColor: "#e60023",
                   backgroundColor: "rgba(230,0,35,0.10)",
                   borderWidth: 3,
                   pointBackgroundColor: "#ffffff",
                   pointBorderColor: "#e60023",
                   pointRadius: 4
               }]
           },
           options: {animation:{duration:800,easing:'easeOutCubic'},
               responsive: true,
               maintainAspectRatio: false,
               scales: {
                   y: { beginAtZero: true }
               }
           }
       });
   }

   // ========= TOP CLIENTES QUE MÁS PAGAN =========
   const totalPorCliente = {};
   for (const f of fact) {
       if (!f.cliente_id) continue;
       const idCli = f.cliente_id;
       const monto = Number(f.total) || 0;
       totalPorCliente[idCli] = (totalPorCliente[idCli] || 0) + monto;
   }
   const topClientes = Object.entries(totalPorCliente)
       .sort((a,b) => b[1] - a[1])
       .slice(0,3);

   const ulTop = document.getElementById("topClientesLista");
   if (ulTop) {
       ulTop.innerHTML = "";
       if (!topClientes.length) {
           const li = document.createElement("li");
           li.textContent = "Sin datos de facturas todavía.";
           ulTop.appendChild(li);
       } else {
           for (const [idCliStr, monto] of topClientes) {
               const idCli = parseInt(idCliStr);
               const user = userMap[idCli];
               const li = document.createElement("li");
               li.innerHTML = `
                   <span class="top-cliente-nombre">${getUserDisplayName(user, idCli)}</span>
                   <span class="top-cliente-monto">₡${Math.round(monto).toLocaleString()}</span>
               `;
               ulTop.appendChild(li);
           }
       }
   }

   // ========= Citas por estado =========
   const estados = await fetch(`${API}/Estados`).then(r=>r.json());
   const mapEstados = {};
   for (const e of estados) {
       mapEstados[e.id_estado] = e.nombre;
   }

   const conteoEstados = {};
   for (const c of citas) {
       const idEst = c.id_estado;
       const nombre = mapEstados[idEst] || `Estado ${idEst}`;
       conteoEstados[nombre] = (conteoEstados[nombre] || 0) + 1;
   }

   const estadoCitasLabels = Object.keys(conteoEstados);
   const estadoCitasData = Object.values(conteoEstados);

   const canvasCitasEstado = document.getElementById("citasEstadoChart");
   if (canvasCitasEstado && estadoCitasLabels.length && window.Chart) {
       new Chart(canvasCitasEstado, {
           type: "doughnut",
           data: {
               labels: estadoCitasLabels,
               datasets: [{
                   data: estadoCitasData,
                   backgroundColor: ["#e60023","#ff9fb3","#ffccd5","#ffd6e0","#ffe6ea"],
                   borderColor: "#ffffff",
                   borderWidth: 2
               }]
           },
           options: {animation:{duration:800,easing:'easeOutCubic'},
               responsive: true,
               maintainAspectRatio: false,
               plugins: {
                   legend: {
                       position: "bottom"
                   }
               }
           }
       });
   }

   // ========= ACTIVIDAD RECIENTE =========
   const vehiculos = await fetch(`${API}/vehiculos`).then(r=>r.json());

   const actividades = [];

   if (citas.length) {
       let ultimaCita = citas[0];
       for (const c of citas) {
           if (c.id_agendamiento > ultimaCita.id_agendamiento) {
               ultimaCita = c;
           }
       }
       const user = userMap[ultimaCita.cliente_id];
       actividades.push({
           fecha: formatDate(ultimaCita.fecha_agregada),
           accion: "Cita agendada",
           usuario: getUserDisplayName(user, ultimaCita.cliente_id),
           detalle: "Nueva cita registrada en el sistema"
       });
   }

   if (fact.length) {
       let ultimaFactura = fact[0];
       for (const f of fact) {
           if (f.id_factura > ultimaFactura.id_factura) {
               ultimaFactura = f;
           }
       }
       const user = userMap[ultimaFactura.cliente_id];
       const total = Number(ultimaFactura.total) || 0;
       const estado = (ultimaFactura.estado_pago || "").toString();
       actividades.push({
           fecha: formatDate(ultimaFactura.fecha_emision),
           accion: estado === "Pagada" ? "Factura pagada" : "Factura generada",
           usuario: getUserDisplayName(user, ultimaFactura.cliente_id),
           detalle: `Monto: ₡${Math.round(total).toLocaleString()}`
       });
   }

   if (vehiculos.length) {
       let ultVeh = vehiculos[0];
       for (const v of vehiculos) {
           // intentar usar IdVehiculo, si no existe, usar id_vehiculo
           const idV = v.idVehiculo ?? v.id_vehiculo ?? 0;
           const idUlt = ultVeh.idVehiculo ?? ultVeh.id_vehiculo ?? 0;
           if (idV > idUlt) {
               ultVeh = v;
           }
       }
       const cliId = ultVeh.clienteId ?? ultVeh.cliente_id;
       const user = userMap[cliId];
       const detalle = `${ultVeh.placa || ""} ${ultVeh.modelo || ""}`.trim() || "Vehículo registrado";
       // No tenemos fecha de registro, usamos hoy como referencia visual
       actividades.push({
           fecha: new Date().toISOString().substring(0,10),
           accion: "Vehículo registrado",
           usuario: getUserDisplayName(user, cliId),
           detalle
       });
   }

   const tbody = document.getElementById("actividadRecienteBody");
   if (tbody) {
       tbody.innerHTML = "";
       if (!actividades.length) {
           const tr = document.createElement("tr");
           tr.innerHTML = `<td colspan="4">Sin actividad reciente.</td>`;
           tbody.appendChild(tr);
       } else {
           for (const act of actividades) {
               const tr = document.createElement("tr");
               tr.innerHTML = `
                   <td>${act.fecha || "-"}</td>
                   <td>${act.accion}</td>
                   <td>${act.usuario}</td>
                   <td>${act.detalle}</td>
               `;
               tbody.appendChild(tr);
           }
       }
   }

 }catch(e){
   console.error("ERROR DASHBOARD:", e);
 }
}


function initAdminCardsHover(){
  const cards = document.querySelectorAll('.admin-card');
  cards.forEach(card => {
    card.addEventListener('mouseenter', () => {
      card.classList.add('card-hover');
    });
    card.addEventListener('mouseleave', () => {
      card.classList.remove('card-hover');
    });
  });
}
