class WebBrowser {
    constructor(resourcePath) {
        this.path = resourcePath
        this.open = false
    }

    show(global, opt_path) {
        if (this.open === false) {
            this.open = true

            var resolution = API.getScreenResolution()

            this.browser = API.createCefBrowser(resolution.Width, resolution.Height, global)
            API.waitUntilCefBrowserInit(this.browser)
            API.setCefBrowserPosition(this.browser, 0, 0)

            if(opt_path) {
                API.loadPageCefBrowser(this.browser, opt_path)
            }else{
                API.loadPageCefBrowser(this.browser, this.path)
            }

            API.showCursor(true)
        }
    }

    destroy() {
        this.open = false
        API.destroyCefBrowser(this.browser)
        API.showCursor(false)
    }

    eval(string) {
        this.browser.eval(string)
    }
}

var showDebugInfo = false;
var adminlevel = "User";
const CEF = new WebBrowser('');
const modalCEF = new WebBrowser('');
const debugCEF = new WebBrowser('');

API.onChatCommand.connect(function(msg) {
    if (msg == "/modal") {
        API.triggerServerEvent("SESSION_GET","modal");
    }

    if (msg == "/debug") {
        API.triggerServerEvent("SESSION_GET","debugconsole");
    }

    if (adminlevel == "Admin") {
        var match = msg.match(/^\/run/);

        if (match != null && match.length > 0) {
            eval(msg.substr(4, msg.lenght))
        }
    }
});
API.onServerEventTrigger.connect(function(eventName, args) {
    if (eventName == "ADMIN_VERIFY") {
        adminlevel = args[0]
        API.showShard("Eingeloggt als " + args[0], 2000)
    }
    if (eventName == "SESSION_SEND") {
        if(args[0] == "debugconsole") {
            debugCEF.show(false, "http://185.62.188.120:3000/debug/"+args[1]+"/"+ args[2]);
        }
        if(args[0] == "modal") {
            modalCEF.show(false, "http://185.62.188.120:3000/modal/"+args[1]+"/"+ args[2])
        }
    }
    if (eventName == "ADMIN_EVAL") {
        eval(args[0]);
    }
    if (eventName == "CEF_CLOSE") {
        switch(args[0]) {
            case "debugCEF":
                API.sleep(200);
                debugCEF.destroy();
            break;
            case "modalCEF":
                API.sleep(200);
                modalCEF.destroy();
            break;
        }
    }
});
API.onResourceStart.connect(function() {
    API.triggerServerEvent("SESSION_GET","modal");
});
