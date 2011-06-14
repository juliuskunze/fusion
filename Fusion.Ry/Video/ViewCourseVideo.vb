Public Class ViewCourseVideo
    Implements IRayTraceVideo

    Public Sub New(ByVal videoSize As Size, ByVal rayTracer As IRayTracer, ByVal cameraViewCourse As IViewCourse)
        _VideoSize = videoSize
        _RayTracer = rayTracer
        _cameraViewCourse = cameraViewCourse
    End Sub

    Private _cameraViewCourse As IViewCourse
    Public ReadOnly Property CameraViewCourse As IViewCourse
        Get
            Return _cameraViewCourse
        End Get
    End Property

    Private _RayTracer As IRayTracer
    Public ReadOnly Property RayTracer As IRayTracer
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

    Public Function GetRayTracerDrawer(ByVal pointOfTime As Double) As RayTraceDrawer Implements IRayTraceVideo.GetRayTracerDrawer
        Return New RayTraceDrawer(Me.RayTracer, Me.VideoSize, Me.CameraViewCourse.GetView(pointOfTime:=pointOfTime))
    End Function

    Public Sub CreateVideo(ByVal directoryPath As String, ByVal timeIntervalStart As Double, ByVal timeIntervalEnd As Double, ByVal timeStep As Double)
        Dim imageIndex As Integer = 0
        Dim maxIndex As Integer = CInt(Floor((timeIntervalEnd - timeIntervalStart) / timeStep))
        Dim formatString = New String("0"c, maxIndex.ToString.Length)

        For time = timeIntervalStart To timeIntervalEnd Step timeStep
            Dim drawer = Me.GetRayTracerDrawer(time)

            drawer.Picture.Save(directoryPath & "\image" & imageIndex.ToString(formatString) & ".png")
            imageIndex += 1
        Next
    End Sub
End Class
