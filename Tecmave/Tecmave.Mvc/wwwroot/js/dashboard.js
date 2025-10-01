
document.addEventListener('DOMContentLoaded', () => {
  const btn = document.querySelector('[data-toggle-sidebar]');
  const sidebar = document.querySelector('.sidebar');
  if (btn && sidebar){ btn.addEventListener('click', ()=> sidebar.classList.toggle('open')); }
});
