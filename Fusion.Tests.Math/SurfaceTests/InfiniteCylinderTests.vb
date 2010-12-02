Public Class InfiniteCylinderTests

    <Test()>
    Public Sub Contains()
        Dim infiniteCylinder = New InfiniteCylinder(New Vector3D(5, 0, 0), New Vector3D(0, 2, 0), 2)

        Assert.That(infiniteCylinder.Contains(New Vector3D(5, 7, 1)))
        Assert.That(Not infiniteCylinder.Contains(New Vector3D(2, 0, 0)))
    End Sub
    
    <Test()>
    Public Sub FirstIntersection()
        Dim infiniteCylinder = New InfiniteCylinder(New Vector3D(17, 0, 0), direction:=New Vector3D(0, 1, 0), radius:=2)
        Dim ray = New Ray(origin:=New Vector3D(0, 10, 0), direction:=New Vector3D(1, 0, 0))

        Assert.AreEqual(New Vector3D(15, 10, 0), infiniteCylinder.Intersection(ray).Location)
        Assert.AreEqual(New Vector3D(-1, 0, 0), infiniteCylinder.Intersection(ray).NormalizedNormal)
    End Sub

End Class
