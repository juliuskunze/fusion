Public Class IntersectedSurfaceTests
    <Test()>
    Public Sub Test()
        Dim sphere = New Sphere(Vector3D.Zero, 1)
        Dim antiSphere = New AntiSphere(New Vector3D(1, 0, 0), 0.5)
        Dim intersectionSurface = New IntersectedSurfacedPointSet3D(sphere, antiSphere)
        Dim ray1 = New Ray(New Vector3D(1, 0, 0), New Vector3D(-1, 0, 0))
        Dim intersection1 = intersectionSurface.Intersections(ray1).First
        Assert.AreEqual(New Vector3D(0.5, 0, 0), intersection1.Location)

    End Sub
End Class
