Imports Splicer
Imports Splicer.Renderer
Imports Splicer.Timeline

Public Class VideoSlicer
    Private Sub New()
    End Sub

    Public Shared Sub Run(pictureInputFileNames As IEnumerable(Of String),
                          videoOutputFileName As String,
                          framesPerSecond As Double)

        Using timeline = New DefaultTimeline(fps:=framesPerSecond)
            Using videoGroup = timeline.AddVideoGroup("main", fps:=framesPerSecond, bitCount:=24, width:=1000, height:=1000)
                Using videoTrack = videoGroup.AddTrack
                    For Each pictureFileName In pictureInputFileNames
                        videoTrack.AddImage(fileName:=pictureFileName, offset:=0, clipEnd:=1 / framesPerSecond)
                    Next

                    Using renderer = New Splicer.Renderer.AviFileRenderer(timeline:=timeline, outputFile:=videoOutputFileName)
                        renderer.Render()
                    End Using
                End Using
            End Using
        End Using
    End Sub

End Class
