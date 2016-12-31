API.onResourceStart.connect(function() {
    API.triggerServerEvent("ADMIN_VERIFY")
    API.triggerServerEvent("SESSION_INIT")
    modalCEF.show()
});
API.onServerEventTrigger.connect(function(eventName, args) {
    if (eventName == "ADMIN_VERIFY") {
        API.adminlevel = args[0]
        API.showShard("Eingeloggt als " + args[0], 2000)
    }
});