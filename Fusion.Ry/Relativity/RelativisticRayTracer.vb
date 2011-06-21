Public Class RelativisticRayTracer
    Inherits RecursiveRayTracer(Of RgbLight)

    Public Sub New(ByVal surface As ISurface(Of Material2D(Of RgbLight)),
                   ByVal lightSource As ILightSource(Of RgbLight),
                   ByVal shadedPointLightSources As List(Of IPointLightSource(Of RgbLight)),
                   ByVal xCameraVelocityInC As Double,
                   Optional ByVal maxIntersectionCount As Integer = 10)
        MyBase.New(surface:=surface, lightSource:=lightSource, shadedPointLightSources:=shadedPointLightSources, maxIntersectionCount:=maxIntersectionCount)

        _RayTransformation = New RelativisticRayTransformation(relativeXVelocityInC:=xCameraVelocityInC)
    End Sub

    Private _RayTransformation As RelativisticRayTransformation

    Public Overrides Function GetColor(ByVal startRay As Ray) As RgbLight
        Dim transformedRay = _RayTransformation.TransformedRay(ray:=startRay)

        Dim transformedIntensity = _RayTransformation.TransformedIntensity(ray:=startRay, intensity:=1)

        Return transformedIntensity * MyBase.GetColor(transformedRay)
    End Function

End Class
