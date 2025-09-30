(function(){
  function parseJSON(v){try{return JSON.parse(v)}catch(e){return null}}
  const u = parseJSON(localStorage.getItem('user')) || parseJSON(sessionStorage.getItem('user')) || {};
  const role = (u.role || u.rol || u.perfil || u.tipo || '').toLowerCase() || 'cliente';
  const name = u.name || u.nombre || u.displayName || (u.email?u.email.split('@')[0]:null) || localStorage.getItem('displayName') || localStorage.getItem('email')?.split('@')[0] || 'Usuario';

  document.documentElement.classList.add(role==='admin'?'role-admin':'role-client');
  const span = document.getElementById('user-display-name'); if(span) span.textContent = name;

  // Add logo if absent
  const siteHeader = document.getElementById('site-header');
  if(siteHeader && !siteHeader.querySelector('img.logo-xs')){
    const img = document.createElement('img');
    img.src = '../assets/img/tecmave_logo_innovacion.jpg';
    img.alt='Tecmave'; img.className='logo-xs';
    img.style.width='20px'; img.style.height='20px'; img.style.borderRadius='6px'; img.style.marginRight='6px';
    siteHeader.prepend(img);
  }

  // Role-specific panel widgets (index only)
  const page = location.pathname.split('/').pop() || 'index.html';
  if(page==='index.html'){
    const main = document.querySelector('main.main') || document.querySelector('main');
    if(main && !document.getElementById('panel-role-widgets')){
      const wrap = document.createElement('section'); wrap.id='panel-role-widgets';
      wrap.innerHTML = `
        <div id="panel-admin" class="role-only-admin" style="margin-bottom:12px;">
          <h2 style="margin:0 0 12px 0;">Panel de Administración</h2>
          <div class="admin-dashboard-grid">
            <div class="admin-dashboard-card">
                <h3>Ingresos Totales</h3>
                <div class="value">₡5.4M</div>
                <div class="change positive">+12% desde el mes pasado</div>
            </div>
            <div class="admin-dashboard-card">
                <h3>Citas Pendientes</h3>
                <div class="value">8</div>
                <div class="change">2 nuevas hoy</div>
            </div>
            <div class="admin-dashboard-card">
                <h3>Nuevos Clientes</h3>
                <div class="value">23</div>
                <div class="change positive">+5% esta semana</div>
            </div>
          </div>

          <div class="admin-dashboard-table-container">
            <h3>Actividad Reciente</h3>
            <table class="table">
              <thead>
                <tr>
                  <th>Fecha</th>
                  <th>Acción</th>
                  <th>Usuario</th>
                  <th>Detalle</th>
                </tr>
              </thead>
              <tbody>
                <tr>
                  <td>2025-07-30</td>
                  <td>Cita Agendada</td>
                  <td>Juan Pérez</td>
                  <td>Mantenimiento preventivo</td>
                </tr>
                <tr>
                  <td>2025-07-29</td>
                  <td>Factura Pagada</td>
                  <td>María García</td>
                  <td>Servicio de frenos</td>
                </tr>
                <tr>
                  <td>2025-07-28</td>
                  <td>Vehículo Registrado</td>
                  <td>Pedro López</td>
                  <td>Toyota Corolla ABC123</td>
                </tr>
              </tbody>
            </table>
          </div>

          <div class="admin-dashboard-chart-container">
            <h3>Servicios Populares</h3>
            <canvas id="adminChartServicios"></canvas>
          </div>
        </div>
        <div id="panel-cliente" class="role-only-client" style="margin-bottom:12px;">
          <h2 style="margin:0 0 12px 0;">Tu taller hoy</h2>
          <div class="grid" style="grid-template-columns: repeat(3, minmax(260px,1fr));">
            <div class="card"><div class="kpi"><div><div class="subtle">Próxima cita</div><div class="value">Jue 10:30</div></div><span class="badge ok">Confirmada</span></div></div>
            <div class="card"><div class="kpi"><div><div class="subtle">Mis vehículos</div><div class="value">3</div></div><span class="badge">2 en taller</span></div></div>
            <div class="card"><div class="kpi"><div><div class="subtle">Balance del mes</div><div class="value">₡180.000</div></div><span class="badge">Pagado</span></div></div>
          </div>
        </div>`;
      main.prepend(wrap);
      const isAdmin = role==='admin';
      document.getElementById('panel-admin').style.display = isAdmin?'':'none';
      document.getElementById('panel-cliente').style.display = isAdmin?'none':'';

      // Initialize admin chart if it exists
      if (isAdmin) {
        const ctx = document.getElementById('adminChartServicios')?.getContext('2d');
        if (ctx) {
          new Chart(ctx, {
            type: 'bar',
            data: {
              labels: ['Aceite', 'Frenos', 'Alineación', 'Diagnóstico', 'Batería'],
              datasets: [{
                label: 'Servicios Realizados',
                data: [45, 30, 20, 15, 10],
                backgroundColor: [
                  'rgba(37, 99, 235, 0.8)',
                  'rgba(239, 68, 68, 0.8)',
                  'rgba(34, 197, 94, 0.8)',
                  'rgba(245, 158, 11, 0.8)',
                  'rgba(148, 163, 184, 0.8)'
                ],
                borderColor: [
                  'rgba(37, 99, 235, 1)',
                  'rgba(239, 68, 68, 1)',
                  'rgba(34, 197, 94, 1)',
                  'rgba(245, 158, 11, 1)',
                  'rgba(148, 163, 184, 1)'
                ],
                borderWidth: 1,
                borderRadius: 8
              }]
            },
            options: {
              responsive: true,
              maintainAspectRatio: false,
              plugins: {
                legend: {
                  labels: {
                    color: '#e6ebf5' // text color from dashboard.css --text
                  }
                }
              },
              scales: {
                x: {
                  grid: {
                    color: 'rgba(255,255,255,0.08)' // grid color from dashboard.css
                  },
                  ticks: {
                    color: '#a8b3c7' // muted color from dashboard.css
                  }
                },
                y: {
                  grid: {
                    color: 'rgba(255,255,255,0.08)' // grid color from dashboard.css
                  },
                  ticks: {
                    color: '#a8b3c7', // muted color from dashboard.css
                    beginAtZero: true
                  }
                }
              }
            }
          });
        }
      }
    }
  }

  // Update sidebar navigation based on role
  const sidebarNav = document.querySelector('.sidebar .nav');
  if (sidebarNav) {
    // Remove existing login/logout links to re-add them correctly
    const loginLink = document.getElementById('login-link');
    const logoutBtn = document.getElementById('logout-btn');
    if (loginLink) loginLink.remove();
    if (logoutBtn) logoutBtn.remove();

    // Add common links
    const commonLinks = `
      <a href="index.html"><i class="fa-solid fa-gauge-high"></i> Panel</a>
      <a href="vehiculos.html" class="client-only"><i class="fa-solid fa-car"></i> Vehículos</a>
      <a href="clientes.html" class="client-only"><i class="fa-solid fa-user-group"></i> Clientes</a>
      <a href="servicios.html" class="client-only"><i class="fa-solid fa-screwdriver-wrench"></i> Servicios</a>
      <a href="agendamiento.html" class="client-only"><i class="fa-solid fa-calendar-check"></i> Agendamiento</a>
      <a href="resenas.html" class="client-only"><i class="fa-solid fa-star"></i> Reseñas</a> <!-- New client link -->
    `;

    const adminLinks = `
  <a href="colaboradores.html" class="admin-only"><i class="fa-solid fa-id-badge"></i> Colaboradores</a>
  <a href="facturacion.html" class="admin-only"><i class="fa-solid fa-file-invoice-dollar"></i> Facturación</a>
  <a href="estadisticas.html" class="admin-only"><i class="fa-solid fa-chart-line"></i> Estadísticas</a>
  <a href="administracion.html" class="admin-only"><i class="fa-solid fa-gear"></i> Administración</a>
  <a href="usuarios.html" class="admin-only"><i class="fa-solid fa-users"></i> Usuarios</a>
  <a href="resenas-usuarios.html" class="admin-only"><i class="fa-solid fa-comments"></i> Reseñas Usuarios</a>
`;

    sidebarNav.innerHTML = commonLinks + adminLinks + `<div class="subtle">Cuenta</div>` + `
      <a id="logout-btn" href="#"><i class="fa-solid fa-right-from-bracket"></i> Cerrar sesión</a>
    `;

    // Re-apply role-based visibility
    const allClientEls = sidebarNav.querySelectorAll('.client-only');
    const allAdminEls = sidebarNav.querySelectorAll('.admin-only');

    if (role === 'admin') {
      allClientEls.forEach(el => el.style.display = 'none');
      allAdminEls.forEach(el => el.style.display = 'flex'); // Ensure admin links are visible
    } else if (role === 'cliente') {
      allAdminEls.forEach(el => el.style.display = 'none');
      allClientEls.forEach(el => el.style.display = 'flex'); // Ensure client links are visible
    } else {
      // If no role or unknown role, hide all role-specific links
      allClientEls.forEach(el => el.style.display = 'none');
      allAdminEls.forEach(el => el.style.display = 'none');
    }

    // Handle login/logout button visibility
    const currentLoginLink = document.getElementById('login-link');
    const currentLogoutBtn = document.getElementById('logout-btn');
    if (u.loggedIn) {
      if (currentLoginLink) currentLoginLink.style.display = 'none';
      if (currentLogoutBtn) {
        currentLogoutBtn.style.display = 'flex';
        currentLogoutBtn.addEventListener('click', function(e){
          e.preventDefault();
          localStorage.removeItem('user');
          window.location.href = (location.protocol === 'file:' ? '../login.html' : '/login.html');
        });
      }
    } else {
      if (currentLoginLink) currentLoginLink.style.display = 'flex';
      if (currentLogoutBtn) currentLogoutBtn.style.display = 'none';
    }
  }
})();