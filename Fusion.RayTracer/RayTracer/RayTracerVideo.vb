Public Class RayTracerVideo(Of TLight As {ILight(Of TLight), New})

    Private ReadOnly _PictureFunction As Func(Of Double, RayTracerPicture(Of TLight))
    Private ReadOnly _FramesPerSecond As Double
    Private ReadOnly _Duration As Double
    Private ReadOnly _StartTime As Double
    Private ReadOnly _TimeStep As Double

    Public Sub New(pictureFunction As Func(Of Double, RayTracerPicture(Of TLight)),
                   framesPerSecond As Double,
                   duration As Double,
                   startTime As Double,
                   timeStep As Double)
        _PictureFunction = pictureFunction
        _FramesPerSecond = framesPerSecond
        _Duration = duration
        _StartTime = startTime
        _TimeStep = timeStep
    End Sub

    Public ReadOnly Property FrameCount As Integer
        Get
            Return CInt(System.Math.Floor(_Duration * _FramesPerSecond + 1))
        End Get
    End Property

    Public Function GetPicture(index As Integer) As RayTracerPicture(Of TLight)
        If index < 0 OrElse index >= Me.FrameCount Then Throw New ArgumentOutOfRangeException("index")

        Dim time = _StartTime + index * _TimeStep

        Return _PictureFunction(time)
    End Function

End Class
