API.onResourceStart.connect(function() {
    API.triggerServerEvent("ADMIN_VERIFY")
    API.triggerServerEvent("SESSION_INIT")
    window.modalCEF.show()
});
API.onServerEventTrigger.connect(function(eventName, args) {
    if (eventName == "ADMIN_VERIFY") {
        window.adminlevel = args[0]
        API.showShard("Eingeloggt als " + args[0], 2000)
    }
});