Public Class StringMutator
    Implements IMutator(Of String)

    Private _rnd As New Random

    Public Function Mutate(ByVal solution As String) As String Implements IMutator(Of String).Mutate
        Dim typeOfMutation = _rnd.NextDouble()

        Select Case typeOfMutation
            Case Is < 0.99
                Return changeChar(solution)
            Case Is < 0.995
                Return addChar(solution)
            Case Else
                Return trimChar(solution)
        End Select
    End Function

    Private Function changeChar(ByVal solution As String) As String
        Dim charPosition = _rnd.Next(solution.Length)

        Dim newCharIndex As Integer
        Do
            newCharIndex = _rnd.Next(32, 255)
        Loop While newCharIndex = 149

        Dim newChar = Chr(newCharIndex)

        Dim mutantString = solution.Substring(0, charPosition) & newChar & solution.Substring(charPosition + 1, solution.Length - charPosition - 1)

        Return mutantString
    End Function


    Private Function addChar(ByVal solution As String) As String
        Return solution & Chr(_rnd.Next(32, 255))
    End Function

    Private Function trimChar(ByVal solution As String) As String
        Return solution.Substring(0, solution.Length - 1)
    End Function
End Class