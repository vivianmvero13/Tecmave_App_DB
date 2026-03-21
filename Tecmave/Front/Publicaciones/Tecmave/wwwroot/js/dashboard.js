(function () {
  function initTecmaveSidebar() {
    if (window.__tecmaveSidebarInit) return;

    const sidebar = document.querySelector('.sidebar');
    const topbar = document.querySelector('.topbar');
    const root = document.documentElement;
    if (!sidebar) return;

    let overlay = document.querySelector('.sidebar-overlay');
    if (!overlay) {
      overlay = document.createElement('div');
      overlay.className = 'sidebar-overlay';
      document.body.appendChild(overlay);
    }

    const syncShellMetrics = () => {
      if (!topbar) return;
      const h = Math.ceil(topbar.getBoundingClientRect().height || 64);
      root.style.setProperty('--shell-topbar-h', `${h}px`);
    };

    const setClosed = () => {
      sidebar.classList.remove('open');
      overlay.classList.remove('active');
      sidebar.setAttribute('aria-hidden', 'true');
    };

    const setOpen = () => {
      syncShellMetrics();
      sidebar.classList.add('open');
      overlay.classList.add('active');
      sidebar.setAttribute('aria-hidden', 'false');
    };

    const toggleSidebar = (ev) => {
      if (ev) {
        ev.preventDefault();
        ev.stopPropagation();
      }
      if (sidebar.classList.contains('open')) {
        setClosed();
      } else {
        setOpen();
      }
    };

    const directBtn = document.getElementById('sidebarToggle') || document.querySelector('[data-toggle-sidebar]');
    if (directBtn) {
      directBtn.addEventListener('click', toggleSidebar, { passive: false });
      directBtn.addEventListener('touchstart', toggleSidebar, { passive: false });
    }

    document.addEventListener('click', (ev) => {
      const toggleBtn = ev.target.closest('[data-toggle-sidebar]');
      if (toggleBtn) {
        toggleSidebar(ev);
      }
    });

    overlay.addEventListener('click', (ev) => {
      ev.preventDefault();
      setClosed();
    });

    document.addEventListener('keydown', (ev) => {
      if (ev.key === 'Escape') setClosed();
    });

    syncShellMetrics();
    window.addEventListener('resize', syncShellMetrics);
    window.addEventListener('orientationchange', syncShellMetrics);
    if (document.fonts && document.fonts.ready) {
      document.fonts.ready.then(syncShellMetrics).catch(() => {});
    }
    requestAnimationFrame(syncShellMetrics);
    setTimeout(syncShellMetrics, 0);
    setClosed();

    window.__tecmaveSidebarInit = true;
    window.TecmaveSidebar = { open: setOpen, close: setClosed, toggle: toggleSidebar, sync: syncShellMetrics };
  }

  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', initTecmaveSidebar, { once: true });
  } else {
    initTecmaveSidebar();
  }
})();
