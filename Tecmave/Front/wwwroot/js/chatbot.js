(function() {
    const body = document.body || document.getElementsByTagName('body')[0];

    const adminAttr = (body && body.getAttribute('data-is-admin')) || 'false';
    const clienteAttr = (body && body.getAttribute('data-is-cliente')) || 'false';

    const isAdmin = String(adminAttr).toLowerCase() === 'true';
    const isCliente = String(clienteAttr).toLowerCase() === 'true';

    const user = JSON.parse(localStorage.getItem('user') || '{}');

    if (!isCliente || isAdmin) {
        return;
    }



    const chatIcon = document.createElement('div');
    chatIcon.innerHTML = '<i class="fas fa-comment-dots"></i>';
    chatIcon.className = 'chat-icon';
    document.body.appendChild(chatIcon);

    const chatWindow = document.createElement('div');
    chatWindow.className = 'chat-window';
    chatWindow.innerHTML = `
        <div class="chat-header">
            <h3>Chat de Asistencia</h3>
            <button class="close-chat">&times;</button>
        </div>
        <div class="chat-body">
            <div class="chat-messages"></div>
        </div>
        <div class="chat-footer">
            <input type="text" placeholder="Escribe un mensaje...">
            <button>Enviar</button>
        </div>
    `;
    document.body.appendChild(chatWindow);

    const chatMessages = chatWindow.querySelector('.chat-messages');
    const chatInput = chatWindow.querySelector('.chat-footer input');
    const sendButton = chatWindow.querySelector('.chat-footer button');
    const closeButton = chatWindow.querySelector('.close-chat');

    const responses = {
        "horario": "Nuestro horario de atención es de Lunes a Sábado de 8:00 AM a 6:00 PM.",
        "ubicacion": "Estamos ubicados en Curridabat, 100 metros este de La Galera.",
        "gerente": "El número del gerente es +506 8888-8888.",
        "servicios": "Ofrecemos una amplia gama de servicios, incluyendo mantenimiento preventivo, reparaciones generales, y más. ¿En qué estás interesado?",
        "precios": "Los precios varían según el servicio. ¿Qué servicio te interesa cotizar?",
        "default": "No he entendido tu pregunta. Por favor, intenta de nuevo o contacta a nuestro equipo de soporte."
    };

    chatIcon.addEventListener('click', () => {
        chatWindow.style.display = 'flex';
        chatIcon.style.display = 'none';
    });

    closeButton.addEventListener('click', () => {
        chatWindow.style.display = 'none';
        chatIcon.style.display = 'block';
    });

    sendButton.addEventListener('click', () => {
        const message = chatInput.value.trim();
        if (message) {
            addMessage('user', message);
            handleResponse(message);
            chatInput.value = '';
        }
    });

    chatInput.addEventListener('keypress', (e) => {
        if (e.key === 'Enter') {
            sendButton.click();
        }
    });

    function addMessage(sender, message) {
        const messageElement = document.createElement('div');
        messageElement.className = `chat-message ${sender}`;
        messageElement.textContent = message;
        chatMessages.appendChild(messageElement);
        chatMessages.scrollTop = chatMessages.scrollHeight;
    }

    function handleResponse(message) {
        const lowerCaseMessage = message.toLowerCase();
        let response = responses.default;

        for (const key in responses) {
            if (lowerCaseMessage.includes(key)) {
                response = responses[key];
                break;
            }
        }
        setTimeout(() => addMessage('bot', response), 500);
    }
})();