Public Class Definition

    Protected ReadOnly _Left As String
    Public ReadOnly Property Left As String
        Get
            Return _Left
        End Get
    End Property

    Protected ReadOnly _Term As String
    Public ReadOnly Property Term As String
        Get
            Return _Term
        End Get
    End Property


    Protected ReadOnly _UserContext As TermContext

    Public Sub New(definition As String, userContext As TermContext)
        Dim parts = definition.SplitIfSeparatorIsNotInBrackets(separator:="="c, bracketTypes:=CompilerTools.AllowedBracketTypes)

        If parts.Count <> 2 Then Throw New InvalidTermException("Definition invalid.")

        _Left = parts.First
        _Term = parts.Last

        _UserContext = userContext
    End Sub

    Public ReadOnly Property IsFunctionDefinition As Boolean
        Get
            Return _Left.Contains(CompilerTools.ArgumentBracketTypes.Single.OpeningBracket) OrElse _Left.Contains(CompilerTools.ArgumentBracketTypes.Single.ClosingBracket)
        End Get
    End Property

End Class
