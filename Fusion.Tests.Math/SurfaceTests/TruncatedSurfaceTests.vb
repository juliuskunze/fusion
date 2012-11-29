Public Class TruncatedSurfaceTests
    <Test()>
    Public Sub Test()
        Dim truncatingPointSet = New AntiSphere(Vector3D.Zero, 1)
        Dim baseSurface = New AntiSphere(New Vector3D(1, 0, 0), 0.5)
        Dim truncatedSurface = New TruncatedSurface(baseSurface:=baseSurface, truncatingPointSet:=truncatingPointSet)
        Dim ray1 = New Ray(New Vector3D(1, 0, 0), New Vector3D(-1, 0, 0))
        Dim intersection1 = truncatedSurface.Intersections(ray1).First
        Assert.AreEqual(New Vector3D(0.5, 0, 0), intersection1.Location)

        Dim ray2 = New Ray(New Vector3D(-1, 0, 0), New Vector3D(2, 0, 0))
        Assert.That(truncatedSurface.Intersections(ray2).Count = 0)
    End Sub
End Class
