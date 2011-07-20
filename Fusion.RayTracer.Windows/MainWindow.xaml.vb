Public Class MainWindow

    Private WithEvents _RayTraceDrawer As RayTraceDrawer(Of RgbLight)
    Private _ResultBitmap As System.Drawing.Bitmap
    Private _RenderStopwatch As Stopwatch

    Private WithEvents _RenderBackgroundWorker As ComponentModel.BackgroundWorker

    Private _CustomPictureSizeOk As Boolean = False
    Private _CustomPictureSize As System.Drawing.Size

    Private _SaveFileDialog As New SaveFileDialog

    Public Sub New()
        Me.InitializeComponent()

        _RenderBackgroundWorker = New ComponentModel.BackgroundWorker
        _RenderBackgroundWorker.WorkerReportsProgress = True
        _RenderBackgroundWorker.WorkerSupportsCancellation = True
    End Sub

    Private Sub RenderButton_Click(ByVal sender As System.Object, ByVal e As RoutedEventArgs) Handles _RenderButton.Click
        If Not Me.TrySetRayTracerDrawer Then Return

        _RenderButton.Visibility = Visibility.Collapsed
        _RenderCancelButton.Visibility = Visibility.Visible
        _RenderProgressBar.Visibility = Visibility.Visible
        Me.TaskbarItemInfo.ProgressState = Shell.TaskbarItemProgressState.Normal

        _RenderStopwatch = Stopwatch.StartNew

        _RenderBackgroundWorker.RunWorkerAsync(_RayTraceDrawer)
    End Sub

    Private Function TrySetRayTracerDrawer() As Boolean
        Dim pictureSize As System.Drawing.Size
        If Not Me.TryGetPictureSize(out_size:=pictureSize) Then Return False

        _RayTraceDrawer = New RayTracingExamples(pictureSize).ExampleBox
        Return True
    End Function

    Private Function TryGetPictureSize(ByRef out_size As System.Drawing.Size) As Boolean
        If Not _CustomPictureSizeOk Then Return False
        out_size = _CustomPictureSize
        Return True
    End Function



    Private Sub SaveButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _SaveButton.Click
        Const fileEnding = ".bmp"
        _SaveFileDialog.FileName = "ray tracing picture "
        If _SaveFileDialog.InitialDirectory = "" Then _SaveFileDialog.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.Desktop
        Dim pictureNumber As Integer = 1
        Do While New IO.FileInfo(_SaveFileDialog.InitialDirectory & "\" & _SaveFileDialog.FileName & pictureNumber & fileEnding).Exists
            pictureNumber += 1
        Loop
        _SaveFileDialog.FileName &= pictureNumber & fileEnding
        If _SaveFileDialog.ShowDialog Then
            _ResultBitmap.Save(_SaveFileDialog.FileName)
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

    Private Sub CalculateNeededTimeButton_Click(ByVal sender As System.Object, ByVal e As RoutedEventArgs) Handles _CalculateNeededTimeButton.Click
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

    Private Sub CalculateNeededTimeOptionsButton_Click(ByVal sender As System.Object, ByVal e As RoutedEventArgs) Handles _CalculateNeededTimeOptionsButton.Click
        _CalculateTimeOptionsDialog.ShowDialog()
    End Sub

    Private Sub RenderBackgroundWorker_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles _RenderBackgroundWorker.DoWork
        Dim rayTracerDrawer = CType(e.Argument, RayTraceDrawer(Of RgbLight))

        Dim resultBitmap = New System.Drawing.Bitmap(rayTracerDrawer.PictureSize.Width, rayTracerDrawer.PictureSize.Height)

        For bitmapX = 0 To rayTracerDrawer.PictureSize.Width - 1
            If _RenderBackgroundWorker.CancellationPending Then
                e.Cancel = True
                Return
            End If

            For bitmapY = 0 To rayTracerDrawer.PictureSize.Height - 1
                rayTracerDrawer.SetPixelColor(resultBitmap, bitmapX, bitmapY)
            Next
            _RenderBackgroundWorker.ReportProgress(CInt((bitmapX + 1) / rayTracerDrawer.PictureSize.Width * 100))
        Next

        e.Result = resultBitmap
    End Sub

    Private Sub RenderBackgroundWorker_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles _RenderBackgroundWorker.ProgressChanged
        _RenderProgressBar.Value = e.ProgressPercentage
    End Sub

    Private Sub RenderBackgroundWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles _RenderBackgroundWorker.RunWorkerCompleted
        If e.Error IsNot Nothing Then Throw e.Error

        _RenderProgressBar.Value = 0
        _RenderProgressBar.Visibility = Visibility.Collapsed
        _RenderButton.Visibility = Visibility.Visible
        _RenderCancelButton.Visibility = Visibility.Collapsed
        _RenderTimeCalculationGroupBox.Visibility = Visibility.Collapsed
        _RenderStopwatch.Stop()
        Me.TaskbarItemInfo.ProgressState = Shell.TaskbarItemProgressState.None

        If e.Cancelled Then
            _RenderTimeCalculationGroupBox.Visibility = Visibility.Visible
            Return
        End If

        _ResultBitmap = CType(e.Result, System.Drawing.Bitmap)

        _ResultImage.Source = New SimpleBitmap(_ResultBitmap).ToBitmapSource

        _TotalElapsedTimeLabel.Content = "Total elapsed time: " & _RenderStopwatch.Elapsed.ToString
        _AverageElapsedTimePerPixelLabel.Content = "Average elapsed time per pixel: " & (_RenderStopwatch.ElapsedMilliseconds / (_ResultBitmap.Size.Width * _ResultBitmap.Size.Height)).ToString & "ms"

        _RenderProgressBar.Value = 0

        _SaveButton.IsEnabled = True
    End Sub

    Private Sub RenderCancelButton_Click(ByVal sender As System.Object, ByVal e As RoutedEventArgs) Handles _RenderCancelButton.Click
        _RenderBackgroundWorker.CancelAsync()
    End Sub

    Private Sub _RenderProgressBar_ValueChanged(ByVal sender As Object, ByVal e As System.Windows.RoutedPropertyChangedEventArgs(Of Double)) Handles _RenderProgressBar.ValueChanged
        Me.TaskbarItemInfo.ProgressValue = e.NewValue / 100
    End Sub

    Private Sub RibbonWindow_Unloaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Unloaded
        Application.Current.Shutdown()
    End Sub

End Class
