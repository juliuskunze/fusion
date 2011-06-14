Public Class MultiFitness(Of SolutionType)
    Implements IFitness(Of SolutionType)

    Public Sub New()
        _FitnessFunctions = New List(Of IFitness(Of SolutionType))
    End Sub

    Private _FitnessFunctions As List(Of IFitness(Of SolutionType))
    Public Property FitnessFunctions() As List(Of IFitness(Of SolutionType))
        Get
            Return _FitnessFunctions
        End Get
        Set(ByVal value As List(Of IFitness(Of SolutionType)))
            _FitnessFunctions = value
        End Set
    End Property

    Public Function Fitness(ByVal solution As SolutionType) As Double Implements IFitness(Of SolutionType).Fitness
        Fitness = 0

        For Each fitnessFunction In Me.FitnessFunctions
            Fitness += fitnessFunction.Fitness(solution)
        Next

        Return Fitness
    End Function
End Class
