Imports System.Windows.Forms

Public Class VideoSplicerWindow
    Private ReadOnly _InputFileDialog As New OpenFileDialog(Me, New FileFilters({}), New DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)))
    Private ReadOnly _OutputFileDialog As New SaveFileDialog(Me, New DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)), ".avi", initialFileName:="video.avi")

    Private Sub RenderButton_Click(sender As System.Object, e As RoutedEventArgs) Handles _RenderButton.Click
        Try
            Dim inputFile = New FileInfo(_OneInputFileTextBox.Text)
            Const picture = "picture"
            Const extensionLength = 4
            Dim inputFiles = inputFile.Directory.GetFiles.Where(Function(x) x.Name.StartsWith(picture) AndAlso (x.Extension = ".png" OrElse x.Extension = ".jpg")).OrderBy(Function(x) CInt(x.Name.Substring(picture.Length, x.Name.Length - picture.Length - extensionLength))).ToArray
            Dim outputFile = New FileInfo(_OutputFileTextBox.Text)
            Dim videoSplicer = New VideoSplicer(inputFiles, outputFile, framesPerSecond:=CInt(_FramesPerSecondTextBox.Text))

            videoSplicer.Run()
        Catch ex As IOException
            System.Windows.MessageBox.Show("An error occured. (Invalid input or output file etc.)")
        End Try
    End Sub

    Private Sub _BrowseInputDirectoryButton_Click(sender As Object, e As RoutedEventArgs) Handles _BrowseInputDirectoryButton.Click
        If _InputFileDialog.Show Then
            _OneInputFileTextBox.Text = _InputFileDialog.File.FullName
            SetInitialOutputPath()
        End If
    End Sub

    Private Sub SetInitialOutputPath()
        _OutputFileDialog.InitialDirectory = _InputFileDialog.File.Directory.Parent
        Dim initialOutputFile = New FileInfo(IO.Path.Combine(_InputFileDialog.File.Directory.FullName, _InputFileDialog.File.Directory.Name & ".avi"))
        _OutputFileDialog.File = initialOutputFile

        _OutputFileTextBox.Text = initialOutputFile.FullName
    End Sub

    Private Sub _SaveOutputFileButton_Click(sender As Object, e As RoutedEventArgs) Handles _SaveOutputFileButton.Click
        If _OutputFileDialog.Show Then
            _OutputFileTextBox.Text = _OutputFileDialog.File.FullName
        End If
    End Sub
End Class
