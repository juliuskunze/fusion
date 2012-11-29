Public Class RenderTimeEstimationOptions
    Private ReadOnly _Time As Double
    Public ReadOnly Property Time As Double
        Get
            If _Mode <> RenderTimeEstimationMode.FixTime Then Throw New InvalidOperationException("Mode must be fix time.")

            Return _Time
        End Get
    End Property

    Private ReadOnly _PixelCount As Integer
    Public ReadOnly Property PixelCount As Integer
        Get
            If _Mode <> RenderTimeEstimationMode.FixPixelCount Then Throw New InvalidOperationException("Mode must be fix pixel count.")

            Return _PixelCount
        End Get
    End Property

    Private ReadOnly _Mode As RenderTimeEstimationMode
    Public ReadOnly Property Mode As RenderTimeEstimationMode
        Get
            Return _Mode
        End Get
    End Property

    Public Sub New(time As Double)
        _Time = time
        _Mode = RenderTimeEstimationMode.FixTime
    End Sub

    Public Sub New(pixelCount As Integer)
        _PixelCount = pixelCount
        _Mode = RenderTimeEstimationMode.FixPixelCount
    End Sub
End Class
