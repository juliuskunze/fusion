Public Class VideoRenderTimeEstimator
    Implements IRenderTimeEstimator

    Private ReadOnly _Video As RayTracerVideo(Of RadianceSpectrum)
    Private ReadOnly _Options As RenderTimeEstimationOptions

    Public Sub New(video As RayTracerVideo(Of RadianceSpectrum), options As RenderTimeEstimationOptions)
        _Video = video
        _Options = options
    End Sub

    Public Function Run() As RenderTimeEstimationResult Implements IRenderTimeEstimator.Run
        If _Video.FrameCount = 0 Then Return New RenderTimeEstimationResult(TimeSpan.Zero, TimeSpan.Zero)

        Dim firstFrame = _Video.GetFrame(0)
        Dim estimator = New PictureRenderTimeEstimator(firstFrame, options:=_Options)
        Dim result = estimator.Run

        Return New RenderTimeEstimationResult(totalTime:=TimeSpan.FromTicks(result.TotalTime.Ticks * _Video.FrameCount), timePerPixel:=result.TimePerPixel)
    End Function

End Class
