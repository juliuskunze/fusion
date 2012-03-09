Public Class RecursiveRayTracerReferenceFrame(Of TLight As {ILight(Of TLight), New})
    Private ReadOnly _RecursiveRayTracer As RecursiveRayTracer(Of TLight)
    Private ReadOnly _ObserverToObject As LorentzTransformation

    Public Sub New(recursiveRayTracer As RecursiveRayTracer(Of TLight), observerToObject As LorentzTransformation)
        _RecursiveRayTracer = recursiveRayTracer
        _ObserverToObject = observerToObject
    End Sub

    Public ReadOnly Property ObserverToObject As LorentzTransformation
        Get
            Return _ObserverToObject
        End Get
    End Property

    Public ReadOnly Property RecursiveRayTracer As RecursiveRayTracer(Of TLight)
        Get
            Return _RecursiveRayTracer
        End Get
    End Property
End Class
