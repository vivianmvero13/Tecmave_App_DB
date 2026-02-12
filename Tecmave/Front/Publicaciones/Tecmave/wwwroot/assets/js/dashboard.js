document.addEventListener('DOMContentLoaded', () => {
  const sidebar = document.querySelector('.sidebar');
  let overlay = document.querySelector('.sidebar-overlay');

  if (!sidebar) return;

  // Crear overlay si no existe
  if (!overlay) {
    overlay = document.createElement('div');
    overlay.className = 'sidebar-overlay';
    document.body.appendChild(overlay);
  }

  const setClosed = () => {
    sidebar.classList.remove('open');
    sidebar.style.transform = 'translateX(-100%)';
    sidebar.style.visibility = '';
    sidebar.style.opacity = '';
    overlay.classList.remove('active');
  };

  const setOpen = () => {
    sidebar.classList.add('open');
    sidebar.style.transform = 'translateX(0)';
    sidebar.style.visibility = 'visible';
    sidebar.style.opacity = '1';
    overlay.classList.add('active');
  };

  // Estado inicial: cerrado
  setClosed();

  const toggleSidebar = () => {
    const isOpen = sidebar.classList.contains('open');
    if (isOpen) {
      setClosed();
    } else {
      setOpen();
    }
  };

  // Delegación de eventos: cualquier click en un elemento con data-toggle-sidebar
  document.addEventListener('click', (ev) => {
    const toggleBtn = ev.target.closest('[data-toggle-sidebar]');
    if (toggleBtn) {
      ev.preventDefault();
      toggleSidebar();
    }
  });

  // Cerrar al hacer click en el overlay
  overlay.addEventListener('click', (ev) => {
    ev.preventDefault();
    setClosed();
  });
});
