Public Class TermDefinition

    Private ReadOnly _Term As String

    Public Sub New(term As String, definitions As IEnumerable(Of String))
        _Term = term
    End Sub

End Class
