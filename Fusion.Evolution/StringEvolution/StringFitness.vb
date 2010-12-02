Public Class StringFitness
    Implements IFitness(Of String)

    Private _targetString As String
    Public Property TargetString() As String
        Get
            Return _targetString
        End Get
        Set(ByVal value As String)
            _targetString = value
        End Set
    End Property


    Public Sub New(ByVal targetString As String)
        _targetString = targetString
    End Sub

    Public Function Fitness(ByVal solution As String) As Double Implements IFitness(Of String).Fitness
        Fitness = 0

        If TargetString.Length <> solution.Length Then
            Fitness -= Abs(TargetString.Length - solution.Length)
        End If

        For index = 0 To Min(TargetString.Length, solution.Length) - 1
            If Me.TargetString(index) = solution(index) Then
                Fitness += 1 / TargetString.Length
            End If
        Next

        Return Fitness
    End Function
End Class
