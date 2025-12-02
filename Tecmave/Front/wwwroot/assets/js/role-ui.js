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
          <h2 style="margin:0 0 12px 0;"><span data-key="admin_panel_title">Panel de Administración</span></h2>
          <div class="admin-dashboard-grid">
            <div class="admin-dashboard-card enhanced-card">
                <div class="card-icon">
                  <i class="fas fa-chart-line"></i>
                </div>
                <h3 data-key="stats_total_income">Ingresos Totales</h3>
                <div class="value">₡5.4M</div>
                <div class="change positive">+12% desde el mes pasado</div>
                <div class="card-glow"></div>
            </div>
            <div class="admin-dashboard-card enhanced-card">
                <div class="card-icon">
                  <i class="fas fa-calendar-check"></i>
                </div>
                <h3 data-key="admin_pending_appointments">Citas Pendientes</h3>
                <div class="value">8</div>
                <div class="change">2 nuevas hoy</div>
                <div class="card-glow"></div>
            </div>
            <div class="admin-dashboard-card enhanced-card">
                <div class="card-icon">
                  <i class="fas fa-users"></i>
                </div>
                <h3 data-key="stats_new_clients">Nuevos Clientes</h3>
                <div class="value">23</div>
                <div class="change positive">+5% esta semana</div>
                <div class="card-glow"></div>
            </div>
          </div>

          <div class="admin-dashboard-table-container enhanced-card">
            <h3 data-key="admin_recent_activity">Actividad Reciente</h3>
            <table class="table">
              <thead>
                <tr>
                  <th data-key="th_date">Fecha</th>
                  <th data-key="th_action">Acción</th>
                  <th data-key="th_user">Usuario</th>
                  <th data-key="th_detail">Detalle</th>
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

          <div class="charts-container">
            <div class="admin-dashboard-chart-container enhanced-card metrics-chart-card">
              <h3 data-key="admin_payment_metrics">Métricas de Pagos</h3>
              <div class="metrics-chart">
                <div class="metric-item">
                  <div class="metric-label" data-key="admin_payments_started">Pagos Iniciados</div>
                  <div class="metric-value">65.2k</div>
                  <div class="metric-bar">
                    <div class="metric-fill" style="width: 100%"></div>
                  </div>
                </div>
                <div class="metric-item">
                  <div class="metric-label" data-key="admin_payments_authorized">Pagos Autorizados</div>
                  <div class="metric-value">54.8k</div>
                  <div class="metric-bar">
                    <div class="metric-fill" style="width: 84%"></div>
                  </div>
                </div>
                <div class="metric-item highlight">
                  <div class="metric-label" data-key="admin_payments_successful">Pagos Exitosos</div>
                  <div class="metric-value">48.6k</div>
                  <div class="metric-bar">
                    <div class="metric-fill" style="width: 74%"></div>
                  </div>
                </div>
                <div class="metric-item highlight">
                  <div class="metric-label" data-key="admin_payments_merchants">Pagos a Comerciantes</div>
                  <div class="metric-value">38.3k</div>
                  <div class="metric-bar">
                    <div class="metric-fill" style="width: 59%"></div>
                  </div>
                </div>
                <div class="metric-item">
                  <div class="metric-label" data-key="admin_transactions_completed">Transacciones Completadas</div>
                  <div class="metric-value">32.9k</div>
                  <div class="metric-bar">
                    <div class="metric-fill" style="width: 50%"></div>
                  </div>
                </div>
              </div>
              <div class="chart-insight">
                <div class="insight-header">68.2k: ¿Qué te gustaría explorar después?</div>
                <div class="insight-text">Quiero saber qué causó la caída de autorizados a pagos exitosos</div>
              </div>
            </div>

            <div class="admin-dashboard-chart-container enhanced-card bar-chart-card">
              <h3 data-key="admin_overview">Visión General</h3>
              <div class="bar-chart-container">
                <div class="bar-chart">
                  <div class="bar-item">
                    <div class="bar-label" data-key="admin_retention">Retención</div>
                    <div class="bar">
                      <div class="bar-fill retention" style="height: 42%"></div>
                    </div>
                    <div class="bar-value">42%</div>
                  </div>
                  <div class="bar-item">
                    <div class="bar-label" data-key="admin_transactions">Transacciones</div>
                    <div class="bar">
                      <div class="bar-fill transactions" style="height: 85%"></div>
                    </div>
                    <div class="bar-value">106k</div>
                  </div>
                  <div class="bar-item">
                    <div class="bar-label" data-key="admin_volume">Volumen</div>
                    <div class="bar">
                      <div class="bar-fill volume" style="height: 65%"></div>
                    </div>
                    <div class="bar-value">$41.5M</div>
                  </div>
                  <div class="bar-item">
                    <div class="bar-label">Insights</div>
                    <div class="bar">
                      <div class="bar-fill insights" style="height: 75%"></div>
                    </div>
                    <div class="bar-value">75%</div>
                  </div>
                </div>
              </div>
              <div class="volume-breakdown">
                <div class="breakdown-item">
                  <span class="breakdown-dot online"></span>
                  <span>Pagos Online</span>
                </div>
                <div class="breakdown-item">
                  <span class="breakdown-dot subscriptions"></span>
                  <span>Suscripciones</span>
                </div>
                <div class="breakdown-item">
                  <span class="breakdown-dot instore"></span>
                  <span>Ventas en Tienda</span>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div id="panel-cliente" class="role-only-client" style="margin-bottom:12px;">
          <h2 style="margin:0 0 12px 0;"><span data-key="client_today_title">Tu taller hoy</span></h2>
          <div class="grid" style="grid-template-columns: repeat(3, minmax(260px,1fr));">
            <div class="card"><div class="kpi"><div><div class="subtle" data-key="client_next_appointment">Próxima cita</div><div class="value">Jue 10:30</div></div><span class="badge ok" data-key="client_confirmed">Confirmada</span></div></div>
            <div class="card"><div class="kpi"><div><div class="subtle" data-key="client_my_vehicles_label">Mis vehículos</div><div class="value">3</div></div><span class="badge" data-key="client_in_workshop">2 en taller</span></div></div>
            <div class="card"><div class="kpi"><div><div class="subtle" data-key="client_month_balance">Balance del mes</div><div class="value">₡180.000</div></div><span class="badge" data-key="client_paid">Pagado</span></div></div>
          </div>
        </div>`;
      main.prepend(wrap);
      const isAdmin = role==='admin';
      document.getElementById('panel-admin').style.display = isAdmin?'':'none';
      document.getElementById('panel-cliente').style.display = isAdmin?'none':'';
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