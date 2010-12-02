Public Class SolutionEventArgs(Of SolutionType)
    Inherits EventArgs

    Public Sub New(ByVal solution As SolutionType)
        _solution = solution
    End Sub

    Private _solution As SolutionType
    Public ReadOnly Property Solution() As SolutionType
        Get
            Return _solution
        End Get
    End Property

End Class
