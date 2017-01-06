function misc_markers() {
    var p_allPlayers = API.getWorldSyncedData('p_allPlayers')

    if (p_allPlayers != null) {
        for (var i = 0; i < p_allPlayers.Count; i++) {
            if (p_allPlayers[i] != null) {
                var p_name = API.getEntitySyncedData(p_allPlayers[i], 'name')
                var p_marker = API.getEntitySyncedData(p_allPlayers[i], 'p_marker')
                var l_marker = API.getEntitySyncedData(API.getLocalPlayer(), 'p_marker')

                if (p_name != null && p_marker != null && l_marker != null) {
                    if (API.returnNative('HAS_ENTITY_CLEAR_LOS_TO_ENTITY', 8, API.getLocalPlayer(), p_allPlayers[i], 17) && API.returnNative('IS_ENTITY_ON_SCREEN', 8, p_allPlayers[i])) {
                        API.setBlipTransparency(p_marker, 255)
                        API.setBlipName(p_marker, p_name)
                    } else {
                        API.setBlipTransparency(p_marker, 0)
                        API.setBlipName(p_marker, '')
                    }
                    API.setBlipTransparency(l_marker, 0)
                    API.setBlipName(l_marker, '')
                }
            }
        }
    }
}