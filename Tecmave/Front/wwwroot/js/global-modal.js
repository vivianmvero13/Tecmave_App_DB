(function () {
    function getElements() {
        const modal = document.getElementById("appFeedbackModal");
        if (!modal) return null;
        const dialog = modal.querySelector(".app-modal-dialog");
        const icon = modal.querySelector("#appModalIcon");
        const title = modal.querySelector("#appModalTitle");
        const message = modal.querySelector("#appModalMessage");
        const btn = modal.querySelector("#appModalClose");
        return { modal, dialog, icon, title, message, btn };
    }

    function normalizeType(msg, explicitType) {
        if (explicitType) return explicitType;
        if (!msg) return "info";
        if (/^\s*✔/.test(msg) || /exitosamente|correctamente|realizad[ao]/i.test(msg)) return "success";
        if (/^\s*❌/.test(msg) || /error|fall[óo]/i.test(msg)) return "error";
        return "info";
    }

    function cleanMessage(msg) {
        if (!msg) return "";
        return String(msg).replace(/^\s*[✔❌]\s*/u, "");
    }

    function showAppMessage(message, options) {
        const els = getElements();
        if (!els) {
            window._nativeAlert && window._nativeAlert(message);
            return;
        }
        const type = normalizeType(message, options && options.type);
        const { modal, dialog, icon, title, message: msgEl, btn } = els;

        dialog.classList.remove("success", "error", "info");
        dialog.classList.add(type);

        if (type === "success") {
            title.textContent = "Operación exitosa";
            icon.innerHTML = '<i class="fa-solid fa-check"></i>';
        } else if (type === "error") {
            title.textContent = "Algo salió mal";
            icon.innerHTML = '<i class="fa-solid fa-triangle-exclamation"></i>';
        } else {
            title.textContent = "Aviso";
            icon.innerHTML = '<i class="fa-solid fa-circle-info"></i>';
        }

        msgEl.textContent = cleanMessage(message);

        function close() {
            modal.classList.remove("show");
            document.removeEventListener("keydown", onKeyDown);
        }

        function onKeyDown(ev) {
            if (ev.key === "Escape") {
                close();
            }
        }

        modal.classList.add("show");
        if (btn) {
            btn.onclick = close;
            btn.focus();
        }

        modal.addEventListener("click", function (ev) {
            if (ev.target === modal) {
                close();
            }
        }, { once: true });

        document.addEventListener("keydown", onKeyDown);
    }

    if (!window._nativeAlert) {
        window._nativeAlert = window.alert;
        window.alert = function (msg) {
            showAppMessage(msg);
        };
    }

    window.showAppMessage = showAppMessage;
})();
