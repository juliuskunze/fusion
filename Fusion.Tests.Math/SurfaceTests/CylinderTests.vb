Public Class CylinderTests

    <Test()>
    Public Sub Contains()
        Dim cylinder = New Cylinder(Vector3D.Zero, New Vector3D(2.5, 0, 0), 1)

        Assert.That(Not cylinder.Contains(New Vector3D(3, 0, 0)))
    End Sub

    <Test()>
    Public Sub FirstIntersection()
        Dim cylinder = New Cylinder(Vector3D.Zero, New Vector3D(2.5, 0, 0), 1)
        Dim ray = New Ray(New Vector3D(4, 0.5, 0.5), direction:=New Vector3D(-1, 0, 0))
        Dim intersection = cylinder.FirstIntersection(ray)

        Assert.AreEqual(New Vector3D(2.5, 0.5, 0.5), intersection.Location)
        Assert.AreEqual(New Vector3D(1, 0, 0), intersection.NormalizedNormal)
    End Sub

End Class
