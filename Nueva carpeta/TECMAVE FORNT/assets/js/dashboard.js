
document.addEventListener('DOMContentLoaded', () => {
  const btn = document.querySelector('[data-toggle-sidebar]');
  const sidebar = document.querySelector('.sidebar');
  const main = document.querySelector('.main');
  let overlay = document.querySelector('.sidebar-overlay');

  if (!overlay) {
    overlay = document.createElement('div');
    overlay.className = 'sidebar-overlay';
    document.body.appendChild(overlay);
  }

  if (btn && sidebar) {
    btn.addEventListener('click', () => {
      sidebar.classList.toggle('open');
      overlay.classList.toggle('active');
    });

    overlay.addEventListener('click', () => {
      sidebar.classList.remove('open');
      overlay.classList.remove('active');
    });
  }
});
