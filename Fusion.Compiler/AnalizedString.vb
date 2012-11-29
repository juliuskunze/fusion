Public Class AnalizedString
    Private ReadOnly _Text As String
    Public ReadOnly Property Text As String
        Get
            Return _Text
        End Get
    End Property

    Private ReadOnly _AllowedBracketTypes As IEnumerable(Of BracketType)

    Public Sub New(text As String)
        Me.New(text, AllowedBracketTypes:=CompilerTools.AllowedBracketTypes)
    End Sub

    Public Sub New(text As String, allowedBracketTypes As IEnumerable(Of BracketType))
        _Text = text
        _AllowedBracketTypes = allowedBracketTypes
    End Sub

    Public Function ToLocated() As LocatedString
        Return New LocatedString(Me, New TextLocation(startIndex:=0, length:=_Text.Length))
    End Function

    Public Overrides Function ToString() As String
        Return _Text
    End Function
End Class
