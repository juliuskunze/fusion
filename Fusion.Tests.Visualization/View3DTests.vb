Public Class View3DTests
    <Test()>
    Public Sub SightRay()
        Dim view = New View3D(observerLocation:=Vector3D.Zero,
                              lookAt:=New Vector3D(1, 0, 0),
                              upDirection:=New Vector3D(0, 1, 0),
                              horizontalViewAngle:=PI / 2)
        Dim sightRay = view.SightRay(viewPlaneLocation:=New Vector2D(1, 1))
        Assert.That(sightRay.Origin = Vector3D.Zero)
        Assert.That(Vector3D.Fit(New Vector3D(1, 1, 1).Normalized, sightRay.NormalizedDirection))
    End Sub
End Class
