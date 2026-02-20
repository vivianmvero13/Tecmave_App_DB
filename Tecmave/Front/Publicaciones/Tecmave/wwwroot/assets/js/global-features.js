document.addEventListener('DOMContentLoaded', () => {
    // Language switcher logic
    const languageSelect = document.getElementById('language-select');
    let translations = {};

    async function loadTranslations() {
        try {
            // Adjust the path to be relative from any page in the 'public' directory
            const response = await fetch('../assets/js/languages.json');
            if (!response.ok) {
                console.error('Failed to load languages.json');
                return;
            }
            translations = await response.json();
            const savedLanguage = localStorage.getItem('language') || 'es';
            setLanguage(savedLanguage);
        } catch (error) {
            console.error('Error loading or parsing languages.json:', error);
        }
    }

    function setLanguage(lang) {
        localStorage.setItem('language', lang);
        if(languageSelect) languageSelect.value = lang;
        translatePage(lang);
    }

    function translatePage(lang) {
        if (!translations[lang]) {
            console.warn(`Translations for language "${lang}" not found.`);
            return;
        }
        document.querySelectorAll('[data-key]').forEach(element => {
            const key = element.getAttribute('data-key');
            if (translations[lang][key]) {
                const childNodes = Array.from(element.childNodes);
                const textNode = childNodes.find(node => node.nodeType === Node.TEXT_NODE && node.textContent.trim().length > 0);
                if(textNode) {
                    textNode.textContent = translations[lang][key];
                } else {
                     element.textContent = translations[lang][key];
                }
            }
        });
    }

    if(languageSelect) {
        languageSelect.addEventListener('change', (event) => {
            setLanguage(event.target.value);
        });
    }

    // Theme switcher logic
    const themeToggleButton = document.getElementById('theme-toggle-btn');
    const body = document.body;

    const applyTheme = () => {
        const isDarkMode = localStorage.getItem('theme') === 'dark';
        if (isDarkMode) {
            body.classList.add('dark-mode');
            if(themeToggleButton) themeToggleButton.innerHTML = '<i class="fas fa-sun"></i>';
        } else {
            body.classList.remove('dark-mode');
            if(themeToggleButton) themeToggleButton.innerHTML = '<i class="fas fa-moon"></i>';
        }
    };

    if(themeToggleButton) {
        themeToggleButton.addEventListener('click', () => {
            if (body.classList.contains('dark-mode')) {
                localStorage.setItem('theme', 'light');
            } else {
                localStorage.setItem('theme', 'dark');
            }
            applyTheme();
        });
    }

    // Initial load calls
    loadTranslations();
    applyTheme();
});
