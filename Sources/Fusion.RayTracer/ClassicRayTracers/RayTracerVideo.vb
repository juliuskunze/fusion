Public Class RayTracerVideo(Of TLight As {ILight(Of TLight), New})

    Private ReadOnly _PictureFunction As Func(Of Double, RayTracerPicture(Of TLight))
    Public ReadOnly Property PictureFunction As Func(Of Double, RayTracerPicture(Of TLight))
        Get
            Return _PictureFunction
        End Get
    End Property

    Private ReadOnly _FramesPerSecond As Double
    Public ReadOnly Property FramesPerSecond As Double
        Get
            Return _FramesPerSecond
        End Get
    End Property

    Private ReadOnly _Duration As Double
    Public ReadOnly Property Duration As Double
        Get
            Return _Duration
        End Get
    End Property

    Private ReadOnly _StartTime As Double
    Public ReadOnly Property StartTime As Double
        Get
            Return _StartTime
        End Get
    End Property

    Private ReadOnly _TimeStep As Double
    Public ReadOnly Property TimeStep As Double
        Get
            Return _TimeStep
        End Get
    End Property

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

    Public Shared Function FromStartAndEndTime(
            pictureFunction As Func(Of Double, RayTracerPicture(Of TLight)),
            framesPerSecond As Double,
            duration As Double,
            startTime As Double,
            endTime As Double) As RayTracerVideo(Of TLight)
        Return New RayTracerVideo(Of TLight)(pictureFunction:=pictureFunction,
                                             framesPerSecond:=framesPerSecond,
                                             duration:=duration,
                                             startTime:=startTime,
                                             timeStep:=(endTime - startTime) / GetFrameCount(duration:=duration, framesPerSecond:=framesPerSecond))
    End Function

    Public ReadOnly Property FrameCount As Integer
        Get
            Return GetFrameCount(duration:=_Duration, framesPerSecond:=_FramesPerSecond)
        End Get
    End Property

    Private Shared Function GetFrameCount(duration As Double, framesPerSecond As Double) As Integer
        Return CInt(Floor(duration * framesPerSecond + 1))
    End Function

    Public Function GetFrame(index As Integer) As RayTracerPicture(Of TLight)
        If index < 0 OrElse index >= FrameCount Then Throw New ArgumentOutOfRangeException("index")

        Dim time = _StartTime + index * _TimeStep

        Return _PictureFunction(time)
    End Function
End Class
