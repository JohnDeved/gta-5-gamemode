function() debug {
	return 1+1
}

API.onUpdate.connect(function() {
	misc_markers()
	hudUpdate()
	debug()
})