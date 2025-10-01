(function(){
  const KEY='tecmave.footer';
  const def = {empresa:'TECMAVE CR',descripcion:'Taller de servicio automotriz y asistencia en carretera.\nMás de 15 años de experiencia en Costa Rica.',direccion:'Curridabat, 100 m este de La Galera',tel:'+506 2285-6409',whatsapp:'50688888888',email:'info@tecmave.cr',year:new Date().getFullYear()};
  const get = ()=>{try{return Object.assign({},def, JSON.parse(localStorage.getItem(KEY)||'{}'))}catch(e){return {...def}}};
  const set = (cfg)=>{localStorage.setItem(KEY, JSON.stringify(cfg)); render()};
  function render(){
    const el = document.getElementById('site-footer'); if(!el) return;
    const c = get();
    el.innerHTML = `
      <div class="footer-v2-main">
          <div class="container">
              <div class="footer-grid">
                  <div class="footer-col">
                      <img src="../assets/img/tecmave_logo_innovacion.jpg" alt="Tecmave Logo" class="footer-logo">
                      <p class="footer-slogan">${c.descripcion}</p>
                      <div class="footer-v2-socials">
                          <a href="#"><i class="fab fa-facebook-f"></i></a>
                          <a href="#"><i class="fab fa-twitter"></i></a>
                          <a href="#"><i class="fab fa-linkedin-in"></i></a>
                          <a href="#"><i class="fab fa-instagram"></i></a>
                          <a href="#"><i class="fab fa-pinterest"></i></a>
                          <a href="#"><i class="fab fa-youtube"></i></a>
                          <a href="#"><i class="fas fa-rss"></i></a>
                      </div>
                  </div>
                  <div class="footer-col">
                      <h4>Quick Links</h4>
                      <ul class="footer-list">
                          <li><a href="index.html">Home</a></li>
                          <li><a href="#">List Layout</a></li>
                          <li><a href="informacion.html">Blog</a></li>
                          <li><a href="contacto.html">Contact</a></li>
                      </ul>
                  </div>
                  <div class="footer-col">
                      <h4>Contact Us</h4>
                      <ul class="footer-list contact-info">
                          <li><i class="fas fa-map-marker-alt"></i> <span>${c.direccion}</span></li>
                          <li><i class="fas fa-phone-alt"></i> <span><a href="tel:${c.tel}">${c.tel}</a></span></li>
                          <li><i class="fas fa-envelope"></i> <span><a href="mailto:${c.email}">${c.email}</a></span></li>
                      </ul>
                  </div>
                  <div class="footer-col">
                      <h4>Remain Updated</h4>
                      <form class="subscribe-form">
                          <input type="email" placeholder="Your email address">
                          <button type="submit">Sign up</button>
                      </form>
                  </div>
              </div>
          </div>
      </div>
      <div class="footer-v2-bottom">
          <div class="container">
              <p>© ${c.year} All rights reserved.</p>
              <p>Designed by ${c.empresa}</p>
          </div>
      </div>`;
  }
  function mountEditor(){
    const isAdmin = document.documentElement.classList.contains('role-admin');
    const el = document.getElementById('site-footer');
    if(!isAdmin || !el) return;
    const btn = document.createElement('button'); btn.textContent='Editar footer'; btn.className='btn secondary'; btn.style.margin='8px 0 12px';
    el.parentElement.insertBefore(btn, el);
    btn.addEventListener('click', ()=>{
      const c = get();
      const panel = document.createElement('div');
      panel.innerHTML = `
      <div class="card" style="padding:16px">
        <h3 style="margin:0 0 8px 0">Editar footer</h3>
        <div class="grid" style="grid-template-columns: 1fr 1fr; gap:12px">
          <label>Empresa <input id="f-empresa" value="${c.empresa}"></label>
          <label>Teléfono <input id="f-tel" value="${c.tel}"></label>
          <label>WhatsApp (solo número) <input id="f-wa" value="${c.whatsapp}"></label>
          <label>Email <input id="f-mail" value="${c.email}"></label>
          <label style="grid-column:1/-1">Dirección <input id="f-dir" value="${c.direccion}"></label>
          <label style="grid-column:1/-1">Descripción <textarea id="f-desc" rows="3">${c.descripcion}</textarea></label>
        </div>
        <div style="display:flex;gap:8px;justify-content:flex-end;margin-top:10px">
          <button id="f-save" class="btn">Guardar</button>
          <button id="f-cancel" class="btn secondary">Cancelar</button>
        </div>
      </div>`;
      btn.insertAdjacentElement('afterend', panel);
      panel.querySelector('#f-cancel').onclick = ()=> panel.remove();
      panel.querySelector('#f-save').onclick = ()=>{
        set({
          empresa: panel.querySelector('#f-empresa').value || c.empresa,
          tel: panel.querySelector('#f-tel').value || c.tel,
          whatsapp: panel.querySelector('#f-wa').value || c.whatsapp,
          email: panel.querySelector('#f-mail').value || c.email,
          direccion: panel.querySelector('#f-dir').value || c.direccion,
          descripcion: panel.querySelector('#f-desc').value || c.descripcion,
          year: new Date().getFullYear()
        });
        panel.remove();
      };
    });
  }
  const page = location.pathname.split('/').pop() || 'index.html';
  if(page==='index.html'){ render(); mountEditor(); }
})();