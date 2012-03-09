Public Class SingleObjectFrameRelativisticRayTracerTests

    <Test()>
    Public Sub Searchlight()
        Dim spectrum = Function(wavelength As Double) 1
        Assert.Greater(GetTestScene(spectrum).GetLight(observerSightRayWithObjectOrigin:=New Ray(New Vector3D, New Vector3D(1, 0, 0))).Function(1), spectrum(1))
    End Sub

    <Test()>
    Public Sub Doppler()
        Dim minimumWavelength = 10 ^ -7

        Dim spectrum = Function(wavelength As Double) If(wavelength > minimumWavelength, 1, 0)


        Dim transformedSpectrum = GetTestScene(spectrum).GetLight(New Ray(New Vector3D, New Vector3D(1, 0, 0))).Function

        Assert.Greater(transformedSpectrum(minimumWavelength), 0)
    End Sub

    Private Function GetTestScene(spectrum As SpectralRadianceFunction) As SingleObjectFrameRelativisticRayTracer
        Dim whitePlane = New SingleMaterialSurface(Of Material2D(Of RadianceSpectrum))(New Plane(New Vector3D(1, 0, 0), New Vector3D(-1, 0, 0)),
                                                                                       Materials2D(Of RadianceSpectrum).LightSource(New RadianceSpectrum(spectrum)))
        Return New SingleObjectFrameRelativisticRayTracer(New RecursiveRayTracer(Of RadianceSpectrum)(whitePlane, New LightSources(Of RadianceSpectrum)({}), {}),
                                                           observerVelocity:=New Vector3D(Constants.SpeedOfLight / 2, 0, 0),
                                                           options:=New LorentzTransformationAtSightRayOptions)
    End Function

End Class