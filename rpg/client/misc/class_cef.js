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

            if (opt_path) {
                API.loadPageCefBrowser(this.browser, opt_path)
            } else {
                API.loadPageCefBrowser(this.browser, this.path)
            }

            API.setCanOpenChat(false);
            API.showCursor(true)
        }
    }

    destroy() {
        this.open = false
        API.destroyCefBrowser(this.browser)
        API.setCanOpenChat(true);
        API.showCursor(false)
    }

    eval(string) {
        this.browser.eval(string)
    }
}