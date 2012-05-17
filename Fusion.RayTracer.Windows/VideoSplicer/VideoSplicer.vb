Imports avilib

Public Class VideoSplicer
    Inherits VideoSplicerBase

    Public Sub New(pictureInputFileNames As IEnumerable(Of String),
                   videoOutputFileName As String,
                   framesPerSecond As Double)
        MyBase.New(pictureInputFileNames, videoOutputFileName, framesPerSecond)
    End Sub

    Public Overrides Sub Run()
        CheckInputExists()

        Dim size = GetVideoSize()

        Dim aviWriter = New AviWriter
        Dim bitmap = aviWriter.Open(fileName:=_VideoOutputFileName, frameRate:=Convert.ToUInt32(_FramesPerSecond), width:=size.Width, height:=size.Height)
        Dim canvas = System.Drawing.Graphics.FromImage(bitmap)

        For Each pictureFileName In _PictureInputFileNames
            canvas.Clear(System.Drawing.Color.Black)
            Dim image = System.Drawing.Image.FromFile(pictureFileName)
            image.RotateFlip(System.Drawing.RotateFlipType.RotateNoneFlipY)
            canvas.DrawImage(image:=image, x:=0, y:=0)
            aviWriter.AddFrame()
            image.Dispose()
        Next

        aviWriter.Close()

    End Sub
End Class
