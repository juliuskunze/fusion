Class MainWindow

    Private WithEvents _RayTraceDrawer As RayTraceDrawer(Of RgbLight)
    Private _Picture As System.Drawing.Bitmap

    Private _CustomPictureSizeOk As Boolean = False
    Private _CustomPictureSize As System.Drawing.Size

    Private _SaveFileDialog As SaveFileDialog

    Public Sub New()
        Me.InitializeComponent()
    End Sub

    Private Sub RenderButton_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles _RenderButton.Click
        If Not Me.TrySetRayTracerDrawer Then Return

        Dim stopwatch = New Stopwatch
        stopwatch.Start()

        _Picture = _RayTraceDrawer.GetPicture

        stopwatch.Stop()
        _TotalElapsedTimeLabel.Content = "Total elapsed time: " & stopwatch.Elapsed.ToString
        _AverageElapsedTimePerPixelLabel.Content = "Average elapsed time per pixel: " & (stopwatch.ElapsedMilliseconds / (_Picture.Size.Width * _Picture.Size.Height)).ToString & "ms"

        _ResultImage.Source = New SimpleBitmap(bitmap:=_Picture).ToBitmapSource

        _SaveButton.IsEnabled = True
    End Sub

    Private Function TrySetRayTracerDrawer() As Boolean
        Dim pictureSize As System.Drawing.Size
        If Not Me.TryGetPictureSize(out_size:=pictureSize) Then Return False

        _RayTraceDrawer = New RayTracingExamples(pictureSize).SecondRoom(cameraZLocation:=29)
        Return True
    End Function

    Private Function TryGetPictureSize(ByRef out_size As System.Drawing.Size) As Boolean
        If Not _CustomPictureSizeOk Then Return False
        out_size = _CustomPictureSize
        Return True
    End Function

    Private Sub RayTraceDrawer_ProgressIncreased(ByVal sender As Object, ByVal e As ProgressEventArgs) Handles _RayTraceDrawer.ProgressIncreased
        Select Case e.Progress
            Case 1
                _RenderProgressBar.Value = 0
            Case Else
                _RenderProgressBar.Value = e.Progress
        End Select
    End Sub

    Private Sub SaveButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _SaveButton.Click
        _SaveFileDialog.FileName = "ray tracing picture "
        Dim pictureNumber As Integer = 1
        Do While New IO.FileInfo(_SaveFileDialog.InitialDirectory & "\" & _SaveFileDialog.FileName & pictureNumber).Exists
            pictureNumber += 1
        Loop
        _SaveFileDialog.FileName &= pictureNumber
        If _SaveFileDialog.ShowDialog Then
            _Picture.Save(_SaveFileDialog.FileName)
        Else
            MessageBox.Show("Saving failed.")
        End If
    End Sub

    Private Sub customSizeTextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _CustomSizeTextBox.TextChanged
        Try
            Dim sizeVector = New Vector2D(Me._CustomSizeTextBox.Text)
            _CustomPictureSize = New System.Drawing.Size(CInt(sizeVector.X), CInt(sizeVector.Y))
            _CustomSizeTextBox.Background = Brushes.White
            _CustomPictureSizeOk = True
        Catch
            _CustomSizeTextBox.Background = Brushes.Tomato
            _CustomPictureSizeOk = False
        End Try
    End Sub

    Private Sub VideoButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _VideoButton.Click
        RayTracingExamples.WriteVideo()
    End Sub

    Private Sub CalculateNeededTimeButton_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles _CalculateNeededTimeButton.Click
        If Not Me.TrySetRayTracerDrawer() Then Return
        If Not _CalculateTimeOptionsDialog.DialogResult Then Return

        Dim size = _RayTraceDrawer.PictureSize

        Dim bitmap = New System.Drawing.Bitmap(size.Width, size.Height)

        Dim random = New Random

        Dim drawTimeStopwatch = New Stopwatch
        Dim testedPixelCount = 0

        Dim stopwatch = New Stopwatch
        stopwatch.Start()
        Do While If(_CalculateTimeOptionsDialog.Mode = CalculateTimeOptionsDialog.FixMode.Time,
                    stopwatch.ElapsedMilliseconds / 1000 < _CalculateTimeOptionsDialog.FixTestTime,
                    testedPixelCount < _CalculateTimeOptionsDialog.FixTestPixelCount)
            Dim randomX = random.Next(size.Width)
            Dim randomY = random.Next(size.Height)

            drawTimeStopwatch.Start()

            bitmap.SetPixel(randomX, randomY, _RayTraceDrawer.GetPixelColor(randomX, randomY))

            drawTimeStopwatch.Stop()

            testedPixelCount += 1
        Loop

        stopwatch.Stop()

        _ResultImage.Source = New SimpleBitmap(bitmap).ToBitmapSource

        'experiment --> 
        Const factor = 3.4
        Dim ticksPerPixel = drawTimeStopwatch.ElapsedTicks / testedPixelCount * factor

        Dim timePerPixel = New TimeSpan(CLng(ticksPerPixel))
        _AverageNeededTimePerPixelLabel.Content = "Time per Pixel: " & timePerPixel.TotalMilliseconds.ToString & "ms"

        Dim picturePixelCount = size.Width * size.Height

        Dim overallTime = New TimeSpan(ticks:=CLng(ticksPerPixel * picturePixelCount))
        _TotalNeededTimeLabel.Content = "Overall time: " & overallTime.ToString
    End Sub

    Private _CalculateTimeOptionsDialog As New CalculateTimeOptionsDialog

    Private Sub CalculateNeededTimeOptionsButton_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles _CalculateNeededTimeOptionsButton.Click
        _CalculateTimeOptionsDialog.ShowDialog()
    End Sub

End Class
