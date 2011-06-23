Public Class LensVideo
    Implements IRayTraceVideo

    Private _VideoSize As Size
    Public ReadOnly Property VideoSize As Size
        Get
            Return _VideoSize
        End Get
    End Property

    Public Sub New(ByVal videoSize As Size)
        _VideoSize = videoSize
    End Sub

    Public Function GetRayTracerDrawer(ByVal pointOfTime As Double) As RayTraceDrawer(Of RgbLight) Implements IRayTraceVideo.GetRayTracerDrawer
        Return New RayTracingExamples(Me.VideoSize).FirstRoom(glassRefractionIndex:=pointOfTime)
    End Function

    Public Sub CreateVideo(ByVal directoryPath As String, ByVal timeIntervalStart As Double, ByVal timeIntervalEnd As Double, ByVal timeStep As Double)
        Dim imageIndex As Integer = 0
        Dim maxIndex As Integer = CInt(Floor((timeIntervalEnd - timeIntervalStart) / timeStep))
        Dim formatString = New String("0"c, maxIndex.ToString.Length)

        For time = timeIntervalStart To timeIntervalEnd Step timeStep
            Me.GetRayTracerDrawer(time).Picture.Save(directoryPath & "\image" & imageIndex.ToString(formatString) & ".png", format:=Imaging.ImageFormat.Png)

            imageIndex += 1
        Next
    End Sub
End Class
