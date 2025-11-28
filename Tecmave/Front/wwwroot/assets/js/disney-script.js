// Inicializar AOS (Animate On Scroll)
document.addEventListener('DOMContentLoaded', function() {
    AOS.init({
        duration: 1000,
        once: true,
        offset: 100
    });

    // Scroll effect for header
    window.addEventListener('scroll', function() {
        const header = document.querySelector('.alpha-header');
        if (window.scrollY > 100) {
            header.classList.add('scrolled');
        } else {
            header.classList.remove('scrolled');
        }

        updateActiveNav();
    });

    // CORREGIDO: Smooth scrolling for navigation links
    document.querySelectorAll('.nav-link').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const targetId = this.getAttribute('href');

            // Solo procesar enlaces que sean anclas (#)
            if (targetId.startsWith('#')) {
                const target = document.querySelector(targetId);

                if (target) {
                    // Actualizar navegación activa
                    document.querySelectorAll('.nav-link').forEach(link => {
                        link.classList.remove('active');
                    });
                    this.classList.add('active');

                    // Scroll suave a la sección
                    const headerHeight = document.querySelector('.alpha-header').offsetHeight;
                    const targetPosition = target.offsetTop - headerHeight - 20;

                    window.scrollTo({
                        top: targetPosition,
                        behavior: 'smooth'
                    });
                }
            } else {
                // Para enlaces externos como login.html, permitir comportamiento normal
                window.location.href = targetId;
            }
        });
    });

    // Active navigation on scroll - MEJORADO
    function updateActiveNav() {
        const sections = document.querySelectorAll('section[id]');
        const navLinks = document.querySelectorAll('.nav-link[href^="#"]');
        const headerHeight = document.querySelector('.alpha-header').offsetHeight;

        let current = '';

        sections.forEach(section => {
            const sectionTop = section.offsetTop - headerHeight - 100;
            const sectionHeight = section.clientHeight;

            if (window.scrollY >= sectionTop && window.scrollY < sectionTop + sectionHeight) {
                current = section.getAttribute('id');
            }
        });

        navLinks.forEach(link => {
            link.classList.remove('active');
            const href = link.getAttribute('href');
            if (href === `#${current}`) {
                link.classList.add('active');
            }
        });

        // Si estamos en el top, marcar "Inicio" como activo
        if (window.scrollY < 100) {
            navLinks.forEach(link => {
                link.classList.remove('active');
            });
            document.querySelector('.nav-link[href="#inicio"]').classList.add('active');
        }
    }

    // Button click handlers - CORREGIDO
    document.querySelectorAll('.btn-hero-primary, .btn-cta-primary').forEach(btn => {
        btn.addEventListener('click', function() {
            // WhatsApp integration
            const phone = '50687059379';
            const message = 'Hola, me interesa agendar una cita en Tecmave';
            window.open(`https://wa.me/${phone}?text=${encodeURIComponent(message)}`, '_blank');

            // Animación de clic
            this.style.transform = 'scale(0.95)';
            setTimeout(() => {
                this.style.transform = '';
            }, 150);
        });
    });

    document.querySelectorAll('.btn-hero-secondary, .btn-cta-secondary').forEach(btn => {
        btn.addEventListener('click', function() {
            // Call phone number
            window.location.href = 'tel:+50622223333';

            // Animación de clic
            this.style.transform = 'scale(0.95)';
            setTimeout(() => {
                this.style.transform = '';
            }, 150);
        });
    });

    document.querySelectorAll('.btn-service-alpha').forEach(btn => {
        btn.addEventListener('click', function() {
            // Animación de clic
            this.style.transform = 'scale(0.95)';
            setTimeout(() => {
                this.style.transform = '';
            }, 150);

            // Mostrar modal de servicio
            setTimeout(() => {
                alert('Próximamente: Más información sobre este servicio');
            }, 200);
        });
    });

    // Efectos hover mejorados para todas las tarjetas
    document.querySelectorAll('.service-card-alpha, .feature-item-alpha, .testimonial-card').forEach(card => {
        card.addEventListener('mouseenter', function() {
            this.style.transform = 'translateY(-10px) scale(1.02)';
        });

        card.addEventListener('mouseleave', function() {
            this.style.transform = 'translateY(0) scale(1)';
        });
    });

    // Contador animado para stats
    function animateStats() {
        const stats = document.querySelectorAll('.stat-number');
        stats.forEach(stat => {
            const target = parseInt(stat.textContent);
            let current = 0;
            const increment = target / 50;
            const timer = setInterval(() => {
                current += increment;
                if (current >= target) {
                    stat.textContent = target + '+';
                    clearInterval(timer);
                } else {
                    stat.textContent = Math.floor(current) + '+';
                }
            }, 40);
        });
    }

    // Activar contadores cuando son visibles
    const statsObserver = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                animateStats();
                statsObserver.unobserve(entry.target);
            }
        });
    }, { threshold: 0.5 });

    const statsSection = document.querySelector('.hero-stats');
    if (statsSection) {
        statsObserver.observe(statsSection);
    }

    // Inicializar navegación activa
    updateActiveNav();

    console.log('Tecmave - Sitio cargado con todas las correcciones ✅');
});