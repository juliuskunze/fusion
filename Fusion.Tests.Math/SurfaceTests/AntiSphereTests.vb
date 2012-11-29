Public Class AntiSphereTests
    <Test()>
    Public Sub FirstIntersection_RayStartsInside()
        Dim antiSphere = New AntiSphere(center:=Vector3D.Zero,
                                        radius:=1)
        Dim ray = New Ray(origin:=Vector3D.Zero,
                          direction:=New Vector3D(1, 0, 0))
        Dim intersection = antiSphere.Intersection(ray)
        Assert.AreEqual(intersection.Location, New Vector3D(1, 0, 0))
        Assert.That(intersection.NormalizedNormal = New Vector3D(-1, 0, 0))
        SurfaceTests.SurfaceRayIntersection(antiSphere, ray)
    End Sub

    <Test()>
    Public Sub FirstIntersection_RayStartsOutside()
        Dim antiSphere = New AntiSphere(center:=Vector3D.Zero, radius:=1)
        Dim ray = New Ray(origin:=New Vector3D(5, 0, 0),
                          direction:=New Vector3D(-1, 0, 0))
        Dim intersection = antiSphere.Intersection(ray)
        Assert.AreEqual(intersection.Location, New Vector3D(-1, 0, 0))
        Assert.That(intersection.NormalizedNormal = New Vector3D(1, 0, 0))

        Dim intersections = antiSphere.Intersections(ray)
        Assert.That(intersections.Count = 1)
        Assert.That(intersections.First.Location = intersection.Location)

        SurfaceTests.SurfaceRayIntersection(antiSphere, ray)
    End Sub
End Class
