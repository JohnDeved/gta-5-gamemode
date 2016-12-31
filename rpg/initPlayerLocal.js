API.onResourceStart.connect(function() {
    API.triggerServerEvent("ADMIN_VERIFY")
});
API.onChatMessage.connect(function(msg, e) {
	if (msg.substr(0,9) == "~!#cid#!:") {
		e.Cancel = true;
		API.sendChatMessage("security_session: " + msg.substr(9,msg.length));		
	}
});