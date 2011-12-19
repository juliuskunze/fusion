Imports Splicer
Imports Splicer.Renderer
Imports Splicer.Timeline

''' <summary>
''' Fails for big amounts of frames and needs much disk space.
''' </summary>
''' <remarks></remarks>
Public Class SimpleVideoSplicer
    Inherits VideoSplicerBase

    Public Sub New(pictureInputFileNames As IEnumerable(Of String),
                   videoOutputFileName As String,
                   framesPerSecond As Double)
        MyBase.New(pictureInputFileNames, videoOutputFileName, framesPerSecond)
    End Sub

    Public Overrides Sub Run()
        Me.CheckInputExists()

        Dim size = Me.GetVideoSize

        Using timeline = New DefaultTimeline(fps:=_FramesPerSecond)
            Using videoGroup = timeline.AddVideoGroup("main", fps:=_FramesPerSecond, bitCount:=24, width:=size.Width, height:=size.Height)
                Using videoTrack = videoGroup.AddTrack
                    For Each pictureFileName In _PictureInputFileNames
                        videoTrack.AddImage(image:=System.Drawing.Image.FromFile(pictureFileName), offset:=0, clipEnd:=1 / _FramesPerSecond)
                    Next

                    Using renderer = New Splicer.Renderer.AviFileRenderer(timeline:=timeline, outputFile:=_VideoOutputFileName)
                        renderer.Render()
                    End Using
                End Using
            End Using
        End Using
    End Sub

End Class
