window.brUi = window.brUi || {};

window.brUi.getRelativeY = (el, clientY) => {
    if (!el) return 0;
    const rect = el.getBoundingClientRect();
    return clientY - rect.top;
};
