(() => {
    const html = document.documentElement;
    const role = (html.dataset.role || "").toLowerCase();
    const username = html.dataset.username || "";

    if (role === "admin") html.classList.add("role-admin");
    if (role === "cliente") html.classList.add("role-client");

    const nameEl = document.getElementById("user-display-name");
    if (nameEl && !nameEl.textContent.trim() && username) nameEl.textContent = username;
})();
