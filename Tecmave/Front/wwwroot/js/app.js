(function() {
    /**
     * Initializes the carousel functionality.
     */
    function initCarousel() {
        const container = document.querySelector(".carousel-container");
        const track = document.querySelector(".carousel-track");
        const nextBtn = document.querySelector(".carousel-next");
        const prevBtn = document.querySelector(".carousel-prev");

        if (!container || !track) return;

        const slideWidth = container.offsetWidth;
        nextBtn?.addEventListener("click", () => track.scrollBy({ left: slideWidth, behavior: "smooth" }));
        prevBtn?.addEventListener("click", () => track.scrollBy({ left: -slideWidth, behavior: "smooth" }));
    }

    /**
     * Displays a toast message.
     * @param {string} message The message to display.
     * @param {number} duration The duration in milliseconds.
     */
    function toast(message, duration = 3000) {
        const toast = document.createElement("div");
        toast.className = "toast";
        toast.textContent = message;
        document.body.appendChild(toast);

        requestAnimationFrame(() => {
            toast.style.opacity = "1";
        });

        setTimeout(() => {
            toast.style.opacity = "0";
            setTimeout(() => document.body.removeChild(toast), 300);
        }, duration);
    }

    // Expose toast to the global window object
    window.toast = toast;

    /**
     * Initializes the modal functionality.
     */
    function initModals() {
        const modalButtons = document.querySelectorAll("[data-modal]");
        modalButtons.forEach(button => {
            button.addEventListener("click", () => {
                const modalId = button.getAttribute("data-modal");
                const modal = document.getElementById(modalId);
                if (modal) {
                    modal.style.display = "block";
                }
            });
        });

        const closeButtons = document.querySelectorAll(".close-button");
        closeButtons.forEach(button => {
            button.addEventListener("click", () => {
                const modal = button.closest(".modal");
                if (modal) {
                    modal.style.display = "none";
                }
            });
        });

        window.addEventListener("click", (event) => {
            if (event.target.classList.contains("modal")) {
                event.target.style.display = "none";
            }
        });
    }

    /**
     * Main initialization function on DOMContentLoaded.
     */
    document.addEventListener("DOMContentLoaded", () => {
        initCarousel();
        initModals();
    });

})();