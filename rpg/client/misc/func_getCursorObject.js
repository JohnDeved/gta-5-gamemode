function getCursorObject() {
    var sensitivity = 0.1
    var clickedEntity = null
    var cursorCoords = API.screenToWorld(new PointF(1920 / 2, 1080 / 2))
    var vecLineXStart = new Vector3(cursorCoords.X + sensitivity, cursorCoords.Y, cursorCoords.Z)
    var vecLineXEnd = new Vector3(cursorCoords.X - sensitivity, cursorCoords.Y, cursorCoords.Z)
    var vecLineYStart = new Vector3(cursorCoords.X, cursorCoords.Y + sensitivity, cursorCoords.Z)
    var vecLineYEnd = new Vector3(cursorCoords.X, cursorCoords.Y - sensitivity, cursorCoords.Z)
    var vecLineZStart = new Vector3(cursorCoords.X, cursorCoords.Y, cursorCoords.Z + sensitivity)
    var vecLineZEnd = new Vector3(cursorCoords.X, cursorCoords.Y, cursorCoords.Z - sensitivity)
    var vecLineXYZStart = new Vector3(cursorCoords.X + sensitivity, cursorCoords.Y + sensitivity, cursorCoords.Z + sensitivity)
    var vecLineXYZEnd = new Vector3(cursorCoords.X - sensitivity, cursorCoords.Y - sensitivity, cursorCoords.Z - sensitivity)
    var vecLineXYStart = new Vector3(cursorCoords.X + sensitivity, cursorCoords.Y + sensitivity, cursorCoords.Z - sensitivity)
    var vecLineXYEnd = new Vector3(cursorCoords.X - sensitivity, cursorCoords.Y - sensitivity, cursorCoords.Z + sensitivity)
    var vecLineYZStart = new Vector3(cursorCoords.X - sensitivity, cursorCoords.Y + sensitivity, cursorCoords.Z + sensitivity)
    var vecLineYZEnd = new Vector3(cursorCoords.X + sensitivity, cursorCoords.Y - sensitivity, cursorCoords.Z - sensitivity)
    var vecLineXZStart = new Vector3(cursorCoords.X + sensitivity, cursorCoords.Y - sensitivity, cursorCoords.Z + sensitivity)
    var vecLineXZEnd = new Vector3(cursorCoords.X - sensitivity, cursorCoords.Y + sensitivity, cursorCoords.Z - sensitivity);
    [
        API.createRaycast(vecLineXStart, vecLineXEnd, -1, null),
        API.createRaycast(vecLineYStart, vecLineYEnd, -1, null),
        API.createRaycast(vecLineZStart, vecLineZEnd, -1, null),
        API.createRaycast(vecLineXYZStart, vecLineXYZEnd, -1, null),
        API.createRaycast(vecLineXYStart, vecLineXYEnd, -1, null),
        API.createRaycast(vecLineYZStart, vecLineYZEnd, -1, null),
        API.createRaycast(vecLineXZStart, vecLineXZEnd, -1, null)
    ].every(function(ray, index) {
        if (ray.didHitEntity) {
            clickedEntity = ray.hitEntity
            return false
        }
        return true
    })

    return clickedEntity
}
