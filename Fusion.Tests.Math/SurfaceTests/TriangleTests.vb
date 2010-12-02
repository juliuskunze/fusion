Public Class TriangleTests

    Private ReadOnly _triangle As New Triangle(vertex1:=New Vector3D(0, 0, 0),
                                              vertex2:=New Vector3D(0, 0, 1),
                                              vertex3:=New Vector3D(0, 1, 0))

    <Test()>
    Public Sub FirstIntersection()
        Dim ray = New Ray(origin:=New Vector3D(-1, 0.25, 0.25),
                          direction:=New Vector3D(1, 0, 0))
        Dim firstIntersection = _triangle.Intersection(ray)
        Assert.That(firstIntersection.NormalizedNormal = New Vector3D(-1, 0, 0))
        Assert.AreEqual(New Vector3D(0, 0.25, 0.25), firstIntersection.Location)
        SurfaceTests.SurfaceRayIntersection(_triangle, ray)
    End Sub

    <Test()>
    Public Sub FirstIntersection_NoIntersection()
        Dim ray = New Ray(origin:=New Vector3D(1, 1, 1),
                          direction:=New Vector3D(-1, 0, 0))
        Dim firstIntersection = _triangle.Intersection(ray)
        Assert.AreEqual(Nothing, firstIntersection)
    End Sub

    <Test()>
    Public Sub FirstIntersection_NoIntersectionFromBehind()
        Dim ray = New Ray(origin:=New Vector3D(1, 0.25, 0.25),
                          direction:=New Vector3D(-1, 0, 0))
        Dim firstIntersection = _triangle.Intersection(ray)
        Assert.AreEqual(Nothing, firstIntersection)
        SurfaceTests.SurfaceRayIntersection(_triangle, ray)
    End Sub

End Class
