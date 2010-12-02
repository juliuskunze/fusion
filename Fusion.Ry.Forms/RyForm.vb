Public Class RyForm

    Private WithEvents _rayTraceDrawer As RayTraceDrawer
    Private _graphics As Graphics
    Private _backColor As Color
    Private _picture As Bitmap

    Private _customPictureSizeOk As Boolean = False
    Private _customPictureSize As Size
    Private _pictureSize As Size

    Private Sub startButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles startButton.Click
        If customSizeRadioButton.Checked Then
            If _customPictureSizeOk Then
                _pictureSize = _customPictureSize
            Else
                Return
            End If
        Else
            _pictureSize = pictureBox.Size
        End If

        _rayTraceDrawer = New RayTracingExamples(_pictureSize).IluminationRoom
        _picture = _rayTraceDrawer.Picture
        pictureBox.BackgroundImage = _picture

        Me.saveButton.Enabled = True
    End Sub

    Private Sub pictureBox_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pictureBox.MouseDown
        Dim mouseLocation = e.Location
        ColorColorPanel(mouseLocation)
    End Sub

    Private Sub ColorColorPanel(ByVal mouseLocation As Point)
        colorPanel.BackColor = _rayTraceDrawer.GetPixelColor(mouseLocation.X, mouseLocation.Y)
        colorPanel.BackColor = _rayTraceDrawer.GetPixelColor(mouseLocation.X, mouseLocation.Y)
    End Sub

    Private Sub rayTraceDrawer_ProgressIncreased(ByVal sender As Object, ByVal e As ProgressEventArgs) Handles _rayTraceDrawer.ProgressIncreased
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
            _picture.Save(saveFileDialog.FileName)
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
        Dim videoTracer As New LensVideo(New Size(500, 500))
        videoTracer.CreateVideo("B:\tmp\vid", 0, 3, 0.050000000000000003)
    End Sub
End Class

