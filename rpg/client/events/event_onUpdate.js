function debug() {
	if(menu_opened) {
		start = new Point(256,420)
		space = 20
		actions = ["Als Fahrer in den Adder einsteigen","Als Passagier in den Adder einsteigen","Motor an","Motor aus","Aussteigen","Agim stinkt","Action 7","Action 8","Action 9","Action 10","Action 11","Action 12"]

		for(var i = 0; i < actions.length; i++)
		{
			if(i === action_selected) {
				API.drawText(actions[i], start.X, start.Y + (space *(i+1)), .3, 255, 0, 0, 255, 4, 0, false, true, 256)
			} else {
				API.drawText(actions[i], start.X, start.Y + (space *(i+1)), .3, 255, 255, 255, 255, 4, 0, false, true, 256)
			}
		}		
	}
}

API.onKeyDown.connect(function (sender, e) {
	if (e.KeyCode === Keys.E) {
		API.showCursor(true)
		menu_opened = true
	}
})

API.onKeyUp.connect(function (sender, e) {
	if (e.KeyCode === Keys.E) {
		API.showCursor(false)
		menu_opened = false
	}
})

API.onUpdate.connect(function() {
	misc_markers()
	hudUpdate()
	debug()
})