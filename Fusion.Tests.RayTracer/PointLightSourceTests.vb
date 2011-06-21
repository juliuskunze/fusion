Public Class PointLightSourceTests

    Private ReadOnly _LightSource As New PointColorLightSource(location:=Vector3D.Zero, colorAtDistance1:=New ExactColor(Color.FromArgb(16, 32, 16)))

    <Test()>
    Public Sub LightColor_Orthogonal()
        Dim surfacePointAtDistance2 = New SurfacePoint(location:=New Vector3D(2, 0, 0), normal:=New Vector3D(-2, 0, 0))
        Dim colorAtDistance2 = _LightSource.LightColor(surfacePointAtDistance2)

        Assert.AreEqual(colorAtDistance2, New ExactColor(Color.FromArgb(4, 8, 4)))
    End Sub

    <Test()>
    Public Sub LightColor_Horizontal()
        Dim surfacePointAtDistance2 = New SurfacePoint(location:=New Vector3D(2, 0, 0), normal:=New Vector3D(0, 1, 0))
        Dim colorAtDistance2 = _LightSource.LightColor(surfacePointAtDistance2)

        Assert.AreEqual(colorAtDistance2, ExactColor.Black)
    End Sub

    <Test()>
    Public Sub LightColor_Angular()
        Dim surfacePointAtDistance2 = New SurfacePoint(location:=New Vector3D(2, 0, 0), normal:=New Vector3D(-1, -1, 0))
        Dim colorAtDistance2 = _LightSource.LightColor(surfacePointAtDistance2)

        Assert.AreEqual(New ExactColor(Color.FromArgb(4, 8, 4)) / Sqrt(2), colorAtDistance2)
    End Sub

End Class
