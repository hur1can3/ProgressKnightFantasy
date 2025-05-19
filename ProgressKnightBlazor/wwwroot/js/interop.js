window.localStorageInterop = {
    saveGame: function (key, data) {
        try { localStorage.setItem(key, data); console.log("Chronicle saved:", key); }
        catch (e) { console.error("Error saving chronicle:", e); }
    },
    loadGame: function (key) {
        try { return localStorage.getItem(key); }
        catch (e) { console.error("Error loading chronicle:", e); return null; }
    },
    deleteSave: function (key) {
        try { localStorage.removeItem(key); console.log("Chronicle erased:", key); }
        catch (e) { console.error("Error erasing chronicle:", e); }
    }
};