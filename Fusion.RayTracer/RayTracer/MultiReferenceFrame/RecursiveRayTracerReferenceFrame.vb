Public Class RecursiveRayTracerReferenceFrame(Of TLight As {ILight(Of TLight), New})
    Private ReadOnly _RecursiveRayTracer As RecursiveRayTracer(Of TLight)
    Private ReadOnly _Transformation As LorentzTransformation

    Public Sub New(recursiveRayTracer As RecursiveRayTracer(Of TLight), transformation As LorentzTransformation)
        _RecursiveRayTracer = recursiveRayTracer
        _Transformation = transformation
    End Sub

    Public ReadOnly Property Transformation() As LorentzTransformation
        Get
            Return _Transformation
        End Get
    End Property

    Public ReadOnly Property RecursiveRayTracer As RecursiveRayTracer(Of TLight)
        Get
            Return _RecursiveRayTracer
        End Get
    End Property
End Class
