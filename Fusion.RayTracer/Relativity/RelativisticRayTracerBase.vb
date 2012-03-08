''' <summary>
''' A ray tracer that provides visual effects (geometry, intensity) for an observer 
''' that moves with a constant velocity (in x-direction) very close to the light velocity.
''' </summary>
''' <remarks></remarks>
Public MustInherit Class SingleReferenceFrameRelativisticRayTracerBase(Of TLight As {ILight(Of TLight), New})
    Implements IRayTracer(Of TLight)

    Private ReadOnly _ObjectToObserver As LorentzTransformation
    Private ReadOnly _ClassicRayTracer As IRayTracer(Of TLight)

    Public Sub New(classicRayTracer As IRayTracer(Of TLight),
                   observerVelocity As Vector3D)
        _ClassicRayTracer = classicRayTracer
        _ObjectToObserver = New LorentzTransformation(relativeVelocity:=observerVelocity)
    End Sub

    Protected ReadOnly Property ClassicRayTracer As IRayTracer(Of TLight)
        Get
            Return _ClassicRayTracer
        End Get
    End Property

    ''' <summary>
    ''' The Lorentz transformation from the object frame to the observer reference frame.
    ''' </summary>
    Protected ReadOnly Property ObjectToObserver As LorentzTransformation
        Get
            Return _ObjectToObserver
        End Get
    End Property

    Public MustOverride Function GetLight(observerSightRayWithObjectOrigin As Ray) As TLight Implements IRayTracer(Of TLight).GetLight

End Class
