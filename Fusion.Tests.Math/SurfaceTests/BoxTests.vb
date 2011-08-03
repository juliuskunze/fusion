Public Class BoxTests

    <Test()>
    Public Sub Test()
        Dim box = New Box(New Vector3D(0, 1, 0), New Vector3D(1, 0, 1))

        Assert.That(box.Contains(New Vector3D(0.5, 0.5, 0.5)))
        Assert.That(Not box.Contains(New Vector3D(0.5, 1.5, 0.5)))

        Dim intersection = box.FirstIntersection(New Ray(New Vector3D(1.5, 0.5, 0.5), New Vector3D(-1, 0, 0)))
        Assert.That(intersection.NormalizedNormal = New Vector3D(1, 0, 0))
        Assert.That(intersection.Location = New Vector3D(1, 0.5, 0.5))

        Assert.That(box.FirstIntersection(New Ray(New Vector3D(1.5, 1.5, 0.5), New Vector3D(-1, 0, 0))) Is Nothing)
    End Sub

End Class
