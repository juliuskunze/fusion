Public Class CalculateTimeOptionsDialog

    Public Sub New()
        Me.InitializeComponent()
    End Sub

    Private Sub OkButton_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles _OkButton.Click
        Me.DialogResult = True

        If Me.Mode = FixMode.Time Then
            Try
                Dim time = CDbl(Me._FixTimeTextBox.Text)
            Catch
                MessageBox.Show("Invalid fix time.")
                Me.DialogResult = False
            End Try
        Else
            Try
                Dim pixelCount = CDbl(Me._FixPixelCountTextBox.Text)
            Catch
                MessageBox.Show("Invalid fix pixel count.")
                Me.DialogResult = False
            End Try
        End If

        Me.Hide()
    End Sub

    Public ReadOnly Property FixTestTime As Double
        Get
            Return CDbl(_FixTimeTextBox.Text)
        End Get
    End Property

    Public ReadOnly Property FixTestPixelCount As Integer
        Get
            Return CInt(_FixPixelCountTextBox.Text)
        End Get
    End Property

    Public Enum FixMode
        Time
        PixelCount
    End Enum

    Public ReadOnly Property Mode As FixMode
        Get
            If _FixTimeRadioButton.IsChecked Then
                Return FixMode.Time
            Else
                Return FixMode.PixelCount
            End If
        End Get
    End Property

    Private Sub Grid_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If Not Me.IsLoaded Then Return

        _FixTimeTextBox.IsEnabled = _FixTimeRadioButton.IsChecked.Value
        _FixPixelCountTextBox.IsEnabled = _FixPixelCountRadioButton.IsChecked.Value
    End Sub

    Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
        e.Cancel = True

        Me.Hide()
    End Sub

End Class
