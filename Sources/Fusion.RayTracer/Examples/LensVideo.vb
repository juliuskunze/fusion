﻿Public Class LensVideo
    Implements IRayTraceVideo

    Private ReadOnly _VideoSize As Size
    Public ReadOnly Property VideoSize As Size
        Get
            Return _VideoSize
        End Get
    End Property

    Public Sub New(videoSize As Size)
        _VideoSize = videoSize
    End Sub

    Public Function GetRayTracerDrawer(pointOfTime As Double) As RayTracerPicture(Of RgbLight) Implements IRayTraceVideo.GetRayTracerDrawer
        Return New RayTracingExamples(Me.VideoSize).OldExampleBox(glassRefractionIndex:=pointOfTime)
    End Function

    Public Sub CreateVideo(directoryPath As String, timeIntervalStart As Double, timeIntervalEnd As Double, timeStep As Double)
        Dim imageIndex As Integer = 0
        Dim maxIndex As Integer = CInt(Floor((timeIntervalEnd - timeIntervalStart) / timeStep))
        Dim formatString = New String("0"c, maxIndex.ToString.Length)

        For time = timeIntervalStart To timeIntervalEnd Step timeStep
            Me.GetRayTracerDrawer(time).GetPicture.Save(directoryPath & "\image" & imageIndex.ToString(formatString) & ".png", format:=Imaging.ImageFormat.Png)

            imageIndex += 1
        Next
    End Sub
End Class
