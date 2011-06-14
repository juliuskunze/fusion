Public Class RyForm

    Private WithEvents _RayTraceDrawer As RayTraceDrawer
    Private _Graphics As Graphics
    Private _BackColor As Color
    Private _Picture As Bitmap

    Private _customPictureSizeOk As Boolean = False
    Private _customPictureSize As Size

    Public Sub New()
        Me.InitializeComponent()
        screenSizeRadioButton.Text = "Screen " & New Vector2D(My.Computer.Screen.Bounds.Size).ToString
    End Sub

    Private Sub startButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles startButton.Click
        If Not Me.TrySetRayTracerDrawer Then Return

        Dim stopWatch = New Stopwatch
        stopWatch.Start()

        _Picture = _RayTraceDrawer.Picture

        stopWatch.Stop()
        elapsedTimeLabel.Text = "Time: " & stopWatch.Elapsed.ToString
        timePerPixelLabel.Text = "Time per pixel: " & (stopWatch.ElapsedMilliseconds / (_Picture.Size.Width * _Picture.Size.Height)).ToString & "ms"

        pictureBox.BackgroundImage = _Picture

        Me.saveButton.Enabled = True
    End Sub

    Private Function TrySetRayTracerDrawer() As Boolean
        Dim pictureSize As Size
        If Not Me.GetPictureSize(out_Size:=pictureSize) Then Return False

        _RayTraceDrawer = New RayTracingExamples(pictureSize).SecondRoom(cameraZLocation:=29)
        Return True
    End Function

    Private Function GetPictureSize(ByRef out_Size As Size) As Boolean

        If customSizeRadioButton.Checked Then
            If Not _customPictureSizeOk Then Return False

            out_Size = _customPictureSize
        ElseIf windowSizeRadioButton.Checked Then
            out_Size = pictureBox.Size
        Else
            out_Size = My.Computer.Screen.Bounds.Size
        End If

        Return True
    End Function

    Private Sub pictureBox_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pictureBox.MouseDown
        Dim mouseLocation = e.Location
        ColorColorPanel(mouseLocation)
    End Sub

    Private Sub ColorColorPanel(ByVal mouseLocation As Point)
        colorPanel.BackColor = _RayTraceDrawer.GetPixelColor(mouseLocation.X, mouseLocation.Y)
    End Sub

    Private Sub rayTraceDrawer_ProgressIncreased(ByVal sender As Object, ByVal e As ProgressEventArgs) Handles _RayTraceDrawer.ProgressIncreased
        Me.progressBar.Value = CInt(e.Progress * 100)
    End Sub

    Private Sub saveButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles saveButton.Click
        saveFileDialog.FileName = "ray tracing picture "
        Dim pictureNumber As Integer = 1
        Do While New IO.FileInfo(saveFileDialog.InitialDirectory & "\" & saveFileDialog.FileName & pictureNumber).Exists
            pictureNumber += 1
        Loop
        saveFileDialog.FileName &= pictureNumber
        If saveFileDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            _Picture.Save(saveFileDialog.FileName)
        End If
    End Sub

    Private Sub form_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.customSizeTextBox.Text = New Vector2D(pictureBox.Size).ToString
    End Sub

    Private Sub customSizeRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles customSizeRadioButton.CheckedChanged
        Me.customSizeTextBox.Enabled = customSizeRadioButton.Checked
    End Sub

    Private Sub customSizeTextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles customSizeTextBox.TextChanged
        Try
            Dim sizeVector = New Vector2D(Me.customSizeTextBox.Text)
            _customPictureSize = New Size(CInt(sizeVector.X), CInt(sizeVector.Y))
            customSizeTextBox.BackColor = Color.White
            _customPictureSizeOk = True
        Catch
            customSizeTextBox.BackColor = Color.Tomato
            _customPictureSizeOk = False
        End Try
    End Sub

    Private Sub VideoButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VideoButton.Click
        Dim viewCourse = New LinearViewCourse(velocity:=New Vector3D(0, 0, -1),
                                             startLocation:=New Vector3D(7.5, 6, 30),
                                             visibleXAngle:=PI * 0.26)

        Dim videoTracer As New ViewCourseVideo(videoSize:=New Size(500, 500),
                                               rayTracer:=New RayTracingExamples(Nothing).SecondRoomRayTracer,
                                               cameraViewCourse:=viewCourse)
        videoTracer.CreateVideo("B:\tmp\vid", timeIntervalStart:=0, timeIntervalEnd:=30, timeStep:=1)
    End Sub

    Private Sub calculateTimeButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calculateTimeButton.Click
        If Not Me.TrySetRayTracerDrawer() Then Return
        If Not _calculatedTimeOptionsForm.DialogResult = Windows.Forms.DialogResult.OK Then Return

        Dim size = _RayTraceDrawer.PictureSize

        Dim bitmap = New Bitmap(size.Width, size.Height)

        Dim random = New Random

        Dim drawTimeStopwatch = New Stopwatch
        Dim testedPixelCount = 0

        Dim stopwatch = New Stopwatch
        stopwatch.Start()
        Do While If(_calculatedTimeOptionsForm.Mode = CalculateTimeOptionsForm.FixMode.Time,
                    stopwatch.ElapsedMilliseconds / 1000 < _calculatedTimeOptionsForm.FixTestTime,
                    testedPixelCount < _calculatedTimeOptionsForm.FixTestPixelCount)
            Dim randomX = random.Next(size.Width)
            Dim randomY = random.Next(size.Height)

            drawTimeStopwatch.Start()

            bitmap.SetPixel(randomX, randomY, _RayTraceDrawer.GetPixelColor(randomX, randomY))

            drawTimeStopwatch.Stop()

            testedPixelCount += 1
        Loop

        stopwatch.Stop()

        pictureBox.BackgroundImage = bitmap

        'experiment --> 
        Const factor = 3.4
        Dim ticksPerPixel = drawTimeStopwatch.ElapsedTicks / testedPixelCount * factor

        Dim timePerPixel = New TimeSpan(CLng(ticksPerPixel))
        calculatedTimePerPixelLabel.Text = "Time per Pixel: " & timePerPixel.TotalMilliseconds.ToString & "ms"

        Dim picturePixelCount = size.Width * size.Height

        Dim overallTime = New TimeSpan(ticks:=CLng(ticksPerPixel * picturePixelCount))
        calculatedOverallTimeLabel.Text = "Overall time: " & overallTime.ToString

        testedPixelCountLabel.Text = "(Tested pixels: " & testedPixelCount.ToString & ")"
    End Sub

    Private _calculatedTimeOptionsForm As CalculateTimeOptionsForm = New CalculateTimeOptionsForm

    Private Sub calculateTimeOptions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calculateTimeOptions.Click
        _calculatedTimeOptionsForm.ShowDialog()
    End Sub

    Private Sub pictureBox_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pictureBox.Resize
        windowSizeRadioButton.Text = "Window " & New Vector2D(pictureBox.Size).ToString
    End Sub
End Class

