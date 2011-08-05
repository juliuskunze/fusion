Public MustInherit Class ValidatingTextBoxBase(Of T)
    Inherits TextBox

    Private Shared ReadOnly _OkBrush As Brush = Brushes.White
    Private Shared ReadOnly _ErrorBrush As Brush = Brushes.OrangeRed

    Private _Result As T
    Public ReadOnly Property Result As T
        Get
            Return _Result
        End Get
    End Property

    Private Sub ValidateAndAdaptStyle()
        _Result = Me.Convert(Me.Text)

        Me.IsOk = _Result IsNot Nothing
    End Sub

    Private Sub TermBox_LostFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.LostFocus
        Me.ValidateAndAdaptStyle()
    End Sub

    Private Property IsOk As Boolean
        Get
            Return Me.Background IsNot _OkBrush
        End Get
        Set(ByVal value As Boolean)
            If value Then
                Me.Background = _OkBrush
            Else
                Me.Background = _ErrorBrush
            End If
        End Set
    End Property

    Public ReadOnly Property HasResult As Boolean
        Get
            Return Me.IsOk
        End Get
    End Property

    Protected MustOverride Function Convert(ByVal text As String) As T

End Class