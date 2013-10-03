Public Class CylinderTests
    Private ReadOnly _Cylinder As Cylinder = New Cylinder(Vector3D.Zero, New Vector3D(2.5, 0, 0), 1)

    <Test()>
    Public Sub Contains()
        Assert.That(_Cylinder.Contains(New Vector3D(1, 0, 0)))
        Assert.That(Not _Cylinder.Contains(New Vector3D(3, 0, 0)))
    End Sub

    <Test()>
    Public Sub FirstIntersection_Front()
        Dim ray = New Ray(New Vector3D(4, 0.5, 0.5), direction:=New Vector3D(-1, 0, 0))
        Dim intersection = _Cylinder.FirstIntersection(ray)

        Assert.AreEqual(New Vector3D(2.5, 0.5, 0.5), intersection.Location)
        Assert.AreEqual(New Vector3D(1, 0, 0), intersection.NormalizedNormal)
    End Sub

    <Test()>
    Public Sub FirstIntersection_Side()
        Dim ray = New Ray(New Vector3D(1, 2, 0), direction:=New Vector3D(0, -1, 0))
        Dim intersection = _Cylinder.FirstIntersection(ray)

        Assert.AreEqual(New Vector3D(1, 1, 0), intersection.Location)
        Assert.AreEqual(New Vector3D(0, 1, 0), intersection.NormalizedNormal)
    End Sub

    <Test()>
    Public Sub NoIntersection_Side()
        Dim ray = New Ray(New Vector3D(10, 2, 0), direction:=New Vector3D(0, -1, 0))
        Dim intersection = _Cylinder.FirstIntersection(ray)

        Assert.AreEqual(Nothing, intersection)
    End Sub
End Class
