
(function(){
  const carousels = document.querySelectorAll('.carousel,.slider,.banner');
  carousels.forEach(c => { c.querySelectorAll('img').forEach(img => { img.loading='lazy'; img.decoding='async'; }); });
  // Hide visuals for admin
  if(document.documentElement.classList.contains('role-admin')){
    carousels.forEach(c => c.style.display='none');
  }
})();
