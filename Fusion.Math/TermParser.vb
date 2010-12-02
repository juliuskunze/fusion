Public Module TermParser

    Private Const _invalidTermMessage As String = "The term is invalid."

    Public Function TermToValue(ByVal term As String) As Double
        For i = term.Length - 1 To 0 Step -1
            If Char.IsWhiteSpace(term(i)) Then
                term = term.Remove(startIndex:=i, count:=1)
            End If
        Next

        Return TermWithoutBlanksToValue(term)
    End Function

    Private Function TermWithoutBlanksToValue(ByVal term As String) As Double
        If term = "" Then
            Throw New ArgumentException(_invalidTermMessage)
        End If

        If IsNumeric(term) AndAlso Not term.Contains("("c) Then
            Return CDbl(term)
        End If

        Dim charIsInBrackets(term.Length - 1) As Boolean

        Dim bracketDepth As Integer = 0

        For i = 0 To term.Length - 1
            If term.Chars(i) = "("c Then
                bracketDepth += 1
            End If

            If bracketDepth < 0 Then
                Throw New ArgumentException(_invalidTermMessage)
            End If
            If bracketDepth > 0 Then
                charIsInBrackets(i) = True
            End If

            If term.Chars(i) = ")"c Then
                bracketDepth -= 1
            End If
        Next

        If bracketDepth <> 0 Then
            Throw New ArgumentException(_invalidTermMessage)
        End If

        Dim termIsInBrackets As Boolean = True
        For i = 0 To term.Length - 1
            If Not charIsInBrackets(i) Then
                termIsInBrackets = False
                Exit For
            End If
        Next

        If termIsInBrackets Then
            Return TermWithoutBlanksToValue(term.Substring(startIndex:=1, length:=term.Length - 2))
        End If

        Select Case term.Chars(0)
            Case "+"c, "-"c
                Dim minusCountAtStart As Integer = 0
                Dim notSignIndex As Integer
                For i = 0 To term.Length - 1
                    Select Case term(i)
                        Case "+"c
                        Case "-"c
                            minusCountAtStart -= 1
                        Case Else
                            notSignIndex = i
                            Exit For
                    End Select
                Next

                Dim termIsPositve As Boolean = (minusCountAtStart Mod 2 = 0)

                If termIsPositve Then
                    Return TermWithoutBlanksToValue(term.Substring(startIndex:=notSignIndex, length:=term.Length - notSignIndex))
                End If

                For i = notSignIndex To term.Length - 1
                    If Not charIsInBrackets(i) Then
                        If term.Chars(i) = "+"c Then
                            Return -TermWithoutBlanksToValue(term.Substring(startIndex:=notSignIndex, length:=i - notSignIndex)) + TermWithoutBlanksToValue(termAfterIndex(term, i))
                        End If
                    End If
                Next

                For i = notSignIndex To term.Length - 1
                    If Not charIsInBrackets(i) Then
                        If term.Chars(i) = "-"c Then
                            Return -TermWithoutBlanksToValue(term.Substring(startIndex:=notSignIndex, length:=i - notSignIndex)) - TermWithoutBlanksToValue(termAfterIndex(term, i))
                        End If
                    End If
                Next

                Return -TermWithoutBlanksToValue(term.Substring(startIndex:=notSignIndex, length:=term.Length - notSignIndex))
        End Select

        For i = 0 To term.Length - 1
            If Not charIsInBrackets(i) Then
                If term.Chars(i) = "+"c Then
                    Return TermWithoutBlanksToValue(termBeforeIndex(term, i)) + TermWithoutBlanksToValue(termAfterIndex(term, i))
                End If
            End If
        Next

        For i = 0 To term.Length - 1
            If Not charIsInBrackets(i) Then
                If term.Chars(i) = "-"c Then
                    Return TermWithoutBlanksToValue(termBeforeIndex(term, i)) - TermWithoutBlanksToValue(termAfterIndex(term, i))
                End If
            End If
        Next

        For i = 0 To term.Length - 1
            If Not charIsInBrackets(i) Then
                If term.Chars(i) = "*"c Then
                    Return TermWithoutBlanksToValue(termBeforeIndex(term, i)) * TermWithoutBlanksToValue(termAfterIndex(term, i))
                End If
            End If
        Next

        For i = 0 To term.Length - 1
            If Not charIsInBrackets(i) Then
                If term.Chars(i) = "/"c Then
                    Return TermWithoutBlanksToValue(termBeforeIndex(term, i)) / TermWithoutBlanksToValue(termAfterIndex(term, i))
                End If
            End If
        Next

        For i = 0 To term.Length - 1
            If Not charIsInBrackets(i) Then
                If term.Chars(i) = "^"c Then
                    Return TermWithoutBlanksToValue(termBeforeIndex(term, i)) ^ TermWithoutBlanksToValue(termAfterIndex(term, i))
                End If
            End If
        Next

        Throw New ArgumentException(_invalidTermMessage)
    End Function

    Private Function termBeforeIndex(ByVal term As String, ByVal index As Integer) As String
        Return term.Substring(0, index)
    End Function

    Private Function termAfterIndex(ByVal term As String, ByVal index As Integer) As String
        Return term.Substring(index + 1, term.Length - 1 - index)
    End Function

End Module
