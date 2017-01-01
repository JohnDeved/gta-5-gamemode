API.onResourceStart.connect(function() {
    API.triggerServerEvent("SESSION_INIT")
});

API.onServerEventTrigger.connect(function(eventName, args) {
    if (eventName == "SESSION_ID_TRANSFER") {
        sessionID = args[0];
        API.sendChatMessage("SessionID: " + args[0]);
    }
});