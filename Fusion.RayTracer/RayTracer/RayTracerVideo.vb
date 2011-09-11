Public Class RayTracerVideo(Of TLight As {ILight(Of TLight), New})

    Private ReadOnly _PictureFunction As Func(Of Double, RayTracerPicture(Of TLight))
    Private ReadOnly _FramesPerSecond As Double
    Private ReadOnly _StartTime As Double
    Private ReadOnly _EndTime As Double
    Private ReadOnly _TimeStep As Double

    Public Sub New(pictureFunction As Func(Of Double, RayTracerPicture(Of TLight)),
                   framesPerSecond As Double,
                   startTime As Double,
                   endTime As Double,
                   timeStep As Double)
        _PictureFunction = pictureFunction
        _FramesPerSecond = framesPerSecond
        _StartTime = startTime
        _EndTime = endTime
        _TimeStep = timeStep
    End Sub

    Public ReadOnly Property PictureCount As Integer
        Get
            Return CInt(System.Math.Floor((_StartTime - _EndTime) / _TimeStep))
        End Get
    End Property

    Public Function GetPicture(index As Double) As RayTracerPicture(Of TLight)

    End Function

End Class
