API.onServerEventTrigger.connect(function(event, args) {
	switch(event)
	{
		case "SESSION_SEND":
			switch (args[0])
			{
				case "debug":
					debugCEF.show(false, "http://185.62.188.120:3000/debug/" + args[1] + "/" + args[2])
					break
				case "modal":
					modalCEF.show(false, "http://185.62.188.120:3000/modal/" + args[1] + "/" + args[2] + "/Testserver/Der Server ist in Moment in Entwicklung*Nur Entwickler sind online, es gibt noch nichts zu sehen.*Willst du fortfahren?")
					break
				case "start"
					startCEF.show(false, "http://185.62.188.120:3000/start/" + args[1] + "/" + args[2])
					break
				case "clothing"
					clothCEF.show(false, "http://185.62.188.120:3000/clothing/" + args[1] + "/" + args[2])
					break
			}
			break
		case "CEF_CLOSE":
			switch (args[0])
			{
				case "debugCEF":
					API.sleep(200)
					debugCEF.destroy()
					break
				case "modalCEF":
					API.sleep(200)
					modalCEF.destroy()
					break
				case "startCEF":
					API.sleep(200)
					startCEF.destroy()
					break
				case "clothingCEF":
					API.sleep(200)
					clothCEF.destroy()
					break
			}		
			break;
	}
})