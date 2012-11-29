Public Class TextToolTip
    Inherits ToolTip

    Private ReadOnly _TextBlock As TextBlock

    Public Sub New()
        _TextBlock = New TextBlock With {.TextWrapping = TextWrapping.WrapWithOverflow}
        MyBase.Content = _TextBlock
    End Sub

    <Obsolete()>
    Public Shadows ReadOnly Property Content As Object
        Get
            Throw New NotImplementedException
        End Get
    End Property

    Public Property Text As String
        Get
            Return _TextBlock.Text
        End Get
        Set(value As String)
            _TextBlock.Text = value
        End Set
    End Property
End Class
