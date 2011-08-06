Public Class DefinitionBase

    Protected ReadOnly _DefinitionWithoutBlanks As String
    Protected ReadOnly _UserContext As TermContext

    Public Sub New(definition As String, userContext As TermContext)
        _DefinitionWithoutBlanks = definition.WithoutBlanks
        _UserContext = userContext
    End Sub

End Class
