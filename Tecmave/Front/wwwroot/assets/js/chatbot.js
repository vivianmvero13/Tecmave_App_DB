// Chatbot mejorado y unificado
(function() {
    const body = document.body || document.getElementsByTagName('body')[0];

    const adminAttr = (body && body.getAttribute('data-is-admin')) || 'false';
    const clienteAttr = (body && body.getAttribute('data-is-cliente')) || 'false';

    const isAdmin = String(adminAttr).toLowerCase() === 'true';
    const isCliente = String(clienteAttr).toLowerCase() === 'true';

    const user = JSON.parse(localStorage.getItem('user') || '{}');
    const lang = localStorage.getItem('language') || 'es';
    if (user.role !== 'cliente') {
        return;
    }



    
    const CHATBOT_API = {
        getSmartResponse(message) {
            const lowerMessage = message.toLowerCase();
            const langCode = (lang === 'en') ? 'en' : 'es';

            // Emergency assistance
            if (
                lowerMessage.includes('emergencia') ||
                lowerMessage.includes('llanta') ||
                lowerMessage.includes('reventó') ||
                lowerMessage.includes('revento') ||
                lowerMessage.includes('choque') ||
                lowerMessage.includes('accidente') ||
                lowerMessage.includes('grúa') ||
                lowerMessage.includes('grua') ||
                lowerMessage.includes('emergency') ||
                lowerMessage.includes('tire') ||
                lowerMessage.includes('flat') ||
                lowerMessage.includes('tow')
            ) {
                if (langCode === 'en') {
                    return {
                        response:
`🚨 **EMERGENCY ROAD ASSISTANCE**

We are here to help you immediately.

• We can coordinate a tow truck
• We can give you quick mechanical support
• 📍 Our workshop is in Alto de Guadalupe, San José

Would you like us to contact you?`,
                        quickReplies: ['📞 Call now', '🚗 Tow truck service', '📍 Send location']
                    };
                }
                return {
                    response:
`🚨 **EMERGENCIA - ASISTENCIA INMEDIATA**

Estamos aquí para ayudarte de inmediato.

• Coordinamos grúa
• Te damos soporte mecánico rápido
• 📍 Estamos en Alto de Guadalupe, San José

¿Quieres que te contactemos?`,
                    quickReplies: ['📞 Llamar ahora', '🚗 Servicio grúa', '📍 Enviar ubicación']
                };
            }

            // Brakes / noises
            if (
                (lowerMessage.includes('ruido') && lowerMessage.includes('fren')) ||
                lowerMessage.includes('freno') ||
                lowerMessage.includes('frenos') ||
                lowerMessage.includes('chilla') ||
                lowerMessage.includes('chirria') ||
                lowerMessage.includes('vibra') ||
                (lowerMessage.includes('brake') && lowerMessage.includes('noise')) ||
                lowerMessage.includes('squeak')
            ) {
                if (langCode === 'en') {
                    return {
                        response:
`🔧 **BRAKE DIAGNOSTIC**

We recommend you do **not** postpone a brake inspection.

Typical causes:
• Worn pads or discs
• Warped rotors
• Lack of maintenance
• Loose or contaminated parts

📅 **Recommendation:** Schedule an inspection as soon as possible.`,
                        quickReplies: ['📅 Schedule appointment', '💰 Brake quote', '📞 Talk to a technician']
                    };
                }
                return {
                    response:
`🔧 **DIAGNÓSTICO DE FRENOS**

Te recomendamos **no** posponer la revisión de frenos.

Posibles causas:
• Pastillas o discos desgastados
• Discos alabeados
• Falta de mantenimiento
• Piezas sueltas o contaminadas

📅 **Recomendación:** Agenda una revisión lo antes posible.`,
                    quickReplies: ['📅 Agendar cita', '💰 Cotizar frenos', '📞 Consultar técnico']
                };
            }

            // Prices / quotes
            if (
                lowerMessage.includes('precio') ||
                lowerMessage.includes('costo') ||
                lowerMessage.includes('cotizacion') ||
                lowerMessage.includes('cotización') ||
                lowerMessage.includes('cuanto') ||
                lowerMessage.includes('cuánto') ||
                lowerMessage.includes('price') ||
                lowerMessage.includes('how much') ||
                lowerMessage.includes('quote') ||
                lowerMessage.includes('estimate')
            ) {
                if (langCode === 'en') {
                    return {
                        response:
`💰 **PERSONALIZED QUOTES**

Prices depend on:
• Vehicle model and year
• Type of service
• Parts that must be replaced

📲 Send us:
• Plate or model
• Type of problem or service you need

📞 **Contact us and we'll give you an accurate quote.**`,
                        quickReplies: ['📞 Request quote', '📅 Schedule diagnostic', '🔧 See services']
                    };
                }
                return {
                    response:
`💰 **COTIZACIONES PERSONALIZADAS**

El precio depende de:
• Modelo y año del vehículo
• Tipo de servicio
• Piezas que se deban reemplazar

📲 Envíanos:
• Placa o modelo
• Tipo de problema o servicio que necesitas

📞 **Contáctanos y te damos una cotización precisa.**`,
                    quickReplies: ['📞 Solicitar cotización', '📅 Agendar diagnóstico', '🔧 Ver servicios']
                };
            }

            // General services
            if (
                lowerMessage.includes('servicio') ||
                lowerMessage.includes('servicios') ||
                lowerMessage.includes('reparación') ||
                lowerMessage.includes('reparacion') ||
                lowerMessage.includes('mantenimiento') ||
                lowerMessage.includes('alineación') ||
                lowerMessage.includes('alineacion') ||
                lowerMessage.includes('diagnóstico') ||
                lowerMessage.includes('diagnostico') ||
                lowerMessage.includes('service') ||
                lowerMessage.includes('maintenance') ||
                lowerMessage.includes('repair')
            ) {
                if (langCode === 'en') {
                    return {
                        response:
`🔧 **TECMAVE SERVICES**

We work on:
• General mechanics
• Electricity and electronics
• Computerized diagnostic
• Preventive maintenance
• Emergency service 24/7

Tell me what type of service you need and I’ll guide you.`,
                        quickReplies: ['🔧 Diagnostic', '📅 Schedule service', '💰 Maintenance quote']
                    };
                }
                return {
                    response:
`🔧 **SERVICIOS TECMAVE**

Trabajamos:
• Mecánica general
• Electricidad y electrónica
• Diagnóstico computarizado
• Mantenimiento preventivo
• Servicio de emergencia 24/7

Cuéntame qué tipo de servicio necesitas y te guío.`,
                    quickReplies: ['🔧 Diagnóstico', '📅 Agendar servicio', '💰 Cotizar mantenimiento']
                };
            }

            // Appointments
            if (
                lowerMessage.includes('cita') ||
                lowerMessage.includes('agendar') ||
                lowerMessage.includes('agenda') ||
                lowerMessage.includes('reservar') ||
                lowerMessage.includes('appointment') ||
                lowerMessage.includes('schedule') ||
                lowerMessage.includes('book')
            ) {
                if (langCode === 'en') {
                    return {
                        response:
`📅 **APPOINTMENT SCHEDULING**

We can help you schedule a visit to our workshop.

Send us:
• Day and time that works for you
• Type of service
• Vehicle model / plate

We'll confirm availability and the best time slot.`,
                        quickReplies: ['📅 Schedule for today', '📆 Schedule this week', '📞 Talk to an advisor']
                    };
                }
                return {
                    response:
`📅 **AGENDAMIENTO DE CITA**

Podemos ayudarte a reservar una visita al taller.

Envíanos:
• Día y hora que te funciona
• Tipo de servicio
• Modelo / placa del vehículo

Te confirmamos la disponibilidad y el mejor horario.`,
                    quickReplies: ['📅 Agenda para hoy', '📆 Agenda esta semana', '📞 Hablar con un asesor']
                };
            }

            // Schedule / opening hours
            if (
                lowerMessage.includes('horario') ||
                lowerMessage.includes('hora') ||
                lowerMessage.includes('abren') ||
                lowerMessage.includes('cierran') ||
                lowerMessage.includes('open') ||
                lowerMessage.includes('close') ||
                lowerMessage.includes('hours')
            ) {
                if (langCode === 'en') {
                    return {
                        response:
`🕒 **OPENING HOURS**

• Monday to Friday: 8:00 a.m. – 6:00 p.m.
• Saturday: 8:00 a.m. – 3:00 p.m.
• Sunday: Emergency service by prior coordination.`,
                        quickReplies: ['📅 Schedule appointment', '📍 See location', '📞 Call']
                    };
                }
                return {
                    response:
`🕒 **HORARIO DE ATENCIÓN**

• Lunes a viernes: 8:00 a.m. – 6:00 p.m.
• Sábado: 8:00 a.m. – 3:00 p.m.
• Domingo: Servicio de emergencia con coordinación previa.`,
                    quickReplies: ['📅 Agendar cita', '📍 Ver ubicación', '📞 Llamar']
                };
            }

            // Location
            if (
                lowerMessage.includes('ubicación') ||
                lowerMessage.includes('ubicacion') ||
                lowerMessage.includes('dirección') ||
                lowerMessage.includes('direccion') ||
                lowerMessage.includes('dónde están') ||
                lowerMessage.includes('donde estan') ||
                lowerMessage.includes('where are you') ||
                lowerMessage.includes('location') ||
                lowerMessage.includes('address')
            ) {
                if (langCode === 'en') {
                    return {
                        response:
`📍 **LOCATION**

We are in **Alto de Guadalupe, San José, Costa Rica.**

If you wish, you can share your location and we’ll guide you or coordinate a tow truck.`,
                        quickReplies: ['📍 Send my location', '🗺️ Open in maps', '📞 Call the workshop']
                    };
                }
                return {
                    response:
`📍 **UBICACIÓN**

Estamos en **Alto de Guadalupe, San José, Costa Rica.**

Si deseas, puedes enviarnos tu ubicación y te guiamos o coordinamos una grúa.`,
                    quickReplies: ['📍 Enviar mi ubicación', '🗺️ Abrir en mapas', '📞 Llamar al taller']
                };
            }

            // Contact
            if (
                lowerMessage.includes('teléfono') ||
                lowerMessage.includes('telefono') ||
                lowerMessage.includes('whatsapp') ||
                lowerMessage.includes('contacto') ||
                lowerMessage.includes('llamar') ||
                lowerMessage.includes('call') ||
                lowerMessage.includes('phone')
            ) {
                if (langCode === 'en') {
                    return {
                        response:
`📞 **CONTACT**

You can contact us at:
• Phone / WhatsApp: +506 2285-9379

We’ll be happy to help you with your vehicle.`,
                        quickReplies: ['📞 Call now', '📲 Write on WhatsApp', '📅 Schedule appointment']
                    };
                }
                return {
                    response:
`📞 **CONTACTO**

Puedes comunicarte con nosotros al:
• Teléfono / WhatsApp: +506 2285-9379

Con gusto te ayudamos con tu vehículo.`,
                    quickReplies: ['📞 Llamar ahora', '📲 Escribir por WhatsApp', '📅 Agendar cita']
                };
            }

            // Greetings / default
            if (
                lowerMessage.includes('hola') ||
                lowerMessage.includes('buenas') ||
                lowerMessage.includes('buenos días') ||
                lowerMessage.includes('buenos dias') ||
                lowerMessage.includes('buenas tardes') ||
                lowerMessage.includes('buenas noches') ||
                lowerMessage.includes('hello') ||
                lowerMessage.includes('hi') ||
                lowerMessage.trim() === ''
            ) {
                if (langCode === 'en') {
                    return {
                        response:
`👋 **Hi! I'm your TECMAVE assistant.**

I can help you with:
• 🚗 Mechanical or electrical problems
• 📅 Appointments and scheduling
• 💰 Quotes
• 📍 Location and contact info

What do you need help with today?`,
                        quickReplies: ['🚨 Emergency', '🔧 Mechanical problem', '📅 Schedule appointment', '💰 Quote', '📍 Location']
                    };
                }
                return {
                    response:
`👋 **¡Hola! Soy tu asistente de TECMAVE.**

Puedo ayudarte con:
• 🚗 Problemas mecánicos o eléctricos
• 📅 Citas y agendamiento
• 💰 Cotizaciones
• 📍 Ubicación y datos de contacto

¿En qué puedo asistirte hoy?`,
                    quickReplies: ['🚨 Emergencia', '🔧 Problema mecánico', '📅 Agendar cita', '💰 Cotización', '📍 Ubicación']
                };
            }

            // Fallback
            if (langCode === 'en') {
                return {
                    response:
`🤝 **Thanks for your message.**

I couldn't identify a specific category, but I can help you with:
• Vehicle problems
• Appointments
• Quotes
• Location and contact

Could you tell me a bit more about your situation?`,
                    quickReplies: ['🔧 It’s a mechanical problem', '⚡ It’s electrical/electronic', '📅 I want an appointment']
                };
            }

            return {
                response:
`🤝 **Gracias por tu mensaje.**

No pude identificar una categoría específica, pero puedo ayudarte con:
• Problemas con tu vehículo
• Citas
• Cotizaciones
• Ubicación y contacto

¿Podrías contarme un poco más de tu situación?`,
                quickReplies: ['🔧 Es un problema mecánico', '⚡ Es eléctrico / electrónico', '📅 Quiero una cita']
            };
        }
    };


    // Crear elementos del chatbot
    const chatIcon = document.createElement('div');
    chatIcon.innerHTML = `
        <i class="fas fa-comment-dots"></i>
    `;
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
        height: 500px;
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
            <div style="display: flex; align-items: center; gap: 12px;">
                <div style="
                    width: 40px;
                    height: 40px;
                    background: rgba(255,255,255,0.2);
                    border-radius: 50%;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    font-size: 1.2rem;
                ">🔧</div>
                <div>
                    <h3 style="margin: 0; font-size: 1.2rem; font-weight: 700;">${lang === 'en' ? 'TECMAVE Assistant' : 'Asistente TECMAVE'}</h3>
                    <p style="margin: 0; font-size: 0.85rem; opacity: 0.9;">${lang === 'en' ? 'Online • Ready to help' : 'En línea • Listo para ayudar'}</p>
                </div>
            </div>
            <button class="close-chat" style="
                background: none;
                border: none;
                color: #fff;
                font-size: 1.8rem;
                cursor: pointer;
                padding: 5px;
                border-radius: 8px;
                transition: all 0.3s ease;
                width: 35px;
                height: 35px;
                display: flex;
                align-items: center;
                justify-content: center;
                line-height: 1;
            ">&times;</button>
        </div>
        <div class="chat-body" style="
            flex: 1;
            padding: 20px;
            overflow-y: auto;
            background: rgba(248,250,252,0.8);
            display: flex;
            flex-direction: column;
        ">
            <div class="chat-messages" style="
                display: flex;
                flex-direction: column;
                gap: 15px;
                flex: 1;
            "></div>
            <div class="quick-replies" style="
                margin-top: 15px;
                display: flex;
                flex-wrap: wrap;
                gap: 8px;
            "></div>
        </div>
        <div class="chat-footer" style="
            padding: 18px 20px;
            display: flex;
            gap: 12px;
            border-top: 1px solid rgba(220,38,38,0.1);
            background: white;
        ">
            <input type="text" placeholder="${lang === 'en' ? 'Type your message here...' : 'Escribe tu mensaje aquí...'}" style="
                flex: 1;
                padding: 14px 18px;
                border: 2px solid rgba(220,38,38,0.1);
                border-radius: 12px;
                background: rgba(255,255,255,0.9);
                color: var(--text);
                font-size: 0.95rem;
                transition: all 0.3s ease;
                outline: none;
            ">
            <button style="
                padding: 14px 20px;
                background: linear-gradient(135deg, var(--primary), var(--primary-2));
                color: #fff;
                border: none;
                border-radius: 12px;
                cursor: pointer;
                font-weight: 600;
                transition: all 0.3s ease;
                box-shadow: 0 4px 15px rgba(220,38,38,0.3);
                font-size: 0.95rem;
                min-width: 80px;
            ">${lang === 'en' ? 'Send' : 'Enviar'}</button>
        </div>
    `;

    document.body.appendChild(chatIcon);
    document.body.appendChild(chatWindow);

    const chatMessages = chatWindow.querySelector('.chat-messages');
    const quickReplies = chatWindow.querySelector('.quick-replies');
    const chatInput = chatWindow.querySelector('.chat-footer input');
    const sendButton = chatWindow.querySelector('.chat-footer button');
    const closeButton = chatWindow.querySelector('.close-chat');

    let conversationHistory = [];

    // Estilos CSS
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
            line-height: 1.5;
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

        @keyframes messageSlideIn {
            from { opacity: 0; transform: translateY(10px); }
            to { opacity: 1; transform: translateY(0); }
        }

        @keyframes chatSlideUp {
            from { opacity: 0; transform: translateY(20px) scale(0.9); }
            to { opacity: 1; transform: translateY(0) scale(1); }
        }
    `;
    document.head.appendChild(style);

    // Mensaje de bienvenida
    setTimeout(() => {
        const welcomeResponse = CHATBOT_API.getSmartResponse(lang === 'en' ? 'hello' : 'hola');
        addMessage('bot', welcomeResponse.response);
        createQuickReplies(welcomeResponse.quickReplies);
    }, 1000);

    // Event Listeners
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

    sendButton.addEventListener('click', sendMessage);
    chatInput.addEventListener('keypress', (e) => {
        if (e.key === 'Enter') sendMessage();
    });

    function sendMessage() {
        const message = chatInput.value.trim();
        if (message) {
            addMessage('user', message);
            conversationHistory.push(message);

            setTimeout(() => {
                const response = CHATBOT_API.getSmartResponse(message);
                addMessage('bot', response.response);
                createQuickReplies(response.quickReplies);
            }, 500);

            chatInput.value = '';
        }
    }

    function addMessage(sender, message) {
        const messageElement = document.createElement('div');
        messageElement.className = `chat-message ${sender}`;
        messageElement.innerHTML = message.replace(/\n/g, '<br>');
        chatMessages.appendChild(messageElement);
        scrollToBottom();
    }

    function createQuickReplies(replies) {
        quickReplies.innerHTML = '';
        if (replies && replies.length > 0) {
            replies.forEach(reply => {
                const button = document.createElement('button');
                button.className = 'quick-reply-btn';
                button.textContent = reply;
                button.addEventListener('click', () => {
                    chatInput.value = reply;
                    sendMessage();
                });
                quickReplies.appendChild(button);
            });
        }
    }

    function scrollToBottom() {
        chatMessages.scrollTop = chatMessages.scrollHeight;
    }
})();