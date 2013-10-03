Public Class View3DTests
    <Test()>
    Public Sub SightRay()
        Dim view = New View3D(observerEvent:=New SpaceTimeEvent,
                              lookAt:=New Vector3D(1, 0, 0),
                              upDirection:=New Vector3D(0, 1, 0),
                              horizontalViewAngle:=PI / 2)
        Dim sightRay = view.SightRay(viewPlaneLocation:=New Vector2D(1, 1))
        Assert.That(sightRay.OriginLocation = Vector3D.Zero)
        Assert.That(Vector3D.Fit(New Vector3D(1, 1, 1).Normalized, sightRay.NormalizedDirection))
    End Sub
End Class
