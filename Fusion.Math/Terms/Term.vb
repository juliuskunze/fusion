Public Class Term
    Inherits TermWithoutBlanks

    Public Sub New(ByVal term As String, ByVal parameterNames As IEnumerable(Of String), ByVal userFunctions As IEnumerable(Of NamedExpression))
        MyBase.New(TermWithoutBlanks:=BlanksRemoved(term),
                   parameterNames:=parameterNames,
                   userFunctions:=userFunctions)
    End Sub

    Private Shared Function BlanksRemoved(ByVal term As String) As String
        Return New String((term.Where(predicate:=Function(c) Not Char.IsWhiteSpace(c))).ToArray)
    End Function

End Class

