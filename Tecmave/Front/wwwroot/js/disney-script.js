// Scroll effect for header
window.addEventListener('scroll', function() {
    const header = document.querySelector('.disney-header');
    if (window.scrollY > 100) {
        header.classList.add('scrolled');
    } else {
        header.classList.remove('scrolled');
    }
});

// Smooth scrolling for navigation links
document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
        e.preventDefault();
        const target = document.querySelector(this.getAttribute('href'));
        if (target) {
            target.scrollIntoView({
                behavior: 'smooth',
                block: 'start'
            });
        }
    });
});

// Animation on scroll
const observerOptions = {
    threshold: 0.1,
    rootMargin: '0px 0px -50px 0px'
};

const observer = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
        if (entry.isIntersecting) {
            entry.target.style.opacity = '1';
            entry.target.style.transform = 'translateY(0)';
        }
    });
}, observerOptions);

// Observe elements for animation
document.querySelectorAll('.fade-in-up, .service-card, .feature-item, .visual-card').forEach(el => {
    el.style.opacity = '0';
    el.style.transform = 'translateY(30px)';
    el.style.transition = 'opacity 0.6s ease, transform 0.6s ease';
    observer.observe(el);
});

// Button click handlers
document.querySelectorAll('.btn-hero-primary, .btn-cta-primary').forEach(btn => {
    btn.addEventListener('click', function() {
        // Redirect to quote request page or open modal
        window.location.href = 'cotizacion.html';
    });
});

document.querySelectorAll('.btn-hero-secondary, .btn-service').forEach(btn => {
    btn.addEventListener('click', function() {
        // Scroll to services section
        document.getElementById('servicios').scrollIntoView({
            behavior: 'smooth'
        });
    });
});