''' <summary>
''' A ray tracer that provides visual effects (geometry, intensity) for an observer 
''' that moves with a constant velocity (in x-direction) very close to the light velocity.
''' </summary>
''' <remarks></remarks>
Public MustInherit Class RelativisticRayTracerBase(Of TLight As {ILight(Of TLight), New})
    Implements IRayTracer(Of TLight)

    Protected ReadOnly _RayTransformation As RelativisticRadianceTransformation
    Protected ReadOnly _ClassicRayTracer As IRayTracer(Of TLight)

    Public Sub New(ByVal classicRayTracer As IRayTracer(Of TLight),
                   ByVal observerVelocity As Vector3D)
        _ClassicRayTracer = classicRayTracer
        _RayTransformation = New RelativisticRadianceTransformation(relativeVelocityOfTInS:=observerVelocity)
    End Sub

    Public MustOverride Function GetLight(ByVal viewRay As Math.Ray) As TLight Implements IRayTracer(Of TLight).GetLight

End Class
