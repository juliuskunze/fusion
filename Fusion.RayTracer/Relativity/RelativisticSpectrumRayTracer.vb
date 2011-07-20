Public Class RelativisticRadianceSpectrumRayTracer
    Inherits RelativisticRayTracer(Of FunctionRadianceSpectrum)

    Public Sub New(ByVal surface As ISurface(Of Material2D(Of FunctionRadianceSpectrum)),
                   ByVal unshadedLightSource As ILightSource(Of FunctionRadianceSpectrum),
                   ByVal shadedPointLightSources As List(Of IPointLightSource(Of FunctionRadianceSpectrum)),
                   ByVal cameraVelocity As Vector3D,
                   Optional ByVal maxIntersectionCount As Integer = 10)
        MyBase.New(surface:=surface,
                   unshadedLightSource:=unshadedLightSource,
                   shadedPointLightSources:=shadedPointLightSources,
                   maxIntersectionCount:=maxIntersectionCount,
                   cameraVelocity:=cameraVelocity)
    End Sub

    Public Overrides Function GetLight(ByVal startRay As Ray) As FunctionRadianceSpectrum
        Dim transformedIntensityFunction = _RayTransformation.GetTransformedSpectralRadianceFunction(Ray:=startRay, SpectralRadianceFunction:=MyBase.GetLight(startRay:=startRay).IntensityFunction)

        Return New FunctionRadianceSpectrum(SpectralRadianceFunction:=transformedIntensityFunction)
    End Function

End Class
