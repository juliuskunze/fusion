''' <summary>
''' A ray tracer that provides visual effects (geometry, intensity) for an observer 
''' that moves with a constant velocity (in x-direction) very close to the light velocity.
''' </summary>
''' <remarks></remarks>
Public MustInherit Class RelativisticRayTracerBase(Of TLight As {ILight(Of TLight), New})
    Implements IRayTracer(Of TLight)

    Protected ReadOnly _RayTransformation As LorentzTransformation
    Protected ReadOnly _ClassicRayTracer As IRayTracer(Of TLight)

    Public Sub New(classicRayTracer As IRayTracer(Of TLight),
                    observerVelocity As Vector3D)
        _ClassicRayTracer = classicRayTracer
        _RayTransformation = New LorentzTransformation(relativeVelocity:=observerVelocity)
    End Sub

    Public MustOverride Function GetLight(viewRay As Math.Ray) As TLight Implements IRayTracer(Of TLight).GetLight

End Class
