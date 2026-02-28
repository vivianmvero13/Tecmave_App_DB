
document.addEventListener('DOMContentLoaded', () => {
  const links = document.querySelectorAll('.sidebar .nav a[href]');
  const current = location.pathname.split('/').pop() || 'index.html';
  links.forEach(a => { const file = a.getAttribute('href')?.split('/').pop(); if(file===current) a.classList.add('active'); });
});
