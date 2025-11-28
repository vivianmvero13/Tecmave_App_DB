
(function(){
  const KEY='tecmave.footer';
  const def = {
    empresa:'TECMAVE CR',
    descripcion:'Taller bonito y cuidado, más de 15 años de experiencia en Costa Rica.',
    tel:'87059379',
    whatsapp:'50687059379',
    email:'info@tecmave.cr',
    direccion:'Bajo de Guadalupe, 25 m norte del cruce.',
    year:new Date().getFullYear()
  };

  const get = ()=> {
    try{
      return Object.assign({}, def, JSON.parse(localStorage.getItem(KEY) || '{}'));
    }catch(e){
      return { ...def };
    }
  };

  const set = (cfg)=>{
    localStorage.setItem(KEY, JSON.stringify(cfg));
    render();
  };

  function render(){
    const el = document.getElementById('site-footer');
    if (!el) return;
    const c = get();
    el.innerHTML = `
      <div class="footer-v2-main">
          <div class="container">
              <div class="footer-grid">
                  <div class="footer-col">
                      <img src="/assets/img/tecmave_logo_innovacion.jpg" alt="Tecmave Logo" class="footer-logo">
                      <p class="footer-slogan">${c.descripcion}</p>
                      <div class="footer-v2-socials">
                         <a href="#"><i class="fab fa-facebook-f"></i></a>
                         <a href="#"><i class="fab fa-instagram"></i></a>
                         <a href="https://wa.me/${c.whatsapp}"><i class="fab fa-whatsapp"></i></a>
                      </div>
                  </div>
                  <div class="footer-col">
                      <h4>Enlaces</h4>
                      <ul class="footer-list">
                          <li><a href="/">Inicio</a></li>
                          <li><a href="/Agendamiento/Index">Agendamiento</a></li>
                          <li><a href="/Contacto">Contacto</a></li>
                      </ul>
                  </div>
                  <div class="footer-col">
                      <h4>Contacto</h4>
                      <ul class="footer-list contact-info">
                          <li><i class="fas fa-map-marker-alt"></i> <span>${c.direccion}</span></li>
                          <li><i class="fas fa-phone-alt"></i> <span><a href="tel:${c.tel}">${c.tel}</a></span></li>
                          <li><i class="fas fa-envelope"></i> <span><a href="mailto:${c.email}">${c.email}</a></span></li>
                      </ul>
                  </div>
                  <div class="footer-col">
                      <h4>Newsletter</h4>
                      <form class="subscribe-form">
                          <input type="email" placeholder="Tu correo electrónico">
                          <button type="submit">Suscribirse</button>
                      </form>
                  </div>
              </div>
          </div>
      </div>
      <div class="footer-v2-bottom">
          <div class="container footer-bottom-grid">
              <p>© ${c.year} Todos los derechos reservados.</p>
              <p>Diseñado por ${c.empresa}</p>
          </div>
      </div>`;
  }

  function mountEditor(){
    const el = document.getElementById('site-footer');
    const isAdmin = document.documentElement.classList.contains('role-admin');
    if (!el || !isAdmin) return;

    const btn = document.getElementById('btn-edit-footer');
    if (!btn) return;

    btn.addEventListener('click', ()=>{
      const c = get();

      const overlay = document.createElement('div');
      overlay.className = 'footer-editor-overlay';
      overlay.innerHTML = `
        <div class="footer-editor-panel">
          <aside class="footer-editor-sidebar">
            <span>EDITAR FOOTER</span>
          </aside>
          <div class="footer-editor-content">
            <h3>Editar footer</h3>
            <div class="footer-editor-grid">
              <label>Empresa
                <input id="f-empresa" value="${c.empresa}">
              </label>
              <label>Teléfono
                <input id="f-tel" value="${c.tel}">
              </label>
              <label>WhatsApp (solo número)
                <input id="f-wa" value="${c.whatsapp}">
              </label>
              <label>Email
                <input id="f-mail" value="${c.email}">
              </label>
              <label class="full-row">Dirección
                <input id="f-dir" value="${c.direccion}">
              </label>
              <label class="full-row">Descripción
                <textarea id="f-desc" rows="3">${c.descripcion}</textarea>
              </label>
            </div>
            <div class="footer-editor-actions">
              <button id="f-save" class="btn">Guardar</button>
              <button id="f-cancel" class="btn secondary">Cancelar</button>
            </div>
          </div>
        </div>
      `;
      document.body.appendChild(overlay);

      const onClose = ()=> overlay.remove();

      overlay.querySelector('#f-cancel').onclick = onClose;
      overlay.addEventListener('click',(e)=>{
        if(e.target === overlay) onClose();
      });

      overlay.querySelector('#f-save').onclick = ()=>{
        const panel = overlay;
        set({
          empresa: panel.querySelector('#f-empresa').value || c.empresa,
          tel: panel.querySelector('#f-tel').value || c.tel,
          whatsapp: panel.querySelector('#f-wa').value || c.whatsapp,
          email: panel.querySelector('#f-mail').value || c.email,
          direccion: panel.querySelector('#f-dir').value || c.direccion,
          descripcion: panel.querySelector('#f-desc').value || c.descripcion,
          year: new Date().getFullYear()
        });
        onClose();
      };
    });
  }

  document.addEventListener('DOMContentLoaded', function(){
    render();
    mountEditor();
  });
})();
