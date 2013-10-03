Public Class NormalizedMidpointCoordinateSystemTests
    <Test()>
    Public Sub VirtualLocation()

        Dim coordinateSystem = New NormalizedMidpointCoordinateSystem(pictureSize:=New Vector2D(100, 20))

        Assert.AreEqual(Vector2D.Zero, coordinateSystem.VirtualLocation(New Vector2D(50, 10)))
        Assert.AreEqual(New Vector2D(1, 0), coordinateSystem.VirtualLocation(New Vector2D(100, 10)))
        Assert.AreEqual(New Vector2D(-1, 0), coordinateSystem.VirtualLocation(New Vector2D(0, 10)))
        Assert.AreEqual(New Vector2D(0, 1 / 5), coordinateSystem.VirtualLocation(New Vector2D(50, 0)))
    End Sub
End Class
