Public Class SolutionEventArgs(Of TSolution)
    Inherits EventArgs

    Public Sub New(solution As TSolution)
        _Solution = solution
    End Sub

    Private ReadOnly _Solution As TSolution
    Public ReadOnly Property Solution() As TSolution
        Get
            Return _Solution
        End Get
    End Property
End Class
