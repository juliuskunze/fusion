Public Class Term
    Inherits TermWithoutBlanks

    Public Sub New(ByVal term As String, ByVal doubleParameterNames As IEnumerable(Of String))
        MyBase.New(TermWithoutBlanks:=BlanksRemoved(term), doubleParameterNames:=doubleParameterNames)
    End Sub

    Private Shared Function BlanksRemoved(ByVal term As String) As String
        For i = term.Length - 1 To 0 Step -1
            If Char.IsWhiteSpace(term(i)) Then
                term = term.Remove(startIndex:=i, count:=1)
            End If
        Next

        Return term
    End Function

End Class

