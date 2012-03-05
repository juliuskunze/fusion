''' <summary>
''' A ray tracer that provides visual effects (geometry, intensity) for an observer 
''' that moves with a constant velocity (in x-direction) very close to the light velocity.
''' </summary>
''' <remarks></remarks>
Public MustInherit Class RelativisticRayTracerBase(Of TLight As {ILight(Of TLight), New})
    Implements IRayTracer(Of TLight)

    Private ReadOnly _LorentzTransformation As LorentzTransformation
    Private ReadOnly _ClassicRayTracer As IRayTracer(Of TLight)

    Public Sub New(classicRayTracer As IRayTracer(Of TLight),
                    observerVelocity As Vector3D)
        _ClassicRayTracer = classicRayTracer
        _LorentzTransformation = New LorentzTransformation(relativeVelocity:=observerVelocity)
    End Sub

    Protected ReadOnly Property ClassicRayTracer As IRayTracer(Of TLight)
        Get
            Return _ClassicRayTracer
        End Get
    End Property

    Protected ReadOnly Property LorentzTransformation As LorentzTransformation
        Get
            Return _LorentzTransformation
        End Get
    End Property

    Public MustOverride Function GetLight(viewRay As Ray) As TLight Implements IRayTracer(Of TLight).GetLight

End Class
