Public Class Assignment

    Protected ReadOnly _Declaration As String
    Public ReadOnly Property Declaration As String
        Get
            Return _Declaration
        End Get
    End Property

    Protected ReadOnly _Term As String
    Public ReadOnly Property Term As String
        Get
            Return _Term
        End Get
    End Property
    
    Protected ReadOnly _Context As TermContext

    Public Sub New(definition As String, context As TermContext)
        Dim parts = definition.SplitIfSeparatorIsNotInBrackets(separator:="="c, bracketTypes:=CompilerTools.AllowedBracketTypes)

        If parts.Count <> 2 Then Throw New InvalidTermException("Definition invalid.")

        _Declaration = parts.First
        _Term = parts.Last

        _Context = context
    End Sub

    Public ReadOnly Property IsFunctionAssignment As Boolean
        Get
            Return Assignment.IsConstantSignatureDefinition(_Declaration)
        End Get
    End Property

    Private Shared Function IsConstantSignatureDefinition(definition As String) As Boolean
        Return definition.Contains(CompilerTools.ParameterBracketType.OpeningBracket) OrElse definition.Contains(CompilerTools.ParameterBracketType.ClosingBracket)
    End Function

End Class
