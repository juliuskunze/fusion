Public NotInheritable Class TermParser
    Private Sub New()
    End Sub

    Public Shared Function TryParse(ByVal term As String, ByVal out_result As Double) As Boolean
        Return New Term(termWithoutBlanks:=BlanksRemoved(term)).TryGetValue(out_result:=out_result)
    End Function

    Public Shared Function Parse(ByVal term As String) As Double
        Return New Term(termWithoutBlanks:=BlanksRemoved(term)).GetValue
    End Function

    Private Shared Function BlanksRemoved(ByVal term As String) As String
        For i = term.Length - 1 To 0 Step -1
            If Char.IsWhiteSpace(term(i)) Then
                term = term.Remove(startIndex:=i, count:=1)
            End If
        Next

        Return term
    End Function

End Class

