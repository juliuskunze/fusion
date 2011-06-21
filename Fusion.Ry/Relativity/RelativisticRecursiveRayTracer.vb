''' <summary>
''' A ray tracer that provides the geometry effects of a camera 
''' that moves with a velocity (in x-direction) very close to the light velocity.
''' </summary>
''' <remarks></remarks>
Public Class RelativisticGeometryRayTracer
    Inherits RecursiveRayTracer

    Public Sub New(ByVal surface As ISurface(Of Material2D(Of ExactColor)),
                   ByVal unshadedLightSource As ILightSource(Of ExactColor),
                   ByVal shadedPointLightSources As List(Of IPointLightSource(Of ExactColor)),
                   ByVal xCameraVelocityInC As Double,
                   Optional ByVal maxIntersectionCount As Integer = 10)
        MyBase.New(surface:=surface, lightSource:=unshadedLightSource, shadedPointLightSources:=shadedPointLightSources, maxIntersectionCount:=maxIntersectionCount)

        _RayTransformation = New RelativisticRayTransformation(relativeXVelocityInC:=xCameraVelocityInC)
    End Sub

    Private _RayTransformation As RelativisticRayTransformation

    Public Overrides Function GetColor(ByVal startRay As Ray) As ExactColor
        Dim transformedRay = _RayTransformation.TransformedRay(ray:=startRay)

        Dim transformedIntensity = _RayTransformation.TransformedIntensity(ray:=startRay, intensity:=1)

        Return transformedIntensity * MyBase.GetColor(transformedRay)
    End Function

End Class
