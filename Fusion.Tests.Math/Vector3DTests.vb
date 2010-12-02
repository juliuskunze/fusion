Public Class Vector3DTests

    <Test()>
    Public Sub CrossProduct()
        Assert.That(New Vector3D(1, 0, 0).CrossProduct(New Vector3D(0, 1, 0)) = New Vector3D(0, 0, 1))
    End Sub

    <Test()>
    Public Sub OrthogonalProjection()
        Assert.That(New Vector3D(-2, -2, 0).OrthogonalProjectionOn(New Vector3D(1, 0, 0)) = New Vector3D(-2, 0, 0))
    End Sub

    Public Sub OrthogonalProjection2()
        Assert.That(New Vector3D(-2, -2, 0).OrthogonalProjectionOn(New Vector3D(1, 0, 0)) = New Vector3D(-2, 0, 0))
    End Sub

End Class
