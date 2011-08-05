Public NotInheritable Class TermParser
    Private Sub New()
    End Sub

    Public Shared Function TryParse(ByVal term As String) As Double?
        Dim f = New Term(Of Func(Of Double))(termWithoutBlanks:=BlanksRemoved(term), doubleParameterNames:={}).TryGetDelegate
        If f Is Nothing Then Return Nothing

        Return f.Invoke
    End Function

    Public Shared Function Parse(ByVal term As String) As Double
        Return New Term(Of Func(Of Double))(termWithoutBlanks:=BlanksRemoved(term), doubleParameterNames:={}).GetDelegate.Invoke
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

