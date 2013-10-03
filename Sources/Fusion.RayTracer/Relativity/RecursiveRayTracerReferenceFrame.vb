Public Class RecursiveRayTracerReferenceFrame
    Private ReadOnly _RecursiveRayTracer As RecursiveRayTracer(Of RadianceSpectrum)
    Private ReadOnly _BaseToObject As LorentzTransformation

    Public Sub New(recursiveRayTracer As RecursiveRayTracer(Of RadianceSpectrum), baseToObject As LorentzTransformation)
        _RecursiveRayTracer = recursiveRayTracer
        _BaseToObject = baseToObject
    End Sub

    Public Sub New(recursiveRayTracer As RecursiveRayTracer(Of RadianceSpectrum), objectFrameVelocityInBaseFrame As Vector3D)
        Me.New(recursiveRayTracer, baseToObject:=New LorentzTransformation(objectFrameVelocityInBaseFrame))
    End Sub

    Public ReadOnly Property BaseToObject As LorentzTransformation
        Get
            Return _BaseToObject
        End Get
    End Property

    Public ReadOnly Property RecursiveRayTracer As RecursiveRayTracer(Of RadianceSpectrum)
        Get
            Return _RecursiveRayTracer
        End Get
    End Property
End Class
