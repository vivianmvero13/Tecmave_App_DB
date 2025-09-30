
(function(){
  if(!/login\.html$/i.test(location.pathname)) return;
  const form = document.getElementById('loginForm');
  if(!form) return;
  form.addEventListener('submit', () => {
    const name = document.querySelector('#login_name, input[name=name], input[name=nombre], input[name=username]')?.value;
    const email = document.getElementById('login_email')?.value;
    if(name){ localStorage.setItem('displayName', name); }
    if(email){ localStorage.setItem('email', email); }
  }, {capture:true});
})();
