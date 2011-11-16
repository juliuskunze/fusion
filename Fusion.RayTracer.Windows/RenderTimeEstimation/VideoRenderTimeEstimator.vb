Public Class VideoRenderTimeEstimator
    Implements IRenderTimeEstimator

    Private ReadOnly _Video As RayTracerVideo(Of RadianceSpectrum)
    Private ReadOnly _Options As RenderTimeEstimationOptions

    Public Sub New(video As RayTracerVideo(Of RadianceSpectrum), options As RenderTimeEstimationOptions)
        _Video = video
        _Options = options
    End Sub

    Public Function Run() As RenderTimeEstimationResult Implements IRenderTimeEstimator.Run

    End Function

End Class
