function admin_chatCommands(msg) {
    if (msg == '/modal') {
        API.triggerServerEvent('SESSION_GET', 'modal')
    }

    if (msg == '/debug') {
        API.triggerServerEvent('SESSION_GET', 'debug')
    }

    if (msg == '/cloth') {
        API.triggerServerEvent('SESSION_GET', 'clothing')
    }

    var match = msg.match(/^\/run/)

    if (match != null && match.length > 0) {
        eval(msg.substr(4, msg.lenght))
    }
}
