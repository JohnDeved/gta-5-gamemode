function hudUpdate() {
    var money = API.getEntitySyncedData(API.getLocalPlayer(), 'money')

    var p_allPlayers = API.getWorldSyncedData('p_allPlayers')

    if (p_allPlayers != null) {
        for (var i = 0; i < p_allPlayers.Count; i++) {
            if (p_allPlayers[i] != null) {
                var player = p_allPlayers[i]
                var head_pos = API.returnNative('GET_PED_BONE_COORDS', 5, player, 12844, 0)
                var local_pos = API.getEntityPosition(API.getLocalPlayer())

                var distance = Math.sqrt(Math.pow(head_pos.X - local_pos.X, 2) + Math.pow(head_pos.Y - local_pos.Y, 2) + Math.pow(head_pos.Z - local_pos.Z, 2))

                skel_head = API.worldToScreen(new Vector3(head_pos.X, head_pos.Y, head_pos.Z + 0.3))

                if (!API.isPlayerInAnyVehicle(player) && API.returnNative('HAS_ENTITY_CLEAR_LOS_TO_ENTITY', 8, API.getLocalPlayer(), player, 17) && distance < 15) {
                    if (API.isPlayerDead(player)) {
                        API.drawText(API.getEntitySyncedData(player, 'name'), skel_head.X, skel_head.Y, 0.3, 75, 75, 75, 255, 4, 1, false, true, 256)
                    } else {
                        switch (API.getEntitySyncedData(player, 'role')) {
                            case 'Civilian':
                                API.drawText(API.getEntitySyncedData(player, 'name'), skel_head.X, skel_head.Y, 0.3, 181, 0, 0, 255, 4, 1, false, true, 256)
                                break
                            case 'Police':
                                API.drawText(API.getEntitySyncedData(player, 'name'), skel_head.X, skel_head.Y, 0.3, 0, 76, 152, 255, 4, 1, false, true, 256)
                                break
                            case 'Medic':
                                API.drawText(API.getEntitySyncedData(player, 'name'), skel_head.X, skel_head.Y, 0.3, 255, 187, 0, 255, 4, 1, false, true, 256)
                                break
                        }
                    }
                }
            }
        }
    }
}
