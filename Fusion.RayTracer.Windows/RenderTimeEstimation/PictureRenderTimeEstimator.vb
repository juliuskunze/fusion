Public Class PictureRenderTimeEstimator
    Implements IRenderTimeEstimator

    Private ReadOnly _Picture As RayTracerPicture(Of RadianceSpectrum)
    Private ReadOnly _Options As RenderTimeEstimationOptions

    Public Sub New(picture As RayTracerPicture(Of RadianceSpectrum), options As RenderTimeEstimationOptions)
        _Picture = picture
        _Options = options
    End Sub

    Public Function Run() As RenderTimeEstimationResult Implements IRenderTimeEstimator.Run
        Dim size = _Picture.PictureSize

        Dim random = New Random

        Dim drawTimeStopwatch = New Stopwatch
        Dim testedPixelCount = 0

        Dim stopwatch = New Stopwatch
        stopwatch.Start()
        Do While If(_Options.Mode = RenderTimeEstimationMode.FixTime,
                    stopwatch.ElapsedMilliseconds / 1000 < _Options.Time,
                    testedPixelCount < _Options.PixelCount)
            Dim randomX = random.Next(size.Width)
            Dim randomY = random.Next(size.Height)

            drawTimeStopwatch.Start()

            _Picture.GetPixelColor(randomX, randomY)

            drawTimeStopwatch.Stop()

            testedPixelCount += 1
        Loop

        stopwatch.Stop()

        'experiment --> 
        Const factor = 3.4
        Dim ticksPerPixel = drawTimeStopwatch.ElapsedTicks / testedPixelCount * factor

        Dim timePerPixel = New TimeSpan(CLng(ticksPerPixel))

        Dim totalPixelCount = size.Width * size.Height
        Dim totalTime = New TimeSpan(ticks:=CLng(ticksPerPixel * totalPixelCount))

        Return New RenderTimeEstimationResult(totalTime:=totalTime, timePerPixel:=timePerPixel)
    End Function

End Class
