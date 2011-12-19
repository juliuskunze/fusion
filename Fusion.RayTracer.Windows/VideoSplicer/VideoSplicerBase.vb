Public MustInherit Class VideoSplicerBase

    Protected ReadOnly _PictureInputFileNames As IEnumerable(Of String)
    Protected ReadOnly _VideoOutputFileName As String
    Protected ReadOnly _FramesPerSecond As Double

    Public Sub New(pictureInputFileNames As IEnumerable(Of String),
                   videoOutputFileName As String,
                   framesPerSecond As Double)
        _PictureInputFileNames = pictureInputFileNames
        _VideoOutputFileName = videoOutputFileName
        _FramesPerSecond = framesPerSecond
    End Sub

    Protected Function GetVideoSize() As System.Drawing.Size
        Return System.Drawing.Image.FromFile(_PictureInputFileNames.First).Size
    End Function

    Protected Sub CheckInputExists()
        _PictureInputFileNames.All(Function(inputFileName) IO.File.Exists(inputFileName))
    End Sub

    Public MustOverride Sub Run()

End Class

