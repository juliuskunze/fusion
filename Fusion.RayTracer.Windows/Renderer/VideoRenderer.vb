Public Class VideoRenderer
    Inherits Renderer(Of Object)

    Private ReadOnly _Video As RayTracerVideo(Of RadianceSpectrum)
    Private ReadOnly _OutputFile As FileInfo
    Private ReadOnly _DefinitionText As String
    Private ReadOnly _PictureOutputDirectory As DirectoryInfo

    Private _DonePictureCount As Integer
    Private Sub IncreaseProgressBy1FrameAndReport()
        Dim count As Integer

        SyncLock Me
            _DonePictureCount += 1

            count = _DonePictureCount
        End SyncLock


        ReportProgress(relativeProgress:=count / _Video.FrameCount)
    End Sub

    Public ReadOnly Property PictureOutputDirectory() As DirectoryInfo
        Get
            Return _PictureOutputDirectory
        End Get
    End Property

    Private ReadOnly _OutputFileNameWithoutExtension As String

    Public Sub New(video As RayTracerVideo(Of RadianceSpectrum), outputFile As FileInfo, definitionText As String)
        _Video = video
        _OutputFile = outputFile
        _DefinitionText = definitionText

        _OutputFileNameWithoutExtension = _OutputFile.Name.Substring(startIndex:=0, length:=_OutputFile.Name.Length - _OutputFile.Extension.Length)
        _PictureOutputDirectory = Directory.CreateDirectory(_OutputFile.DirectoryName & "\" & _OutputFileNameWithoutExtension)
        _FrameFiles = Enumerable.Range(0, _Video.FrameCount).Select(Function(index) GetFrameFile(index))
    End Sub

    Private Sub BackgroundWorker_DoWork(sender As Object, e As ComponentModel.DoWorkEventArgs) Handles _BackgroundWorker.DoWork
        _DonePictureCount = 0

        File.WriteAllText(IO.Path.Combine(PictureOutputDirectory.FullName, "definition.vid"), _DefinitionText)
        
        Enumerable.Range(0, _Video.FrameCount).AsParallel.ForAll(
            Sub(index)
                    RenderFrame(index:=index, e:=e)
                End Sub)

        If _BackgroundWorker.CancellationPending Then
            e.Cancel = True
            Return
        End If

        Dim videoSplicer = New VideoSplicer(pictureInputFileNames:=_FrameFiles.Select(Function(file) file.FullName),
                                            videoOutputFileName:=_OutputFile.FullName,
                                            framesPerSecond:=_Video.FramesPerSecond)
        videoSplicer.Run()

        e.Result = Nothing
    End Sub

    Private Sub RenderFrame(index As Integer,
                            e As ComponentModel.DoWorkEventArgs)
        If _BackgroundWorker.CancellationPending Then
            e.Cancel = True
            Return
        End If

        Dim frame = _Video.GetFrame(index:=index)
        Dim frameRenderer = New FrameRenderer(frame:=frame, outputFile:=GetFrameFile(index:=index))
        frameRenderer.Run()

        IncreaseProgressBy1FrameAndReport()
    End Sub

    Private ReadOnly _FrameFiles As IEnumerable(Of FileInfo)

    Private Function GetFrameFile(index As Integer) As FileInfo
        Return New FileInfo(_PictureOutputDirectory.FullName & String.Format("\picture{0}.png", index))
    End Function
End Class
