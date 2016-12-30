class CefHelper {
  constructor (resourcePath) {
    this.path = resourcePath
    this.open = false
  }

  show () {
    if (this.open === false) {
      this.open = true

      var resolution = API.getScreenResolution()

      this.browser = API.createCefBrowser(resolution.Width, resolution.Height, true)
      API.waitUntilCefBrowserInit(this.browser)
      API.setCefBrowserPosition(this.browser, 0, 0)
      API.loadPageCefBrowser(this.browser, this.path)
      API.showCursor(true)
    }
  }

  destroy () {
    this.open = false
    API.destroyCefBrowser(this.browser)
    API.showCursor(false)
  }

  eval (string) {
    this.browser.eval(string)
  }
}

const index = new CefHelper('html/index.html')

var drawSkeletor = false;
var indexCef = false;

API.onUpdate.connect(function (sender, args) {
    if (drawSkeletor) {
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
	if (msg == "/box") {
		if (indexCef) {
            index.destroy()
            indexCef = false;
		} else {
            index.show()
            indexCef = true;
		}
	}
    if (msg.match(/^\/run/) != null) {
        eval(msg.substr(4, msg.lenght));
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
