Public Class SolutionEventArgs(Of SolutionType)
    Inherits EventArgs

    Public Sub New(solution As SolutionType)
        _Solution = solution
    End Sub

    Private _Solution As SolutionType
    Public ReadOnly Property Solution() As SolutionType
        Get
            Return _Solution
        End Get
    End Property

End Class
