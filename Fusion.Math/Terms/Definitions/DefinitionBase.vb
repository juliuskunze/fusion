Public Class DefinitionBase

    Protected ReadOnly _NamePart As String
    Public ReadOnly Property NamePart As String
        Get
            Return _NamePart
        End Get
    End Property

    Protected ReadOnly _Term As String

    Protected ReadOnly _UserContext As TermContext

    Public Sub New(definition As String, userContext As TermContext)
        Dim parts = definition.WithoutBlanks.Split("="c)

        If parts.Count <> 2 Then Throw New InvalidTermException("Definition invalid.")

        _NamePart = parts.First
        _Term = parts.Last

        _UserContext = userContext
    End Sub

End Class
