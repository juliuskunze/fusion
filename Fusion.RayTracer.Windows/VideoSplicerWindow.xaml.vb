Public Class VideoSplicerWindow
    
    Private Sub RenderButton_Click(sender As System.Object, e As RoutedEventArgs) Handles _RenderButton.Click
        Try
            Dim inputFile = New FileInfo(_InputFileTextBox.Text)

            Dim fileCount = inputFile.Directory.GetFiles.Count

            Dim inputFiles = From index In Enumerable.Range(0, fileCount) Select IO.Path.Combine(inputFile.Directory.FullName, "picture" & index & inputFile.Extension)

            Dim videoSplicer = New VideoSplicer(inputFiles, _OutputFileTextBox.Text, framesPerSecond:=CInt(_FramesPerSecondTextBox.Text))

            videoSplicer.Run()
        Catch ex As IOException
            MessageBox.Show("An error occured. (Invalid input or output file etc.)")
        End Try
    End Sub
End Class
