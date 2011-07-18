Public Class RelativisticSpectrumRayTracer
    Inherits RelativisticRayTracer(Of RadianceSpectrum)

    Public Sub New(ByVal surface As ISurface(Of Material2D(Of RadianceSpectrum)),
                   ByVal unshadedLightSource As ILightSource(Of RadianceSpectrum),
                   ByVal shadedPointLightSources As List(Of IPointLightSource(Of RadianceSpectrum)),
                   ByVal cameraVelocity As Vector3D,
                   Optional ByVal maxIntersectionCount As Integer = 10)
        MyBase.New(surface:=surface,
                   unshadedLightSource:=unshadedLightSource,
                   shadedPointLightSources:=shadedPointLightSources,
                   maxIntersectionCount:=maxIntersectionCount,
                   cameraVelocity:=cameraVelocity)
    End Sub

    Private _RayTransformation As RelativisticRadianceTransformation

    Public Overrides Function GetLight(ByVal startRay As Ray) As RadianceSpectrum
        Dim transformedIntensityFunction = _RayTransformation.GetTransformedSpectralRadianceFunction(ray:=startRay, spectralRadianceFunction:=MyBase.GetLight(startRay:=startRay).IntensityFunction)

        Return New RadianceSpectrum(spectralRadianceFunction:=transformedIntensityFunction)
    End Function

End Class
