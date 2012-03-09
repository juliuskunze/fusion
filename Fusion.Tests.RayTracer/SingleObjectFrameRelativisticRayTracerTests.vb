Public Class SingleObjectFrameRelativisticRayTracerTests

    <Test()>
    Public Sub Searchlight()
        Dim spectrum = Function(wavelength As Double) 1
        Dim whitePlane = New SingleMaterialSurface(Of Material2D(Of RadianceSpectrum))(New Plane(New Vector3D(1, 0, 0), New Vector3D(-1, 0, 0)),
                                                                                       Materials2D(Of RadianceSpectrum).LightSource(New RadianceSpectrum(spectrum)))
        Dim r = New SingleObjectFrameRelativisticRayTracer(New RecursiveRayTracer(Of RadianceSpectrum)(whitePlane, New LightSources(Of RadianceSpectrum)({}), {}),
                                                           observerVelocity:=New Vector3D(Constants.SpeedOfLight / 2, 0, 0),
                                                           options:=New LorentzTransformationAtSightRayOptions)

        Assert.Greater(r.GetLight(observerSightRayWithObjectOrigin:=New Ray(New Vector3D, New Vector3D(1, 0, 0))).Function(1), spectrum(1))
    End Sub

    <Test()>
    Public Sub Doppler()
        Dim minimumWavelength = 10 ^ -7

        Dim spectrum = Function(wavelength As Double) If(wavelength > minimumWavelength, 1, 0)
        Dim whitePlane = New SingleMaterialSurface(Of Material2D(Of RadianceSpectrum))(New Plane(New Vector3D(1, 0, 0), New Vector3D(-1, 0, 0)),
                                                                                       Materials2D(Of RadianceSpectrum).LightSource(New RadianceSpectrum(spectrum)))
        Dim r = New SingleObjectFrameRelativisticRayTracer(New RecursiveRayTracer(Of RadianceSpectrum)(whitePlane, New LightSources(Of RadianceSpectrum)({}), {}),
                                                           observerVelocity:=New Vector3D(Constants.SpeedOfLight / 2, 0, 0),
                                                           options:=New LorentzTransformationAtSightRayOptions)

        Dim transformedSpectrum = r.GetLight(observerSightRayWithObjectOrigin:=New Ray(New Vector3D, New Vector3D(1, 0, 0))).Function

        Assert.Greater(transformedSpectrum(minimumWavelength), 0)
    End Sub

End Class