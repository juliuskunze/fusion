Public Class ViewCourseVideo
    Implements IRayTraceVideo

    Public Sub New(videoSize As Size, rayTracer As IRayTracer(Of RgbLight), cameraViewCourse As IViewCourse)
        _VideoSize = videoSize
        _RayTracer = rayTracer
        _CameraViewCourse = cameraViewCourse
    End Sub

    Private _CameraViewCourse As IViewCourse
    Public ReadOnly Property CameraViewCourse As IViewCourse
        Get
            Return _CameraViewCourse
        End Get
    End Property

    Private _RayTracer As IRayTracer(Of RgbLight)
    Public ReadOnly Property RayTracer As IRayTracer(Of RgbLight)
        Get
            Return _RayTracer
        End Get
    End Property

    Private _VideoSize As Size
    Public ReadOnly Property VideoSize As Size
        Get
            Return _VideoSize
        End Get
    End Property

    Public Function GetRayTracerDrawer(pointOfTime As Double) As RayTracerPicture(Of RgbLight) Implements IRayTraceVideo.GetRayTracerDrawer
        Return New RayTracerPicture(Of RgbLight)(Me.RayTracer, Me.VideoSize, Me.CameraViewCourse.GetView(pointOfTime:=pointOfTime), lightToRgbColorConverter:=New RgbLightToRgbColorConverter)
    End Function

    Public Sub CreateVideo(directoryPath As String, timeIntervalStart As Double, timeIntervalEnd As Double, timeStep As Double)
        Dim imageIndex As Integer = 0
        Dim maxIndex As Integer = CInt(Floor((timeIntervalEnd - timeIntervalStart) / timeStep))
        Dim formatString = New String("0"c, maxIndex.ToString.Length)

        For time = timeIntervalStart To timeIntervalEnd Step timeStep
            Dim drawer = Me.GetRayTracerDrawer(time)

            drawer.GetPicture.Save(directoryPath & "\image" & imageIndex.ToString(formatString) & ".png")
            imageIndex += 1
        Next
    End Sub
End Class
