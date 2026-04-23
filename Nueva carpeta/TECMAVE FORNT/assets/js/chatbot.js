// Chatbot corregido con OpenAI API
(function() {
    const user = JSON.parse(localStorage.getItem('user') || '{}');
    if (user.role !== 'cliente') {
        return;
    }

    // OpenAI API Configuration - ¡IMPORTANTE! CONFIGURA TU API KEY
    const CHATBOT_API = {
        endpoint: 'https://api.openai.com/v1/chat/completions',
        // ⚠️ ⚠️ ⚠️ REEMPLAZA ESTO CON TU API KEY REAL ⚠️ ⚠️ ⚠️
        apiKey: 'sk-tu-api-key-real-aqui', // 👈 ¡ESTO DEBE SER TU KEY VÁLIDA!
        
        async sendMessage(message, conversationHistory = []) {
            console.log("Enviando mensaje:", message);
            
            // Verificar si la API key es válida
            if (!this.apiKey || this.apiKey.includes('tu-api-key') || this.apiKey === 'tu_openai_key_aqui') {
                console.warn("⚠️ API KEY NO CONFIGURADA - Usando respuestas predefinidas");
                const response = this.getSmartFallback(message);
                conversationHistory.push(response);
                return response;
            }
            
            try {
                console.log("🔗 Conectando con OpenAI...");
                const response = await this.callOpenAIAPI(message, conversationHistory);
                conversationHistory.push(response);
                return response;
            } catch (error) {
                console.error('❌ Error OpenAI:', error);
                const fallback = this.getSmartFallback(message);
                conversationHistory.push(fallback);
                return fallback;
            }
        },

        async callOpenAIAPI(message, history) {
            const messages = [
                {
                    role: "system",
                    content: `Eres un asistente especializado de TECMAVE, taller mecánico en Costa Rica. 

INFORMACIÓN CRÍTICA DE TECMAVE:
• Taller: TECMAVE
• Teléfono: +506 8705 9379
• WhatsApp: +506 8705 9379  
• Horario: Lunes-Viernes 7AM-6PM, Sábados 8AM-4PM
• Ubicación: Alto de Guadalupe, San José
• Servicios: Mecánica general, frenos, suspensión, alineación, cambio de aceite, diagnóstico

RESPONDE SIEMPRE:
1. Como experto en mecánica automotriz
2. Menciona información de TECMAVE cuando sea relevante
3. Para emergencias, deriva al teléfono +506 8705 9379
4. Sé específico y técnico en tus respuestas
5. Si no sabes algo, recomienda contactar al taller

EMERGENCIAS COMUNES:
- Llanta reventada: Servicio de grúa disponible
- Frenos que fallan: Revisión inmediata
- Sobrecalentamiento: No conduzcas, llama al taller
- Batería muerta: Servicio a domicilio

¡Responde de manera ÚTIL y que SOLUCIONE el problema del usuario!`
                }
            ];

            // Agregar historial reciente
            const recentHistory = history.slice(-4);
            for (let i = 0; i < recentHistory.length; i++) {
                messages.push({
                    role: i % 2 === 0 ? "user" : "assistant",
                    content: recentHistory[i]
                });
            }

            // Mensaje actual
            messages.push({
                role: "user",
                content: message
            });

            const response = await fetch(this.endpoint, {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${this.apiKey}`,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    model: "gpt-3.5-turbo",
                    messages: messages,
                    max_tokens: 300,
                    temperature: 0.7
                })
            });

            if (!response.ok) {
                throw new Error(`HTTP ${response.status}`);
            }

            const data = await response.json();
            return data.choices[0].message.content.trim();
        },

        getSmartFallback(message) {
            const lowerMessage = message.toLowerCase();
            
            // Detectar emergencias y problemas específicos
            if (lowerMessage.includes('exploto') || lowerMessage.includes('revento') || lowerMessage.includes('llanta')) {
                return "🚨 **EMERGENCIA - LLANTA REVENTADA**\n\n¡Llámanos inmediatamente al +506 8705 9379!\n\n• Servicio de grúa disponible\n• Cambio de llanta de emergencia\n• Atención 24/7 para emergencias\n• Ubicación: Alto de Guadalupe, San José";
            }

            if (lowerMessage.includes('ruido') && lowerMessage.includes('fren')) {
                return "🔧 **PROBLEMA DE FRENOS**\n\nLos ruidos al frenar pueden ser:\n• Pastillas de freno desgastadas\n• Discos dañados\n• Objetos atascados\n\n**Te recomiendo:** Traer el vehículo para revisión INMEDIATA de frenos. La seguridad es lo primero.\n\n📞 Agenda cita: +506 8705 9379";
            }

            if (lowerMessage.includes('no frena') || lowerMessage.includes('frena mal')) {
                return "🚨 **EMERGENCIA DE FRENOS**\n\n¡NO CONDUZCAS! Llama al +506 8705 9379 para:\n• Grúa de emergencia\n• Revisión inmediata\n• Reparación urgente de frenos";
            }

            if (lowerMessage.includes('hola') || lowerMessage.includes('buenas')) {
                return "¡Hola! 👋 Soy tu asistente de TECMAVE. Veo que tienes una emergencia. ¿En qué puedo ayudarte específicamente?\n\n• 🚨 **Emergencias**: +506 8705 9379\n• 🔧 **Problemas mecánicos**\n• 📍 **Ubicación y horarios**\n• 💰 **Cotizaciones**";
            }

            // Respuesta para otras preguntas
            return `🔧 **TECMAVE - Asistente Mecánico**\n\nPara darte una respuesta precisa sobre "${message}", te recomiendo:\n\n📞 **Contactarnos directamente:** +506 8705 9379\n\n📍 **Visítanos:** Alto de Guadalupe, San José\n🕐 **Horario:** L-V 7AM-6PM, S 8AM-4PM\n\n¿Es una emergencia? Llámanos ahora mismo.`;
        }
    };

    // [EL RESTO DEL CÓDIGO DE LA INTERFAZ SE MANTIENE IGUAL]
    // Crear elementos del chatbot
    const chatIcon = document.createElement('div');
    chatIcon.innerHTML = `
        <i class="fas fa-comment-dots"></i>
        <div class="chat-notification"></div>
    `;
    chatIcon.className = 'chat-icon';
    chatIcon.style.cssText = `
        position: fixed;
        bottom: 20px;
        right: 20px;
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
        animation: chatPulse 2s infinite;
    `;

    const chatNotification = chatIcon.querySelector('.chat-notification');
    chatNotification.style.cssText = `
        position: absolute;
        top: -3px;
        right: -3px;
        width: 16px;
        height: 16px;
        background: #22c55e;
        border-radius: 50%;
        border: 2px solid white;
        animation: notificationPulse 1.5s infinite;
        display: none;
    `;

    const chatWindow = document.createElement('div');
    chatWindow.className = 'chat-window';
    chatWindow.style.cssText = `
        position: fixed;
        bottom: 90px;
        right: 20px;
        width: 350px;
        height: 500px;
        background: white;
        border-radius: 16px;
        box-shadow: 0 20px 60px rgba(0,0,0,0.3);
        z-index: 10000;
        display: none;
        flex-direction: column;
        overflow: hidden;
        border: 1px solid rgba(220,38,38,0.1);
        animation: chatSlideUp 0.3s ease-out;
    `;

    chatWindow.innerHTML = `
        <div class="chat-header" style="
            padding: 15px;
            background: linear-gradient(135deg, var(--primary), var(--primary-2));
            color: #fff;
            display: flex;
            justify-content: space-between;
            align-items: center;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        ">
            <div style="display: flex; align-items: center; gap: 10px;">
                <div style="
                    width: 35px;
                    height: 35px;
                    background: rgba(255,255,255,0.2);
                    border-radius: 50%;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    font-size: 1rem;
                ">🤖</div>
                <div>
                    <h3 style="margin: 0; font-size: 1.1rem; font-weight: 700;">Asistente TECMAVE</h3>
                    <p style="margin: 0; font-size: 0.8rem; opacity: 0.9;">En línea</p>
                </div>
            </div>
            <button class="close-chat" style="
                background: none;
                border: none;
                color: #fff;
                font-size: 1.5rem;
                cursor: pointer;
                padding: 4px;
                border-radius: 6px;
                transition: all 0.3s ease;
                width: 30px;
                height: 30px;
                display: flex;
                align-items: center;
                justify-content: center;
            ">&times;</button>
        </div>
        <div class="chat-body" style="
            flex: 1;
            padding: 15px;
            overflow-y: auto;
            background: rgba(248,250,252,0.8);
        ">
            <div class="chat-messages" style="
                display: flex;
                flex-direction: column;
                gap: 12px;
            "></div>
        </div>
        <div class="chat-footer" style="
            padding: 15px;
            display: flex;
            gap: 10px;
            border-top: 1px solid rgba(220,38,38,0.1);
            background: white;
        ">
            <input type="text" placeholder="Escribe tu mensaje..." style="
                flex: 1;
                padding: 12px 15px;
                border: 2px solid rgba(220,38,38,0.1);
                border-radius: 10px;
                background: rgba(255,255,255,0.9);
                color: var(--text);
                font-size: 0.9rem;
                transition: all 0.3s ease;
            ">
            <button style="
                padding: 12px 16px;
                background: linear-gradient(135deg, var(--primary), var(--primary-2));
                color: #fff;
                border: none;
                border-radius: 10px;
                cursor: pointer;
                font-weight: 600;
                transition: all 0.3s ease;
                box-shadow: 0 4px 12px rgba(220,38,38,0.3);
                font-size: 0.9rem;
            ">Enviar</button>
        </div>
    `;

    document.body.appendChild(chatIcon);
    document.body.appendChild(chatWindow);

    const chatMessages = chatWindow.querySelector('.chat-messages');
    const chatInput = chatWindow.querySelector('.chat-footer input');
    const sendButton = chatWindow.querySelector('.chat-footer button');
    const closeButton = chatWindow.querySelector('.close-chat');

    let conversationHistory = [];

    // Animaciones CSS
    const style = document.createElement('style');
    style.textContent = `
        @keyframes chatPulse {
            0% { transform: scale(1); box-shadow: 0 8px 25px rgba(220,38,38,0.4); }
            50% { transform: scale(1.05); box-shadow: 0 12px 35px rgba(220,38,38,0.6); }
            100% { transform: scale(1); box-shadow: 0 8px 25px rgba(220,38,38,0.4); }
        }

        @keyframes notificationPulse {
            0% { transform: scale(1); opacity: 1; }
            50% { transform: scale(1.2); opacity: 0.7; }
            100% { transform: scale(1); opacity: 1; }
        }

        @keyframes chatSlideUp {
            from { opacity: 0; transform: translateY(20px) scale(0.9); }
            to { opacity: 1; transform: translateY(0) scale(1); }
        }

        @keyframes messageSlideIn {
            from { opacity: 0; transform: translateY(10px); }
            to { opacity: 1; transform: translateY(0); }
        }

        .chat-icon:hover {
            transform: scale(1.1) rotate(5deg);
            box-shadow: 0 12px 35px rgba(220,38,38,0.6);
        }

        .chat-footer input:focus {
            border-color: var(--primary) !important;
            box-shadow: 0 0 0 3px rgba(220,38,38,0.1) !important;
            transform: scale(1.02);
        }

        .chat-footer button:hover {
            transform: translateY(-2px);
            box-shadow: 0 6px 20px rgba(220,38,38,0.4);
        }

        .close-chat:hover {
            background: rgba(255,255,255,0.2) !important;
            transform: scale(1.1);
        }

        .typing-indicator {
            display: flex;
            gap: 4px;
            padding: 12px 16px;
            background: rgba(255,255,255,0.8);
            border-radius: 15px;
            align-self: flex-start;
            max-width: 70px;
            margin-bottom: 5px;
        }

        .typing-indicator span {
            width: 6px;
            height: 6px;
            background: var(--primary);
            border-radius: 50%;
            animation: typingBounce 1.4s infinite ease-in-out;
        }

        .typing-indicator span:nth-child(1) { animation-delay: -0.32s; }
        .typing-indicator span:nth-child(2) { animation-delay: -0.16s; }

        @keyframes typingBounce {
            0%, 80%, 100% { transform: scale(0); }
            40% { transform: scale(1); }
        }

        .chat-message {
            padding: 12px 16px;
            border-radius: 15px;
            max-width: 80%;
            word-wrap: break-word;
            animation: messageSlideIn 0.3s ease-out;
            line-height: 1.4;
            font-size: 0.9rem;
        }

        .chat-message.user {
            background: linear-gradient(135deg, var(--primary), var(--primary-2));
            color: white;
            align-self: flex-end;
            box-shadow: 0 4px 12px rgba(220,38,38,0.3);
        }

        .chat-message.bot {
            background: rgba(255,255,255,0.9);
            color: var(--text);
            align-self: flex-start;
            border: 1px solid rgba(220,38,38,0.1);
            box-shadow: 0 4px 12px rgba(0,0,0,0.05);
        }
    `;
    document.head.appendChild(style);

    // Mensaje de bienvenida inicial
    setTimeout(() => {
        addMessage('bot', '¡Hola! 👋 Soy tu asistente especializado de TECMAVE. Puedo ayudarte con:\n\n🚨 **Emergencias** (llantas, frenos, grúa)\n🔧 **Problemas mecánicos**\n📅 **Citas y horarios**\n💰 **Cotizaciones**\n\n¿En qué puedo asistirte?', true);
        chatNotification.style.display = 'block';
    }, 1500);

    // Event Listeners
    chatIcon.addEventListener('click', () => {
        chatWindow.style.display = 'flex';
        chatIcon.style.display = 'none';
        chatNotification.style.display = 'none';
        chatInput.focus();
    });

    closeButton.addEventListener('click', () => {
        chatWindow.style.display = 'none';
        chatIcon.style.display = 'flex';
    });

    sendButton.addEventListener('click', sendMessage);
    chatInput.addEventListener('keypress', (e) => {
        if (e.key === 'Enter') {
            sendMessage();
        }
    });

    // Funciones del chatbot
    function sendMessage() {
        const message = chatInput.value.trim();
        if (message) {
            addMessage('user', message);
            conversationHistory.push(message);
            handleAIResponse(message);
            chatInput.value = '';
        }
    }

    async function handleAIResponse(message) {
        showTypingIndicator();

        try {
            const response = await CHATBOT_API.sendMessage(message, conversationHistory);
            setTimeout(() => {
                hideTypingIndicator();
                addMessage('bot', response);
            }, 1000 + Math.random() * 500);
        } catch (error) {
            hideTypingIndicator();
            const fallbackResponse = CHATBOT_API.getSmartFallback(message);
            addMessage('bot', fallbackResponse);
        }
    }

    function addMessage(sender, message, isWelcome = false) {
        const messageElement = document.createElement('div');
        messageElement.className = `chat-message ${sender}`;
        
        const formattedMessage = message.replace(/\n/g, '<br>');
        messageElement.innerHTML = formattedMessage;

        chatMessages.appendChild(messageElement);
        scrollToBottom();

        if (!isWelcome && sender === 'bot' && chatWindow.style.display === 'none') {
            chatNotification.style.display = 'block';
            chatIcon.style.animation = 'chatPulse 0.5s 3';
        }
    }

    function showTypingIndicator() {
        const typingElement = document.createElement('div');
        typingElement.className = 'typing-indicator';
        typingElement.innerHTML = `
            <span></span>
            <span></span>
            <span></span>
        `;
        chatMessages.appendChild(typingElement);
        scrollToBottom();
    }

    function hideTypingIndicator() {
        const typingIndicator = document.querySelector('.typing-indicator');
        if (typingIndicator) {
            typingIndicator.remove();
        }
    }

    function scrollToBottom() {
        const chatBody = chatWindow.querySelector('.chat-body');
        chatBody.scrollTop = chatBody.scrollHeight;
    }

    // Quick replies mejoradas
    const quickReplies = [
        '🚨 Llanta reventada',
        '🔧 Ruido en frenos', 
        '📞 Contacto emergencia',
        '📍 Ubicación',
        '💰 Cotización'
    ];

    setTimeout(() => {
        if (conversationHistory.length === 1) {
            const quickRepliesContainer = document.createElement('div');
            quickRepliesContainer.style.cssText = `
                display: flex;
                flex-wrap: wrap;
                gap: 6px;
                margin-top: 12px;
                justify-content: center;
            `;

            quickReplies.forEach(reply => {
                const button = document.createElement('button');
                button.textContent = reply;
                button.style.cssText = `
                    padding: 6px 12px;
                    background: rgba(220,38,38,0.1);
                    border: 1px solid rgba(220,38,38,0.2);
                    border-radius: 15px;
                    color: var(--primary);
                    font-size: 0.8rem;
                    cursor: pointer;
                    transition: all 0.3s ease;
                    white-space: nowrap;
                `;
                button.addEventListener('click', () => {
                    chatInput.value = reply.replace(/[🚨🔧📞📍💰]/g, '').trim();
                    sendMessage();
                });

                quickRepliesContainer.appendChild(button);
            });

            chatMessages.appendChild(quickRepliesContainer);
            scrollToBottom();
        }
    }, 3000);
})();