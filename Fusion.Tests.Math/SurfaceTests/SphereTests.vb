Public Class SphereTests
    <Test()>
    Public Sub FirstIntersection_RayStartsOutside()
        Dim sphere = New Sphere(center:=Vector3D.Zero, radius:=1)
        Dim ray = New Ray(origin:=New Vector3D(5, 0, 0),
                          direction:=New Vector3D(-1, 0, 0))
        Dim firstIntersection = sphere.Intersection(ray)
        Assert.AreEqual(firstIntersection.Location, New Vector3D(1, 0, 0))
        Assert.That(firstIntersection.NormalizedNormal = New Vector3D(1, 0, 0))
        SurfaceTests.SurfaceRayIntersection(sphere, ray)
    End Sub

    <Test()>
    Public Sub FirstIntersection_RayStartsInside()
        Dim sphere = New Sphere(center:=Vector3D.Zero, radius:=1)
        Dim ray = New Ray(origin:=Vector3D.Zero,
                          direction:=New Vector3D(1, 0, 0))
        Dim firstIntersection = sphere.Intersection(ray)
        Assert.AreEqual(Nothing, firstIntersection)
        SurfaceTests.SurfaceRayIntersection(sphere, ray)
    End Sub

    <Test()>
    Public Sub FirstIntersection_NoIntersection()
        Dim sphere = New Sphere(center:=Vector3D.Zero, radius:=1)
        Dim ray = New Ray(origin:=New Vector3D(5, 3, 4),
                          direction:=New Vector3D(-1, 0, 0))
        Dim firstIntersection = sphere.Intersection(ray)
        Assert.AreEqual(firstIntersection, Nothing)
    End Sub
End Class
