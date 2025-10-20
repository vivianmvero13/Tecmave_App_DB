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
  if (sidebarNav && !sidebarNav.dataset.initialized) {
    sidebarNav.dataset.initialized = 'true'; // Set a flag to run only once per page
    const adminLinks = [
      { href: 'home.html', icon: 'fa-solid fa-house', text: 'Home' },
      { href: 'index.html', icon: 'fa-gauge-high', text: 'Panel' },
      { href: 'colaboradores.html', icon: 'fa-id-badge', text: 'Colaboradores' },
      { href: 'facturacion.html', icon: 'fa-file-invoice-dollar', text: 'Facturación' },
      { href: 'estadisticas.html', icon: 'fa-chart-line', text: 'Estadísticas' },
      { href: 'administracion.html', icon: 'fa-gear', text: 'Administración' },
      { href: 'usuarios.html', icon: 'fa-users', text: 'Usuarios' },
      { href: 'resenas-usuarios.html', icon: 'fa-comments', text: 'Reseñas Usuarios' },
    ];

    const clientLinks = [
      { href: 'home.html', icon: 'fa-solid fa-house', text: 'Home' },
      { href: 'index.html', icon: 'fa-gauge-high', text: 'Panel' },
      { href: 'vehiculos.html', icon: 'fa-car', text: 'Vehículos' },
      { href: 'clientes.html', icon: 'fa-user-group', text: 'Clientes' },
      { href: 'servicios.html', icon: 'fa-screwdriver-wrench', text: 'Servicios' },
      { href: 'agendamiento.html', icon: 'fa-calendar-check', text: 'Agendamiento' },
      { href: 'resenas.html', icon: 'fa-star', text: 'Reseñas' },
    ];

    const links = role === 'admin' ? adminLinks : clientLinks;

    // Clear existing content to prevent duplication
    sidebarNav.innerHTML = '';

    // Build the entire nav HTML string at once
    let navHTML = '';
    links.forEach(link => {
      navHTML += `<a href="${link.href}"><i class="fa-solid ${link.icon}"></i> ${link.text}</a>`;
    });

    // Append account section and links to the HTML string
    navHTML += `
      <div class="subtle">Cuenta</div>
      <a href="#"><i class="fa-solid fa-user"></i> Cuenta</a>
      <a href="#" id="logout-btn"><i class="fa-solid fa-right-from-bracket"></i> Cerrar sesión</a>
    `;

    // Replace the content in one go
    sidebarNav.innerHTML = navHTML;

    // Re-attach the event listener for the new logout button
    const logoutBtn = sidebarNav.querySelector('#logout-btn');
    if (logoutBtn) {
      logoutBtn.addEventListener('click', function(e){
        e.preventDefault();
        localStorage.removeItem('user');
        window.location.href = (location.protocol === 'file:' ? '../login.html' : '/login.html');
      });
    }
  }
})();