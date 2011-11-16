Public Class RenderTimeEstimationOptionsDialog

    Public Sub New()
        Me.InitializeComponent()
    End Sub

    Private Sub OkButton_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles _OkButton.Click
        Me.DialogResult = True

        If Me.Mode = RenderTimeEstimationMode.FixTime Then
            Try
                Dim time = CDbl(Me._MaxTimeBox.Text)
            Catch
                MessageBox.Show("Invalid fix time.")
                Me.DialogResult = False
            End Try
        Else
            Try
                Dim pixelCount = CDbl(Me._PixelCountBox.Text)
            Catch
                MessageBox.Show("Invalid fix pixel count.")
                Me.DialogResult = False
            End Try
        End If

        Me.Hide()
    End Sub

    Public ReadOnly Property Options As RenderTimeEstimationOptions
        Get
            If Me.Mode = RenderTimeEstimationMode.FixTime Then
                Return New RenderTimeEstimationOptions(Time:=Me.Time)
            Else
                Return New RenderTimeEstimationOptions(PixelCount:=Me.PixelCount)
            End If
        End Get
    End Property

    Private ReadOnly Property Time As Double
        Get
            Return CDbl(_MaxTimeBox.Text)
        End Get
    End Property

    Private ReadOnly Property PixelCount As Integer
        Get
            Return CInt(_PixelCountBox.Text)
        End Get
    End Property

    Private ReadOnly Property Mode As RenderTimeEstimationMode
        Get
            If _FixTimeRadioButton.IsChecked Then
                Return RenderTimeEstimationMode.FixTime
            Else
                Return RenderTimeEstimationMode.FixPixelCount
            End If
        End Get
    End Property

    Private Sub Grid_Checked(sender As System.Object, e As System.Windows.RoutedEventArgs)
        If Not Me.IsLoaded Then Return

        _MaxTimeBox.IsEnabled = _FixTimeRadioButton.IsChecked.Value
        _PixelCountBox.IsEnabled = _FixPixelCountRadioButton.IsChecked.Value
    End Sub

    Protected Overrides Sub OnClosing(e As System.ComponentModel.CancelEventArgs)
        e.Cancel = True

        Me.Hide()
    End Sub

End Class
