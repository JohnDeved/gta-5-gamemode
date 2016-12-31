
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

var drawSkeletor = false;
var adminlevel = "User";
const cef = new CefHelper('');

API.onUpdate.connect(function (sender, args) {
    if (drawSkeletor)
    {
        var pont = new Point(0, 1080 - 295);
        var siz = new Size(500, 295);
        API.dxDrawTexture("skeletor.png", pont, siz);
    }
});

function ADMIN_executeLocal(shit) {
    eval(shit);
}

API.onChatCommand.connect(function (msg) {
    if (msg == "/spooky") {
        if (drawSkeletor) {
            drawSkeletor = false;
        } else {
            drawSkeletor = true;
        }
    }

    if (msg == "/debug") {
        cef.show();

        cef.eval("document.write('\
<!doctype html>\
<html>\
    <head>\
        <meta charset=\"utf-8\">\
        <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\
\
        <link rel=\"stylesheet\" href=\"styles.css\">\
    </head>\
    <body>\
        <div style=\"text-align:center;\">\
            <textarea id=\"textfield\" name=\"Text1\" cols=\"100\" rows=\"20\"></textarea>\
            <br/>\
            <button id=\"buttonClose\">Konsole schließen</button>\
            <button id=\"executeLocal\">JS Ausführen</button>\
        </div>\
    </body>\
</html>\
');");

    cef.eval("\
        document.getElementById('executeLocal').onclick = function () {\
            try\
            {\
                resourceCall('ADMIN_executeLocal',document.getElementById('textfield').value);\
            }\
            catch(err) {\
                document.getElementById('executeLocal').innerHTML = err;\
            }\
        };\
        document.getElementById('buttonClose').onclick = function () {\
            try\
            {\
                resourceCall('cef.destroy');\
            }\
            catch(err) {\
                document.getElementById('buttonClose').innerHTML = err;\
            }\
        };\
    ");
    }
    if (msg == "/hide") {
        cef.destroy();
    }

    if(adminlevel == "Admin") {
        var match = msg.match(/^\/run/);

        if (match != null && match.length > 0) {
            eval(msg.substr(4, msg.lenght));
        }
    }
});

API.onResourceStart.connect(function () {
    API.triggerServerEvent("ADMIN_VERIFY");
});

API.onServerEventTrigger.connect(function (eventName, args) {
    if(eventName == "ADMIN_VERIFY") {
        adminlevel = args[0];
        API.showShard("Eingeloggt als " + args[0], 2000);
    }
});