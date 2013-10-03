''' <summary>
''' Represents a picture renderer that saves the picture after rendering.
''' </summary>
''' <remarks></remarks>
Public Class FrameRenderer
    Inherits PictureRenderer

    Private ReadOnly _OutputFile As FileInfo

    Public Sub New(frame As RayTracerPicture(Of RadianceSpectrum), outputFile As FileInfo)
        MyBase.New(picture:=frame)
        _OutputFile = outputFile
    End Sub

    Private Sub FrameRenderer_Completed(e As RenderResultEventArgs(Of System.Drawing.Bitmap)) Handles Me.Completed
        Save(result:=e.Result)
    End Sub

    Private Sub Save(result As System.Drawing.Bitmap)
        result.Save(_OutputFile.FullName, format:=System.Drawing.Imaging.ImageFormat.Png)
    End Sub

    Public Overloads Sub Run()
        Save(result:=MyBase.Run)
    End Sub
End Class
