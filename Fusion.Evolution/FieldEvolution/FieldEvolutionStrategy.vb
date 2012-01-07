Public Class FieldEvolutionStrategy
    Implements IEvolutionStrategy(Of ParticleField2D)

    Private ReadOnly _Initializer As IInitializer(Of ParticleField2D)
    Private ReadOnly _Fitness As IFitness(Of ParticleField2D)
    Private ReadOnly _Mutator As FieldMutator

    Public Event BestSolutionImproved(sender As Object, e As SolutionEventArgs(Of Physics.ParticleField2D)) Implements IEvolutionStrategy(Of Physics.ParticleField2D).BestSolutionImproved

    Public Sub New(fitness As IFitness(Of ParticleField2D), initializer As IInitializer(Of ParticleField2D), mutator As FieldMutator)
        _Fitness = fitness
        _Initializer = initializer
        _Mutator = mutator
    End Sub

    Private _CurrentBestSolution As ParticleField2D
    Public ReadOnly Property CurrentBestSolution As Physics.ParticleField2D Implements IEvolutionStrategy(Of Physics.ParticleField2D).CurrentBestSolution
        Get
            Return _CurrentBestSolution
        End Get
    End Property

    Public ReadOnly Property CurrentBestFitness As Double Implements IEvolutionStrategy(Of Physics.ParticleField2D).CurrentBestFitness
        Get
            Return _Fitness.Fitness(_CurrentBestSolution)
        End Get
    End Property

    Public Sub StartEvolution() Implements IEvolutionStrategy(Of Physics.ParticleField2D).StartEvolution
        _CurrentBestSolution = _Initializer.Initialize
    End Sub

    Public Sub Evolute()
        For i = 0 To 100
            Dim mutatedSolution = _Mutator.Mutate(_CurrentBestSolution)
            Dim currentFitness = _Fitness.Fitness(_CurrentBestSolution)
            Dim mutatedFitness = _Fitness.Fitness(mutatedSolution)
            If mutatedFitness > currentFitness Then
                _CurrentBestSolution = mutatedSolution
                RaiseEvent BestSolutionImproved(Me, New SolutionEventArgs(Of ParticleField2D)(CurrentBestSolution))
            End If
        Next
    End Sub

End Class
