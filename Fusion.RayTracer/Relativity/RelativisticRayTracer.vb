''' <summary>
''' A ray tracer that provides visual effects (geometry, intensity) for an observer 
''' that moves with a constant velocity (in x-direction) very close to the light velocity.
''' </summary>
''' <remarks></remarks>
Public Class RelativisticRayTracer(Of TLight As {ILight(Of TLight), New})
    Inherits RecursiveRayTracer(Of TLight)

    Public Sub New(ByVal surface As ISurface(Of Material2D(Of TLight)),
                   ByVal unshadedLightSource As ILightSource(Of TLight),
                   ByVal shadedPointLightSources As List(Of IPointLightSource(Of TLight)),
                   ByVal cameraVelocity As Vector3D,
                   Optional ByVal maxIntersectionCount As Integer = 10)
        MyBase.New(surface:=surface, unshadedLightSource:=unshadedLightSource, shadedPointLightSources:=shadedPointLightSources, maxIntersectionCount:=maxIntersectionCount)

        _RayTransformation = New RelativisticRadianceTransformation(relativeVelocity:=cameraVelocity)
    End Sub

    Private _RayTransformation As RelativisticRadianceTransformation

    Public Overrides Function GetLight(ByVal startRay As Ray) As TLight
        Dim transformedRay = _RayTransformation.GetTransformedRay(ray:=startRay)

        Dim transformedIntensity = _RayTransformation.GetTransformedSpectralRadiance(ray:=startRay, intensity:=1)

        Return MyBase.GetLight(transformedRay).MultiplyBrightness(transformedIntensity)
    End Function

End Class
