API.onResourceStart.connect(function() {
    API.triggerServerEvent("ADMIN_VERIFY")
    API.triggerServerEvent("SESSION_INIT")
});