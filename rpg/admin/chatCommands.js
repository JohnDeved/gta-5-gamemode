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
const debugCEF_old = new WebBrowser('');
const modalCEF = new WebBrowser('');
const debugCEF = new WebBrowser('');
const testCEF = new WebBrowser('');

API.onChatCommand.connect(function(msg) {
    if (msg == "/modal") {
        API.triggerServerEvent("SESSION_GET","modal");
    }

    if (msg == "/debugnew") {
        API.triggerServerEvent("SESSION_GET","debugconsole");
    }

    if (msg == "/web") {
        testCEF.show(false)
    }

    if (msg == "/debug") {
        debugCEF_old.show(true)

        debugCEF_old.eval('document.write(\'\
        <!doctype html>\
        <html>\
            <head>\
                <meta charset="utf-8">\
                <meta name="viewport" content="width=device-width, initial-scale=1">\
\
                <link rel="stylesheet" href="styles.css">\
            </head>\
            <body>\
                <div style="text-align:center;">\
                    <textarea id="textfield" name="Text1" cols="100" rows="20"></textarea>\
                    <br/>\
                    <button id="buttonClose">Konsole schließen</button>\
                    <button id="executeLocal">JS Ausführen</button>\
                </div>\
            </body>\
        </html>\
        \');');

        debugCEF_old.eval("\
        document.getElementById('executeLocal').onclick = function () {\
            try\
            {\
                resourceCall('eval',document.getElementById('textfield').value);\
            }\
            catch(err) {\
                document.getElementById('executeLocal').innerHTML = err;\
            }\
        };\
        document.getElementById('buttonClose').onclick = function () {\
            try\
            {\
                resourceCall('debugCEF_old.destroy');\
            }\
            catch(err) {\
                document.getElementById('buttonClose').innerHTML = err;\
            }\
        };\
    ");
    }
    if (msg == "/hide") {
        debugCEF_old.destroy()
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
                debugCEF.destroy();
            break;
            case "modalCEF":
                modalCEF.destroy();
            break;
        }
    }
});
API.onResourceStart.connect(function() {
});
