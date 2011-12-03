''' <summary>
''' Analyzes an assignment like 'Real a = 0'.
''' </summary>
''' <remarks></remarks>
Public Class Assignment

    Protected ReadOnly _SignatureString As LocatedString
    ''' <summary>
    ''' First part of the assignment like 'Real a'.
    ''' </summary>
    Public ReadOnly Property SignatureString As LocatedString
        Get
            Return _SignatureString
        End Get
    End Property

    Protected ReadOnly _TermString As LocatedString
    ''' <summary>
    ''' Last part of the assignment like '0'.
    ''' </summary>
    Public ReadOnly Property TermString As LocatedString
        Get
            Return _TermString
        End Get
    End Property

    Protected ReadOnly _Context As TermContext

    Public Sub New(definition As LocatedString, context As TermContext)
        Dim parts = definition.SplitIfSeparatorIsNotInBrackets(separator:="="c, bracketTypes:=CompilerTools.AllowedBracketTypes)

        If parts.Count <> 2 Then Throw New InvalidTermException(definition, "Definition expected.")

        _SignatureString = parts.First
        _TermString = parts.Last

        _Context = context
    End Sub

    Public ReadOnly Property IsFunctionDefinition As Boolean
        Get
            Return GetIsFunctionDefinition(_SignatureString.ToString)
        End Get
    End Property

    Private Shared Function GetIsFunctionDefinition(signatureString As String) As Boolean
        Return signatureString.Contains(CompilerTools.ParameterBracketType.OpeningBracket) OrElse signatureString.Contains(CompilerTools.ParameterBracketType.ClosingBracket)
    End Function

End Class
