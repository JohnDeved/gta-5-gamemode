class WebBrowser {
    constructor(resourcePath) {
        this.path = resourcePath
        this.open = false
    }

    show(global) {
        if (this.open === false) {
            this.open = true

            var resolution = API.getScreenResolution()

            this.browser = API.createCefBrowser(resolution.Width, resolution.Height, global)
            API.waitUntilCefBrowserInit(this.browser)
            API.setCefBrowserPosition(this.browser, 0, 0)
            API.loadPageCefBrowser(this.browser, this.path)
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
const CEF = new WebBrowser('html/debug.html');
const debugCEF_old = new WebBrowser('');
const modalCEF = new WebBrowser('html/modal.html');
const testCEF = new WebBrowser('http://185.62.188.120:3000/');
const debugCEF = new WebBrowser('html/debug.html');

API.onChatCommand.connect(function(msg) {
    if (msg == "/modal") {
        modalCEF.show(true)
    }

    if (msg == "/debugnew") {
        debugCEF.show(true)
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
});
API.onResourceStart.connect(function() {
    modalCEF.show(true)
});