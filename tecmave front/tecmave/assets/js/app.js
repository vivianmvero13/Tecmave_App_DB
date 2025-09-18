

const PUBLIC_PAGES = ["/login.html", "/registro.html", "/logout.html", "/"];
const ROUTES = {
  adminOnly: [
    "/public/roles.html",
    "/public/colaboradores.html",
    "/public/administracion.html",
    "/public/facturacion.html",
    "/public/estadisticas.html"
  ],
  clientOnly: [
    "/public/clientes.html",
    "/public/resenas.html",
    "/public/asistencia.html",
    "/public/vehiculos.html",
    "/public/agendamiento.html"
  ]
};

function getUser() {

  try {
    const raw = 
document.addEventListener('DOMContentLoaded', () => { applyRoleVisibility(); guardAccess();
    const user = JSON.parse(localStorage.getItem('user'));
    const header = document.getElementById('site-header');
    if (user && user.role) {
        let roleName = user.role.charAt(0).toUpperCase() + user.role.slice(1);
        let navItems = `<nav><ul>`;
        navItems += `<li><a href='index.html'>Inicio</a></li>`;
        if (user.role === 'admin') {
            navItems += `<li><a href='admin.html'>Administración</a></li>`;
        }
        if (user.role === 'cliente') {
            navItems += `<li><a href='cliente.html'>Servicios</a></li>`;
        }
        navItems += `<li><a href='#' id='logout-btn'>Cerrar sesión (${roleName})</a></li>`;
        navItems += `</ul></nav>`;
        header.innerHTML = navItems;

        document.getElementById('logout-btn').addEventListener('click', () => {
            localStorage.removeItem('user');
            window.location.href = 'login.html';
        });
    } else {
        header.innerHTML = "<nav><ul><li><a href='login.html'>Iniciar sesión</a></li></ul></nav>";
    }
});
;
    if (raw) return JSON.parse(raw);
  } catch(e) {}
  const m = location.hash && location.hash.match(/user=([^&]+)/);
  if (m && m[1]) {
    try {
      const decoded = JSON.parse(atob(decodeURIComponent(m[1])));
      localStorage.setItem('user', JSON.stringify(decoded));
      history.replaceState(null, '', location.pathname + location.search);
      return decoded;
    } catch(e) {}
  }
  return null;
} catch { return null; }
}
function setUser(u) { localStorage.setItem("user", JSON.stringify(u)); }
function clearUser() { localStorage.removeItem("user"); }

function normalizePath() { return window.location.pathname.replace(/\/+$/, "") || "/"; }


function guardAccess() {
  const user = getUser();
  const path = location.pathname;
  const isPublic = /\/public\//.test(path) || path.endsWith('/public') || path.endsWith('public');
  if (!user && isPublic) {
    window.location.href = (location.protocol === 'file:' ? '../login.html' : '/login.html');
    return;
  }
}

  if (user) {
    const isAdminOnly = ROUTES.adminOnly.some(r => path.endsWith(r) || (isFile && path.includes('/public/') && r.replace('/public','').endsWith(path.split('/public')[-1] || '')));
    const isClientOnly = ROUTES.clientOnly.some(r => path.endsWith(r) || (isFile && path.includes('/public/') && r.replace('/public','').endsWith(path.split('/public')[-1] || '')));

    if (isAdminOnly && user.role !== "admin") {
      alert("Acceso no autorizado para clientes.");
      const toClient = isFile ? (inPublic ? 'clientes.html' : 'public/clientes.html') : '/public/clientes.html';
      window.location.replace(toClient);
      return;
    }
    if (isClientOnly && user.role !== "cliente") {
      alert("Acceso no autorizado para administradores.");
      const toAdmin = isFile ? (inPublic ? 'administracion.html' : 'public/administracion.html') : '/public/administracion.html';
      window.location.replace(toAdmin);
      return;
    }
  }
}

  if (user) {
    if (ROUTES.adminOnly.includes(path) && user.role !== "admin") {
      alert("Acceso no autorizado para clientes.");
      window.location.replace("/public/clientes.html");
      return;
    }
    if (ROUTES.clientOnly.includes(path) && user.role !== "cliente") {
      alert("Acceso no autorizado para administradores.");
      window.location.replace("/public/administracion.html");
      return;
    }
  }
}


        </div>
      </li>
    </ul>
  </div>
  `;
}

        </div>
      </li>
    </ul>
  </div>
  `;
}

function footerTemplate() {
  return `
  <div class="container">
    <div class="grid cols-3">
      <div>
        <h3>TECMAVE CR</h3>
        <p class="help">Soluciones para logística y mantenimiento en Costa Rica.</p>
      </div>
      <div>
        <h3>Contacto</h3>
        <p class="help">Tel: <a class="link" href="tel:+50688888888">+506 8888-8888</a></p>
        <p class="help">WhatsApp: <a class="link" href="https://wa.me/50688888888" target="_blank">wa.me/50688888888</a></p>
        <p class="help">Email: <a class="link" href="mailto:info@tecmave.cr">info@tecmave.cr</a></p>
      </div>
      <div>
        <h3>Horario</h3>
        <p class="help">Lunes a Viernes: 7:00 AM - 6:00 PM</p>
        <p class="help">Sábados: 8:00 AM - 12:00 PM</p>
      </div>
    </div>
    <p class="help" style="margin-top:1rem">© 2025 TECMAVE CR. Todos los derechos reservados.</p>
  </div>`;
}


  const logoutBtn = document.getElementById("logout-btn");
  if (logoutBtn) {
    logoutBtn.addEventListener("click", (e) => {
      e.preventDefault();
      clearUser();
      window.location.replace("/login.html");
    });
  }
}

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

function mostrarSeccion(id) {
  const secciones = document.querySelectorAll('[id^="seccion-"]');
  secciones.forEach(s => s.style.display = "none");
  const activa = document.getElementById("seccion-" + id);
  if (activa) activa.style.display = "block";
}
function wireSectionLinks() {
  document.querySelectorAll("a[data-seccion]").forEach(link => {
    link.addEventListener("click", (e) => {
      e.preventDefault();
      const id = e.currentTarget.getAttribute("data-seccion");
      mostrarSeccion(id);
    });
  });
}

function toast(message, duration = 3000) {
  const toast = document.createElement("div");
  toast.className = "toast";
  toast.textContent = message;
  document.body.appendChild(toast);
  requestAnimationFrame(() => { toast.style.opacity = "1"; });
  setTimeout(() => {
    toast.style.opacity = "0";
    setTimeout(() => document.body.removeChild(toast), 300);
  }, duration);
}


  if (!document.getElementById('site-footer')) {
    const f = document.createElement('footer');
    f.id = 'site-footer';
    f.className = 'footer';
    document.body.appendChild(f);
  }
}
document.addEventListener("DOMContentLoaded", () => {
  guardAccess();
  injectLayout();
  initCarousel();
  wireSectionLinks();
  if (document.getElementById("seccion-inicio")) mostrarSeccion("inicio");
});

window.toast = toast;
window.mostrarSeccion = mostrarSeccion;

function applyRoleVisibility() {
  const user = getUser();
  const adminEls = document.querySelectorAll('.admin-only');
  const clientEls = document.querySelectorAll('.client-only');
  if (user) {
    const nameSpan = document.getElementById('current-user');
    if (nameSpan) nameSpan.textContent = `${user.name} (${user.role})`;
    const loginLink = document.getElementById('fallback-login-link');
    const logoutBtn = document.getElementById('logout-btn');
    if (loginLink) loginLink.style.display = 'none';
    if (logoutBtn) logoutBtn.style.display = 'block';
    if (user.role === 'admin') clientEls.forEach(el => el.style.display = 'none');
    if (user.role === 'cliente') adminEls.forEach(el => el.style.display = 'none');
    if (logoutBtn) logoutBtn.addEventListener('click', (e) => {
      e.preventDefault();
      localStorage.removeItem('user');
      window.location.href = (location.protocol === 'file:' ? '../login.html' : '/login.html');
    });
  } else {
    [...adminEls, ...clientEls].forEach(el => el.style.display = 'none');
  }
}
