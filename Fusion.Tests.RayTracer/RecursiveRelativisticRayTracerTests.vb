Public Class RecursiveRelativisticRayTracerTests
    <Test()>
    Public Sub Searchlight()
        Dim spectrum = Function(wavelength As Double) 1

        Assert.Greater(GetTestScene(spectrum).GetLight(New SightRay(New Ray(New Vector3D, New Vector3D(1, 0, 0)))).Function(1), spectrum(1))
    End Sub

    Private Function GetTestScene(spectrum As SpectralRadianceFunction) As RecursiveRelativisticRayTracer
        Dim whitePlane = New MaterialSurface(Of Material2D(Of RadianceSpectrum))(New Plane(New Vector3D(1, 0, 0), New Vector3D(-1, 0, 0)),
                                                                                       Materials2D(Of RadianceSpectrum).LightSource(New RadianceSpectrum(spectrum)))
        Dim frame = New RecursiveRayTracerReferenceFrame(New RecursiveRayTracer(Of RadianceSpectrum)(whitePlane, New LightSources(Of RadianceSpectrum)({}), {}), New LorentzTransformation(New Vector3D(-Constants.SpeedOfLight / 2, 0, 0)))
        Return New RecursiveRelativisticRayTracer(referenceFrames:={frame}, options:=New LorentzTransformationAtSightRayOptions)
    End Function

    <Test()>
    Public Sub Doppler()
        Const minimumWavelength = 10 ^ -7
        Dim spectrum = Function(wavelength As Double) If(wavelength > minimumWavelength, 1, 0)

        Dim transformedSpectrum = GetTestScene(spectrum).GetLight(New SightRay(New Ray(New Vector3D, New Vector3D(1, 0, 0)))).Function

        Assert.Greater(transformedSpectrum(minimumWavelength), 0)
    End Sub
End Class