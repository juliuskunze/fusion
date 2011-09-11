Imports Splicer
Imports Splicer.Renderer
Imports Splicer.Timeline

Public Class VideoRenderer

    Public Sub New(framesPerSecond As Double, width As Integer, height As Integer)
        Dim timeline = New DefaultTimeline(framesPerSecond)
        Dim videoGroup = timeline.AddVideoGroup("main", framesPerSecond, 24, 1000, 1000)
        Dim videoTrack = videoGroup.AddTrack()


        For i = 0 To 29
            Dim fileName = String.Format("B:\Julius-Ordner\Zeitlos\Programmierung\Fusion.Ry-Bilder\25 g vid2\new.bmp", i.ToString("00"))
            videoTrack.AddImage(fileName, 0, 1 / framesPerSecond)

            Dim fileName2 = String.Format("B:\Julius-Ordner\Zeitlos\Programmierung\Fusion.Ry-Bilder\25 g vid2\new2.bmp", i.ToString("00"))
            videoTrack.AddImage(fileName2, 0, 1 / framesPerSecond)
        Next
        System.IO.Path.GetTempPath()
        Using renderer = New Renderer.AviFileRenderer(timeline, outputFile:="B:\Julius-Ordner\Zeitlos\Programmierung\Fusion.Ry-Bilder\25 g vid2\vid.avi")
            renderer.Render()
        End Using
    End Sub

End Class
