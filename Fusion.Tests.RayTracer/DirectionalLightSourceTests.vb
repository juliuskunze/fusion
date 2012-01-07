Public Class DirectionalLightSourceTests

    <Test()>
    Public Sub Test()

        Dim directionalLightSource = New DirectionalLightSource(Of RadianceSpectrum)(direction:=New Vector3D(1, 0, 0), light:=New RadianceSpectrum(Function() 1))
        Assert.AreEqual(1, directionalLightSource.GetLight(New SurfacePoint(New Vector3D, New Vector3D(-1, 0, 0))).GetSpectralRadiance(1))
        Assert.AreEqual(0, directionalLightSource.GetLight(New SurfacePoint(New Vector3D, New Vector3D(1, 0, 0))).GetSpectralRadiance(1))

    End Sub

End Class
