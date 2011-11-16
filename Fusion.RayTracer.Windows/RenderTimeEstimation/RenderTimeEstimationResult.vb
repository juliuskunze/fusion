Public Class RenderTimeEstimationResult

    Private ReadOnly _TotalTime As TimeSpan
    Public ReadOnly Property TotalTime As TimeSpan
        Get
            Return _TotalTime
        End Get
    End Property

    Private ReadOnly _TimePerPixel As TimeSpan
    Public ReadOnly Property TimePerPixel As TimeSpan
        Get
            Return _TimePerPixel
        End Get
    End Property

    Public Sub New(totalTime As TimeSpan, timePerPixel As TimeSpan)
        _TotalTime = totalTime
        _TimePerPixel = timePerPixel
    End Sub

End Class
