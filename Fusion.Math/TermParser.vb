Public NotInheritable Class TermParser
    Private Sub New()
    End Sub

    Public Shared Function Parse(ByVal term As String) As Double
        For i = term.Length - 1 To 0 Step -1
            If Char.IsWhiteSpace(term(i)) Then
                term = term.Remove(startIndex:=i, count:=1)
            End If
        Next

        Return New Term(termWithoutBlanks:=term).GetValue
    End Function

End Class

