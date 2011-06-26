Public Class RelativisticSpectrumRayTracer
    Inherits RelativisticRayTracer(Of FunctionLightSpectrum)

    Public Sub New(ByVal surface As ISurface(Of Material2D(Of FunctionLightSpectrum)),
                   ByVal unshadedLightSource As ILightSource(Of FunctionLightSpectrum),
                   ByVal shadedPointLightSources As List(Of IPointLightSource(Of FunctionLightSpectrum)),
                   ByVal xCameraVelocityInC As Double,
                   Optional ByVal maxIntersectionCount As Integer = 10)
        MyBase.New(surface:=surface,
                   unshadedLightSource:=unshadedLightSource,
                   shadedPointLightSources:=shadedPointLightSources,
                   maxIntersectionCount:=maxIntersectionCount,
                   xCameraVelocityInC:=xCameraVelocityInC)
    End Sub

    Private _RayTransformation As RelativisticRayTransformation

    Public Overrides Function GetLight(ByVal startRay As Ray) As FunctionLightSpectrum
        Dim transformedIntensityFunction = _RayTransformation.GetTransformedIntensityFunction(ray:=startRay, intensityFunction:=MyBase.GetLight(startRay:=startRay).IntensityFunction)

        Return New FunctionLightSpectrum(IntensityFunction:=transformedIntensityFunction)
    End Function

End Class
