Public Class Signature

    Public Shared Function IsConstantSignatureDefinition(definition As String) As Boolean
        Return definition.Contains(CompilerTools.ParameterBracketTypes.Single.OpeningBracket) OrElse definition.Contains(CompilerTools.ParameterBracketTypes.Single.ClosingBracket)
    End Function

End Class
