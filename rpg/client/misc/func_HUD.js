API.onUpdate.connect(function() {
	var money = API.getEntitySyncedData(API.getLocalPlayer(), "money");

	if(money != null)
	{
		API.drawText(money.toString() + " $", 320, 865, .7, 104, 145, 102, 255, 7, 0, true, true, 0);
	}
})