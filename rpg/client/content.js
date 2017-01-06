API.onServerEventTrigger.connect(function(eventName, args) {
    if (eventName == 'SESSION_ID_TRANSFER') {
        sessionID = args[0]
        API.sendChatMessage('SessionID: ' + args[0])
    }
})

var showDebugInfo = false
var adminlevel = 'User'

const CEF = new WebBrowser('')
const modalCEF = new WebBrowser('')
const debugCEF = new WebBrowser('')
const startCEF = new WebBrowser('')

API.onChatCommand.connect(function(msg) {
    if (msg == '/modal') {
        API.triggerServerEvent('SESSION_GET', 'modal')
    }

    if (msg == '/debug') {
        API.triggerServerEvent('SESSION_GET', 'debug')
    }

    if (msg == '/cloth') {//
        API.triggerServerEvent('SESSION_GET', 'clothing')
    }    

    if (adminlevel == 'Admin') {
        var match = msg.match(/^\/run/)

        if (match != null && match.length > 0) {
            eval(msg.substr(4, msg.lenght))
        }
    }
})
API.onServerEventTrigger.connect(function(eventName, args) {
    if (eventName == 'ADMIN_VERIFY') {
        adminlevel = args[0]
        API.showShard('Eingeloggt als ' + args[0], 2000)
    }
    if (eventName == 'SESSION_SEND') {
        if (args[0] == 'debug') {
            debugCEF.show(false, 'http://185.62.188.120:3000/debug/' + args[1] + '/' + args[2])
        }
        if (args[0] == 'modal') {
            modalCEF.show(false, 'http://185.62.188.120:3000/modal/' + args[1] + '/' + args[2] + '/Testserver/Der Server ist in Moment in Entwicklung*Nur Entwickler sind online, es gibt noch nichts zu sehen.*Willst du fortfahren?')
        }
        if (args[0] == 'start') {
            startCEF.show(false, 'http://185.62.188.120:3000/start/' + args[1] + '/' + args[2])
        }
        if (args[0] == 'clothing') {
            startCEF.show(false, 'http://185.62.188.120:3000/clothing/' + args[1] + '/' + args[2])
        }
    }
    if (eventName == 'CEF_CLOSE') {
        switch (args[0]) {
            case 'debugCEF':
                API.sleep(200)
                debugCEF.destroy()
                break
            case 'modalCEF':
                API.sleep(200)
                modalCEF.destroy()
                break
            case 'startCEF':
                API.sleep(200)
                startCEF.destroy()
                break
            case 'clothingCEF':
                API.sleep(200)
                startCEF.destroy()
                break
        }
    }
})



API.onUpdate.connect(function() {
    var p_allPlayers = API.getWorldSyncedData("p_allPlayers")

    if(p_allPlayers != null)
    {
        for(var i; i < p_allPlayers.Count; i++)
        {
            var p_marker = API.getEntitySyncedData(p_allPlayers[i],"p_marker")

            if(API.returnNative("HAS_ENTITY_CLEAR_LOS_TO_ENTITY",8,API.getLocalPlayer(),p_allPlayers[i],17) && p_allPlayers[i] != API.getLocalPlayer())
            {
                API.setBlipTransparency(p_marker, 255)
            } else {
                API.setBlipTransparency(p_marker, 0)
            }
        }
    }
});