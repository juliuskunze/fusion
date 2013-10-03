Imports avilib

Public Class VideoSplicer
    Private ReadOnly _PictureInputFiles As IEnumerable(Of FileInfo)
    Private ReadOnly _VideoOutputFile As FileInfo
    Private ReadOnly _FramesPerSecond As Double

    Public Sub New(pictureInputFiles As IEnumerable(Of FileInfo),
                   videoOutputFile As FileInfo,
                   framesPerSecond As Double)
        _PictureInputFiles = PictureInputFiles
        _VideoOutputFile = videoOutputFile
        _FramesPerSecond = framesPerSecond
    End Sub

    Private Function GetVideoSize() As System.Drawing.Size
        Return System.Drawing.Image.FromFile(_PictureInputFiles.First.FullName).Size
    End Function

    Private Sub CheckInputExists()
        _PictureInputFiles.All(Function(inputFileName) inputFileName.Exists)
    End Sub

    Public Sub Run()
        CheckInputExists()

        Dim size = GetVideoSize()

        Dim aviWriter = New AviWriter
        Dim bitmap = aviWriter.Open(fileName:=_VideoOutputFile.FullName, frameRate:=Convert.ToUInt32(_FramesPerSecond), width:=size.Width, height:=size.Height)
        Dim canvas = System.Drawing.Graphics.FromImage(bitmap)

        For Each pictureFile In _PictureInputFiles
            canvas.Clear(System.Drawing.Color.Black)
            Dim image = System.Drawing.Image.FromFile(pictureFile.FullName)
            image.RotateFlip(System.Drawing.RotateFlipType.RotateNoneFlipY)
            canvas.DrawImage(image:=image, x:=0, y:=0)
            aviWriter.AddFrame()
            image.Dispose()
        Next

        aviWriter.Close()

    End Sub
End Class
