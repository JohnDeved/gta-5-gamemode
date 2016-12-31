API.onResourceStart.connect(function() {
    API.triggerServerEvent("SESSION_INIT")
});

API.onServerEventTrigger.connect(function(eventName, args) {
    if (eventName == "SESSION_ID_TRANSFER") {
        sessionID = args[0];
        API.sendChatMessage("SessionID: " + args[0]);
    }
});

API.onChatMessage.connect(function(msg, e) {
	if (msg.substr(0,9) == "~!#cid#!:") {
		e.Cancel = true;
		API.sendChatMessage("security_session: " + msg.substr(9,msg.length));		
	}
});