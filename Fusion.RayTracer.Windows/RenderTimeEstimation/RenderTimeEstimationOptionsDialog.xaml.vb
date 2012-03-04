Public Class RenderTimeEstimationOptionsDialog

    Public Sub New()
        Me.InitializeComponent()
        DialogResult = False
    End Sub

    Private Sub OkButton_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles _OkButton.Click
        DialogResult = True

        If Mode = RenderTimeEstimationMode.FixTime Then
            Try
                Dim time = CDbl(_MaxTimeBox.Text)
            Catch
                MessageBox.Show("Invalid fix time.")
                Me.DialogResult = False
            End Try
        Else
            Try
                Dim pixelCount = CDbl(_PixelCountBox.Text)
            Catch
                MessageBox.Show("Invalid fix pixel count.")
                Me.DialogResult = False
            End Try
        End If

        Hide()
    End Sub

    Public ReadOnly Property Options As RenderTimeEstimationOptions
        Get
            If Mode = RenderTimeEstimationMode.FixTime Then
                Return New RenderTimeEstimationOptions(Time:=Time)
            Else
                Return New RenderTimeEstimationOptions(PixelCount:=PixelCount)
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

    Private Sub Grid_Checked(sender As System.Object, e As RoutedEventArgs)
        If Not IsLoaded Then Return

        _MaxTimeBox.IsEnabled = _FixTimeRadioButton.IsChecked.Value
        _PixelCountBox.IsEnabled = _FixPixelCountRadioButton.IsChecked.Value
    End Sub

    Protected Overrides Sub OnClosing(e As ComponentModel.CancelEventArgs)
        e.Cancel = True

        Hide()
    End Sub

End Class
