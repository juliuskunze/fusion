Imports System.Runtime.CompilerServices

Public Module StringExtensions

    <Extension()>
    Public Function WithoutBlanks(s As String) As String
        Return New String((s.Where(Function(c) Not Char.IsWhiteSpace(c))).ToArray)
    End Function

    <Extension()>
    Public Function IsValidVariableName(s As String) As Boolean
        If s = "" Then Return False

        Return Char.IsLetter(s.First) AndAlso s.All(Function(c) Char.IsLetterOrDigit(c))
    End Function

    <Extension()>
    Public Function GetCharIsInBracketsArray(s As String) As Boolean()
        Dim _CharIsInBrackets(s.Length - 1) As Boolean
        Dim bracketDepth = 0

        For i = 0 To s.Length - 1
            If s.Chars(i) = "("c Then
                bracketDepth += 1
            End If

            If bracketDepth < 0 Then Throw New InvalidTermException(s, message:="End of term exprected.")

            If bracketDepth > 0 Then
                _CharIsInBrackets(i) = True
            End If

            If s.Chars(i) = ")"c Then
                bracketDepth -= 1
            End If
        Next

        If bracketDepth > 0 Then Throw New InvalidTermException(s, message:="Missing "")"".")

        Return _CharIsInBrackets
    End Function

    <Extension()>
    Public Function IsInBrackets(s As String) As Boolean
        Dim charIsInBracketsArray = s.GetCharIsInBracketsArray

        Return charIsInBracketsArray.All(Function(inBrackets) inBrackets)
    End Function

End Module
