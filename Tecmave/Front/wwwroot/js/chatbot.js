(function () {
    // ====== Guard: solo clientes ======
    const user = safeJsonParse(localStorage.getItem('user')) || {};
    const lang = localStorage.getItem('language') || 'es';

    if (String(user.role || '').toLowerCase() !== 'cliente') return;

    // ====== Config del taller (ajústalo aquí) ======
    const CONFIG = {
        PHONE: '+50622859379', // sin espacios para tel:
        DISPLAY_PHONE: '+506 2285-9379',
        WHATSAPP: '50622859379', // wa.me necesita solo dígitos
        LOCATION_TEXT: 'Alto de Guadalupe, San José, Costa Rica',
        MAPS_URL: 'https://www.google.com/maps/search/?api=1&query=Alto%20de%20Guadalupe%2C%20San%20Jos%C3%A9%2C%20Costa%20Rica',
        HOURS: {
            weekdays: '8:00 a.m. – 6:00 p.m.',
            saturday: '8:00 a.m. – 3:00 p.m.',
            sunday: 'Emergencias con coordinación previa.'
        },
        // Si tienes una ruta interna o página para agendar, ponla aquí.
        APPOINTMENT_URL: null // e.g. '/Agendamiento' o 'https://...'
    };

    // ====== Textos multi-idioma ======
    const I18N = {
        es: {
            assistantName: 'Asistente TECMAVE',
            online: 'En línea • Listo para ayudar',
            placeholder: 'Escribe tu mensaje aquí...',
            send: 'Enviar',
            welcome:
                `👋 **¡Hola! Soy tu asistente de TECMAVE.**\n\n` +
                `Puedo ayudarte con:\n` +
                `• 📞 Información de contacto\n` +
                `• 🕒 Horarios\n` +
                `• 📍 Ubicación\n` +
                `• 📅 Agendar cita\n` +
                `• 🔧 Servicios\n\n` +
                `¿Qué necesitas?`,
            menuButtons: [
                { label: '📞 Información de contacto', intent: 'CONTACT' },
                { label: '🕒 Horarios', intent: 'HOURS' },
                { label: '📍 Ubicación', intent: 'LOCATION' },
                { label: '📅 Quiero agendar cita', intent: 'APPOINTMENT' },
                { label: '🔧 Servicio', intent: 'SERVICES' }
            ],
            backMenu: '⬅️ Volver al menú',
            contactTitle: `📞 **CONTACTO**`,
            hoursTitle: `🕒 **HORARIO DE ATENCIÓN**`,
            locationTitle: `📍 **UBICACIÓN**`,
            appointmentTitle: `📅 **AGENDAR CITA**`,
            servicesTitle: `🔧 **SERVICIOS TECMAVE**`,
            fallback:
                `🤝 **Gracias por tu mensaje.**\n\n` +
                `Para ayudarte más rápido, elige una opción del menú o cuéntame:\n` +
                `• ¿Qué vehículo tienes?\n` +
                `• ¿Qué síntoma presenta?\n`
        },
        en: {
            assistantName: 'TECMAVE Assistant',
            online: 'Online • Ready to help',
            placeholder: 'Type your message here...',
            send: 'Send',
            welcome:
                `👋 **Hi! I'm your TECMAVE assistant.**\n\n` +
                `I can help with:\n` +
                `• 📞 Contact info\n` +
                `• 🕒 Hours\n` +
                `• 📍 Location\n` +
                `• 📅 Book an appointment\n` +
                `• 🔧 Services\n\n` +
                `What do you need?`,
            menuButtons: [
                { label: '📞 Contact info', intent: 'CONTACT' },
                { label: '🕒 Hours', intent: 'HOURS' },
                { label: '📍 Location', intent: 'LOCATION' },
                { label: '📅 Book appointment', intent: 'APPOINTMENT' },
                { label: '🔧 Services', intent: 'SERVICES' }
            ],
            backMenu: '⬅️ Back to menu',
            contactTitle: `📞 **CONTACT**`,
            hoursTitle: `🕒 **OPENING HOURS**`,
            locationTitle: `📍 **LOCATION**`,
            appointmentTitle: `📅 **APPOINTMENT**`,
            servicesTitle: `🔧 **TECMAVE SERVICES**`,
            fallback:
                `🤝 **Thanks for your message.**\n\n` +
                `To help you faster, choose an option from the menu or tell me:\n` +
                `• What vehicle do you have?\n` +
                `• What symptoms are you seeing?\n`
        }
    };

    const L = (lang === 'en') ? I18N.en : I18N.es;

    // ====== UI ======
    const chatIcon = document.createElement('div');
    chatIcon.innerHTML = `<i class="fas fa-comment-dots"></i>`;
    chatIcon.className = 'chat-icon';
    chatIcon.style.cssText = `
    position: fixed;
    bottom: 25px;
    right: 25px;
    width: 60px;
    height: 60px;
    background: linear-gradient(135deg, var(--primary), var(--primary-2));
    color: #ffffff;
    border-radius: 50%;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 1.8rem;
    cursor: pointer;
    z-index: 10000;
    box-shadow: 0 8px 25px rgba(220,38,38,0.4);
    transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
    border: 3px solid rgba(255,255,255,0.2);
  `;

    const chatWindow = document.createElement('div');
    chatWindow.className = 'chat-window';
    chatWindow.style.cssText = `
    position: fixed;
    bottom: 95px;
    right: 25px;
    width: 380px;
    height: 520px;
    background: white;
    border-radius: 20px;
    box-shadow: 0 25px 60px rgba(0,0,0,0.3);
    z-index: 10000;
    display: none;
    flex-direction: column;
    overflow: hidden;
    border: 1px solid rgba(220,38,38,0.1);
  `;

    chatWindow.innerHTML = `
    <div class="chat-header" style="
      padding: 18px 20px;
      background: linear-gradient(135deg, var(--primary), var(--primary-2));
      color: #fff;
      display: flex;
      justify-content: space-between;
      align-items: center;
    ">
      <div style="display:flex;align-items:center;gap:12px;">
        <div style="
          width: 40px;height: 40px;background: rgba(255,255,255,0.2);
          border-radius: 50%;display:flex;align-items:center;justify-content:center;
          font-size:1.2rem;
        ">🔧</div>
        <div>
          <h3 style="margin:0;font-size:1.2rem;font-weight:700;">${escapeHtml(L.assistantName)}</h3>
          <p style="margin:0;font-size:0.85rem;opacity:0.9;">${escapeHtml(L.online)}</p>
        </div>
      </div>
      <button class="close-chat" style="
        background:none;border:none;color:#fff;font-size:1.8rem;cursor:pointer;
        padding:5px;border-radius:8px;transition:all .3s ease;
        width:35px;height:35px;display:flex;align-items:center;justify-content:center;
        line-height:1;
      ">&times;</button>
    </div>

    <div class="chat-body" style="
      flex:1;padding:20px;overflow-y:auto;background: rgba(248,250,252,0.8);
      display:flex;flex-direction:column;
    ">
      <div class="chat-messages" style="display:flex;flex-direction:column;gap:15px;flex:1;"></div>
      <div class="quick-replies" style="margin-top:15px;display:flex;flex-wrap:wrap;gap:8px;"></div>
    </div>

    <div class="chat-footer" style="
      padding:18px 20px;display:flex;gap:12px;border-top:1px solid rgba(220,38,38,0.1);
      background:white;
    ">
      <input type="text" placeholder="${escapeHtml(L.placeholder)}" style="
        flex:1;padding:14px 18px;border:2px solid rgba(220,38,38,0.1);
        border-radius:12px;background: rgba(255,255,255,0.9);
        color: var(--text);font-size:0.95rem;transition: all .3s ease;outline:none;
      ">
      <button style="
        padding:14px 20px;background: linear-gradient(135deg, var(--primary), var(--primary-2));
        color:#fff;border:none;border-radius:12px;cursor:pointer;font-weight:600;
        transition: all .3s ease;box-shadow: 0 4px 15px rgba(220,38,38,0.3);
        font-size:0.95rem;min-width:80px;
      ">${escapeHtml(L.send)}</button>
    </div>
  `;

    document.body.appendChild(chatIcon);
    document.body.appendChild(chatWindow);

    const chatMessages = chatWindow.querySelector('.chat-messages');
    const quickReplies = chatWindow.querySelector('.quick-replies');
    const chatInput = chatWindow.querySelector('.chat-footer input');
    const sendButton = chatWindow.querySelector('.chat-footer button');
    const closeButton = chatWindow.querySelector('.close-chat');

    // ====== CSS ======
    const style = document.createElement('style');
    style.textContent = `
    .chat-icon:hover {
      transform: scale(1.1) rotate(5deg) !important;
      box-shadow: 0 15px 40px rgba(220,38,38,0.7) !important;
    }
    .chat-footer input:focus {
      border-color: var(--primary) !important;
      box-shadow: 0 0 0 4px rgba(220,38,38,0.15) !important;
      transform: scale(1.02) !important;
    }
    .chat-footer button:hover {
      transform: translateY(-2px) scale(1.05) !important;
      box-shadow: 0 8px 25px rgba(220,38,38,0.5) !important;
    }
    .close-chat:hover {
      background: rgba(255,255,255,0.2) !important;
      transform: scale(1.1) rotate(90deg) !important;
    }
    .chat-message {
      padding: 14px 18px;
      border-radius: 18px;
      max-width: 85%;
      word-wrap: break-word;
      animation: messageSlideIn 0.3s ease-out;
      line-height: 1.55;
      font-size: 0.92rem;
      margin: 5px 0;
    }
    .chat-message.user {
      background: linear-gradient(135deg, var(--primary), var(--primary-2));
      color: white;
      align-self: flex-end;
      box-shadow: 0 6px 20px rgba(220,38,38,0.3);
      border-bottom-right-radius: 8px;
    }
    .chat-message.bot {
      background: rgba(255,255,255,0.95);
      color: var(--text);
      align-self: flex-start;
      border: 1px solid rgba(220,38,38,0.1);
      box-shadow: 0 6px 20px rgba(0,0,0,0.08);
      border-bottom-left-radius: 8px;
    }
    .quick-reply-btn {
      padding: 10px 16px;
      background: rgba(220,38,38,0.08);
      border: 2px solid rgba(220,38,38,0.15);
      border-radius: 12px;
      color: var(--primary);
      font-size: 0.85rem;
      cursor: pointer;
      transition: all 0.3s ease;
      white-space: nowrap;
      font-weight: 600;
      margin: 2px;
    }
    .quick-reply-btn:hover {
      background: rgba(220,38,38,0.15);
      border-color: rgba(220,38,38,0.3);
      transform: translateY(-2px) scale(1.02);
      box-shadow: 0 4px 12px rgba(220,38,38,0.2);
    }
    @keyframes messageSlideIn { from { opacity:0; transform: translateY(10px);} to {opacity:1; transform: translateY(0);} }
    @keyframes chatSlideUp { from {opacity:0; transform: translateY(20px) scale(0.9);} to {opacity:1; transform: translateY(0) scale(1);} }
  `;
    document.head.appendChild(style);

    // ====== Engine (respuestas por INTENT) ======
    function getResponseByIntent(intent, freeText) {
        switch (intent) {
            case 'MENU':
                return {
                    response: L.welcome,
                    quickReplies: [...L.menuButtons]
                };

            case 'CONTACT':
                return {
                    response:
                        `${L.contactTitle}\n\n` +
                        (lang === 'en'
                            ? `You can contact us at:\n• Phone/WhatsApp: ${CONFIG.DISPLAY_PHONE}\n\nHow would you like to contact us?`
                            : `Puedes comunicarte con nosotros al:\n• Teléfono / WhatsApp: ${CONFIG.DISPLAY_PHONE}\n\n¿Cómo deseas contactarnos?`),
                    quickReplies: [
                        { label: (lang === 'en' ? '📞 Call now' : '📞 Llamar ahora'), action: { type: 'tel', value: CONFIG.PHONE } },
                        { label: '📲 WhatsApp', action: { type: 'wa', value: CONFIG.WHATSAPP } },
                        { label: L.backMenu, intent: 'MENU' }
                    ]
                };

            case 'HOURS':
                return {
                    response:
                        `${L.hoursTitle}\n\n` +
                        (lang === 'en'
                            ? `• Monday to Friday: ${CONFIG.HOURS.weekdays}\n• Saturday: ${CONFIG.HOURS.saturday}\n• Sunday: ${CONFIG.HOURS.sunday}`
                            : `• Lunes a viernes: ${CONFIG.HOURS.weekdays}\n• Sábado: ${CONFIG.HOURS.saturday}\n• Domingo: ${CONFIG.HOURS.sunday}`),
                    quickReplies: [
                        { label: (lang === 'en' ? '📅 Book appointment' : '📅 Agendar cita'), intent: 'APPOINTMENT' },
                        { label: L.backMenu, intent: 'MENU' }
                    ]
                };

            case 'LOCATION':
                return {
                    response:
                        `${L.locationTitle}\n\n` +
                        (lang === 'en'
                            ? `We are in **${CONFIG.LOCATION_TEXT}**.\n\nYou can open the location in Maps or contact us on WhatsApp.`
                            : `Estamos en **${CONFIG.LOCATION_TEXT}**.\n\nPuedes abrir la ubicación en Maps o escribirnos por WhatsApp.`),
                    quickReplies: [
                        { label: '🗺️ ' + (lang === 'en' ? 'Open in Maps' : 'Abrir Maps'), action: { type: 'url', value: CONFIG.MAPS_URL } },
                        { label: '📲 WhatsApp', action: { type: 'wa', value: CONFIG.WHATSAPP } },
                        { label: L.backMenu, intent: 'MENU' }
                    ]
                };

            case 'APPOINTMENT':
                return {
                    response:
                        `${L.appointmentTitle}\n\n` +
                        (lang === 'en'
                            ? `To schedule, send us:\n• Preferred day/time\n• Vehicle plate/model\n• Service needed\n\nIf you want, you can book via WhatsApp.`
                            : `Para agendar, envíanos:\n• Día y hora que te funciona\n• Placa / modelo del vehículo\n• Servicio que necesitas\n\nSi deseas, puedes agendar por WhatsApp.`),
                    quickReplies: [
                        ...(CONFIG.APPOINTMENT_URL
                            ? [{ label: (lang === 'en' ? '📅 Open booking' : '📅 Abrir agendamiento'), action: { type: 'url', value: CONFIG.APPOINTMENT_URL } }]
                            : []),
                        { label: '📲 WhatsApp', action: { type: 'wa', value: CONFIG.WHATSAPP } },
                        { label: L.backMenu, intent: 'MENU' }
                    ]
                };

            case 'SERVICES':
                return {
                    response:
                        `${L.servicesTitle}\n\n` +
                        (lang === 'en'
                            ? `We work on:\n• General mechanics\n• Electricity & electronics\n• Computerized diagnostic\n• Preventive maintenance\n\nTell me what you need and we’ll guide you.`
                            : `Trabajamos:\n• Mecánica general\n• Electricidad y electrónica\n• Diagnóstico computarizado\n• Mantenimiento preventivo\n\nCuéntame qué necesitas y te guiamos.`),
                    quickReplies: [
                        { label: (lang === 'en' ? '📅 Book appointment' : '📅 Agendar cita'), intent: 'APPOINTMENT' },
                        { label: (lang === 'en' ? '📞 Contact' : '📞 Contacto'), intent: 'CONTACT' },
                        { label: L.backMenu, intent: 'MENU' }
                    ]
                };

            default:
                // Si el usuario escribe libre, puedes “clasificar” a uno de los intents.
                // Mantengo algo simple para no romper: si detecta palabras, enruta; si no, fallback.
                const classified = classifyFreeTextToIntent(freeText || '');
                if (classified && classified !== 'FREE') return getResponseByIntent(classified, freeText);

                return {
                    response: L.fallback,
                    quickReplies: [...L.menuButtons, { label: L.backMenu, intent: 'MENU' }]
                };
        }
    }

    function classifyFreeTextToIntent(text) {
        const t = String(text || '').toLowerCase();

        if (!t.trim()) return 'MENU';

        if (t.includes('horario') || t.includes('hours') || t.includes('abren') || t.includes('cierran')) return 'HOURS';
        if (t.includes('ubic') || t.includes('address') || t.includes('location') || t.includes('donde')) return 'LOCATION';
        if (t.includes('cita') || t.includes('agendar') || t.includes('appointment') || t.includes('book')) return 'APPOINTMENT';
        if (t.includes('servicio') || t.includes('mantenimiento') || t.includes('service') || t.includes('repair')) return 'SERVICES';
        if (t.includes('tel') || t.includes('whatsapp') || t.includes('contact') || t.includes('llamar') || t.includes('call')) return 'CONTACT';

        return 'FREE';
    }

    // ====== Chat behavior ======
    chatIcon.addEventListener('click', () => {
        chatWindow.style.display = 'flex';
        chatWindow.style.animation = 'chatSlideUp 0.3s ease-out';
        chatIcon.style.display = 'none';
        chatInput.focus();
    });

    closeButton.addEventListener('click', () => {
        chatWindow.style.display = 'none';
        chatIcon.style.display = 'flex';
    });

    sendButton.addEventListener('click', () => sendTextMessage());
    chatInput.addEventListener('keypress', (e) => {
        if (e.key === 'Enter') sendTextMessage();
    });

    function sendTextMessage() {
        const message = chatInput.value.trim();
        if (!message) return;

        addMessage('user', message);

        setTimeout(() => {
            const res = getResponseByIntent('FREE', message);
            addMessage('bot', res.response);
            renderQuickReplies(res.quickReplies);
        }, 250);

        chatInput.value = '';
    }

    function sendIntent(intent, labelForUser) {
        addMessage('user', labelForUser || intent);

        setTimeout(() => {
            const res = getResponseByIntent(intent);
            addMessage('bot', res.response);
            renderQuickReplies(res.quickReplies);
        }, 200);
    }

    function addMessage(sender, message) {
        const el = document.createElement('div');
        el.className = `chat-message ${sender}`;

        // Render simple markdown seguro
        el.innerHTML = renderSafeMarkdown(message);

        chatMessages.appendChild(el);
        scrollToBottom();
    }

    function renderQuickReplies(replies) {
        quickReplies.innerHTML = '';
        if (!replies || !replies.length) return;

        replies.forEach((r) => {
            const btn = document.createElement('button');
            btn.className = 'quick-reply-btn';
            btn.textContent = r.label;

            btn.addEventListener('click', () => {
                // Mostrar lo que “elige” el usuario
                const label = r.label;

                // Acción (tel/wa/url) o intent
                if (r.action) {
                    addMessage('user', label);
                    handleAction(r.action);
                    // Luego de acción, mantenemos menú para no “quedar colgado”
                    const res = getResponseByIntent('MENU');
                    addMessage('bot', res.response);
                    renderQuickReplies(res.quickReplies);
                    return;
                }

                if (r.intent) {
                    sendIntent(r.intent, label);
                    return;
                }

                // fallback
                sendIntent('MENU', label);
            });

            quickReplies.appendChild(btn);
        });
    }

    function handleAction(action) {
        try {
            if (action.type === 'tel') {
                window.location.href = `tel:${action.value}`;
                return;
            }
            if (action.type === 'wa') {
                const msg = (lang === 'en')
                    ? 'Hello, I need assistance with my vehicle.'
                    : 'Hola, necesito ayuda con mi vehículo.';
                const url = `https://wa.me/${action.value}?text=${encodeURIComponent(msg)}`;
                window.open(url, '_blank', 'noopener');
                return;
            }
            if (action.type === 'url') {
                window.open(action.value, '_blank', 'noopener');
                return;
            }
        } catch (e) {
            // si algo falla, no rompemos el chat
            console.error('Chatbot action error:', e);
        }
    }

    function scrollToBottom() {
        chatMessages.scrollTop = chatMessages.scrollHeight;
    }

    // ====== Helpers: JSON, escape y markdown básico ======
    function safeJsonParse(s) {
        try { return JSON.parse(s); } catch { return null; }
    }

    function escapeHtml(str) {
        return String(str || '')
            .replaceAll('&', '&amp;')
            .replaceAll('<', '&lt;')
            .replaceAll('>', '&gt;')
            .replaceAll('"', '&quot;')
            .replaceAll("'", '&#039;');
    }

    function renderSafeMarkdown(text) {
        // 1) escapar HTML
        let s = escapeHtml(String(text || ''));

        // 2) **bold**
        s = s.replace(/\*\*(.+?)\*\*/g, '<strong>$1</strong>');

        // 3) saltos de línea
        s = s.replace(/\n/g, '<br>');

        return s;
    }

    // ====== Welcome ======
    setTimeout(() => {
        const res = getResponseByIntent('MENU');
        addMessage('bot', res.response);
        renderQuickReplies(res.quickReplies);
    }, 700);
})();
