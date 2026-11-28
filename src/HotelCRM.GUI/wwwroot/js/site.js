// Sidebar collapse (B2). The class sits on <html> so the inline script in _Layout's <head>
// can restore it before the first paint; localStorage remembers the choice between pages.
const toggle = document.getElementById("hc-toggle");
if (toggle) {
    const root = document.documentElement;
    toggle.setAttribute("aria-expanded", String(!root.classList.contains("hc-collapsed")));
    toggle.addEventListener("click", () => {
        const collapsed = root.classList.toggle("hc-collapsed");
        toggle.setAttribute("aria-expanded", String(!collapsed));
        try {
            localStorage.setItem("hc-side", collapsed ? "collapsed" : "open");
        } catch {
            // Storage blocked (private window): the sidebar still toggles, the choice is just not remembered.
        }
    });
}
