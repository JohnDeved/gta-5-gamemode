API.onUpdate.connect(function() {
	var money = API.getEntitySyncedData(API.getLocalPlayer(), "money");

	var player = API.getLocalPlayer();
	var head_pos = API.returnNative("GET_PED_BONE_COORDS",5,player,12844,0);
	skel_head = API.worldToScreen(new Vector3(head_pos.X,head_pos.Y,head_pos.Z + .3));

	if(API.isPlayerDead(player)) {
		API.drawText(API.getEntitySyncedData(player,"name"), skel_head.X, skel_head.Y, .3, 75, 75, 75, 255, 4, 1, false, true, 256);
	} else {

		switch(API.getEntitySyncedData(player,"role"))
		{
			case"Civilian":
				API.drawText(API.getEntitySyncedData(player,"name"), skel_head.X, skel_head.Y, .3, 255, 255, 255, 255, 4, 1, false, true, 256);
				break;
			case"Police":
				API.drawText(API.getEntitySyncedData(player,"name"), skel_head.X, skel_head.Y, .3, 0, 76, 152, 255, 4, 1, false, true, 256);
				break;
			case"Medic":
				API.drawText(API.getEntitySyncedData(player,"name"), skel_head.X, skel_head.Y, .3, 255, 187, 0, 255, 4, 1, false, true, 256);
				break;
		}
	}	
})