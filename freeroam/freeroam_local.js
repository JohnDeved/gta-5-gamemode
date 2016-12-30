
var drawSkeletor = false;

API.onUpdate.connect(function (sender, args) {    
    if (drawSkeletor)
    {
        var pont = new Point(0, 1080 - 295);
        var siz = new Size(500, 295);
        API.dxDrawTexture("skeletor.png", pont, siz);        
    }
});

API.onChatCommand.connect(function (msg) {
	if (msg == "/spooky") {
		if (drawSkeletor) {
			drawSkeletor = false;
		} else {
			drawSkeletor = true;
		}
	}
	if (msg == "/run") {
		var splitted = msg.split('_').slice(1).join('_');
        if(splitted.legnth < 2) {
            API.showShard("Kein Code angegeben!", 2000);
        }else{
            eval(splitted[1]);
        }
	}
});

API.onServerEventTrigger.connect(function (evName, args) {
    if (evName == "startCountdown") {                
        API.callNative("REQUEST_SCRIPT_AUDIO_BANK", "HUD_MINI_GAME_SOUNDSET", true);
        API.callNative("PLAY_SOUND_FRONTEND", 0, "CHECKPOINT_NORMAL", "HUD_MINI_GAME_SOUNDSET");
        API.showShard("3");
        API.sleep(1000);
        API.callNative("REQUEST_SCRIPT_AUDIO_BANK", "HUD_MINI_GAME_SOUNDSET", true);
        API.callNative("PLAY_SOUND_FRONTEND", 0, "CHECKPOINT_NORMAL", "HUD_MINI_GAME_SOUNDSET");
        API.showShard("2");
        API.sleep(1000);
        API.callNative("REQUEST_SCRIPT_AUDIO_BANK", "HUD_MINI_GAME_SOUNDSET", true);
        API.callNative("PLAY_SOUND_FRONTEND", 0, "CHECKPOINT_NORMAL", "HUD_MINI_GAME_SOUNDSET");
        API.showShard("1");
        API.sleep(1000);
        API.callNative("REQUEST_SCRIPT_AUDIO_BANK", "HUD_MINI_GAME_SOUNDSET", true);
        API.callNative("PLAY_SOUND_FRONTEND", 0, "CHECKPOINT_NORMAL", "HUD_MINI_GAME_SOUNDSET");
        API.showShard("go!", 2000);       
    }
});