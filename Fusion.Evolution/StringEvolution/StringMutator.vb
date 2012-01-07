Public Class StringMutator
    Implements IMutator(Of String)

    Private ReadOnly _Random As New Random

    Public Function Mutate(solution As String) As String Implements IMutator(Of String).Mutate
        Dim typeOfMutation = _Random.NextDouble()

        Select Case typeOfMutation
            Case Is < 0.99
                Return changeChar(solution)
            Case Is < 0.995
                Return addChar(solution)
            Case Else
                Return trimChar(solution)
        End Select
    End Function

    Private Function changeChar(solution As String) As String
        Dim charPosition = _Random.Next(solution.Length)

        Dim newCharIndex As Integer
        Do
            newCharIndex = _Random.Next(32, 255)
        Loop While newCharIndex = 149

        Dim newChar = Chr(newCharIndex)

        Dim mutantString = solution.Substring(0, charPosition) & newChar & solution.Substring(charPosition + 1, solution.Length - charPosition - 1)

        Return mutantString
    End Function


    Private Function addChar(solution As String) As String
        Return solution & Chr(_Random.Next(32, 255))
    End Function

    Private Function trimChar(solution As String) As String
        Return solution.Substring(0, solution.Length - 1)
    End Function
End Class