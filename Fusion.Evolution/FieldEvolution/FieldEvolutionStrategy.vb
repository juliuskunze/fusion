Public Class FieldEvolutionStrategy
    Implements IEvolutionStrategy(Of ParticleField2D)

    Private _Initializer As IInitializer(Of ParticleField2D)
    Private _Fitness As IFitness(Of ParticleField2D)
    Private _Mutator As FieldMutator

    Public Event BestSolutionImproved(ByVal sender As Object, ByVal e As SolutionEventArgs(Of Physics.ParticleField2D)) Implements IEvolutionStrategy(Of Physics.ParticleField2D).BestSolutionImproved

    Public Sub New(ByVal fitness As IFitness(Of ParticleField2D), ByVal initializer As IInitializer(Of ParticleField2D), ByVal mutator As FieldMutator)
        _Fitness = fitness
        _Initializer = initializer
        _Mutator = mutator
    End Sub

    Private _currentBestSolution As ParticleField2D
    Public ReadOnly Property CurrentBestSolution As Physics.ParticleField2D Implements IEvolutionStrategy(Of Physics.ParticleField2D).CurrentBestSolution
        Get
            Return _currentBestSolution
        End Get
    End Property

    Public ReadOnly Property CurrentBestFitness As Double Implements IEvolutionStrategy(Of Physics.ParticleField2D).CurrentBestFitness
        Get
            Return _Fitness.Fitness(_currentBestSolution)
        End Get
    End Property

    Public Sub StartEvolution() Implements IEvolutionStrategy(Of Physics.ParticleField2D).StartEvolution
        _currentBestSolution = _Initializer.Initialize
    End Sub

    Public Sub Evolute()
        For i = 0 To 100
            Dim mutatedSolution = _Mutator.Mutate(_currentBestSolution)
            Dim currentFitness = _Fitness.Fitness(_currentBestSolution)
            Dim mutatedFitness = _Fitness.Fitness(mutatedSolution)
            If mutatedFitness > currentFitness Then
                _currentBestSolution = mutatedSolution
                RaiseEvent BestSolutionImproved(Me, New SolutionEventArgs(Of ParticleField2D)(CurrentBestSolution))
            End If
        Next
    End Sub

End Class
