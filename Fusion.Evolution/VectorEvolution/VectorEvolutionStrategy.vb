Public Class VectorEvolutionStrategy
    Implements IEvolutionStrategy(Of Vector2D)

    Public Event BestSolutionImproved(sender As Object, e As SolutionEventArgs(Of Math.Vector2D)) Implements IEvolutionStrategy(Of Vector2D).BestSolutionImproved

    Public Sub New(Optional maxStepsWithoutFitnessGrowth As Integer = 100)
        _MaxStepsWithoutFitnessGrowth = maxStepsWithoutFitnessGrowth
    End Sub

    Private _MaxStepsWithoutFitnessGrowth As Integer
    Public Property MaxStepsWithoutFitnessGrowth() As Integer
        Get
            Return _MaxStepsWithoutFitnessGrowth
        End Get
        Set(value As Integer)
            _MaxStepsWithoutFitnessGrowth = value
        End Set
    End Property


    Private _VectorInitializer As IInitializer(Of Vector2D) = New VectorInitializer
    Private _VectorMutator As IMutator(Of Vector2D) = New VectorMutator(1)
    Private _VectorFitness As VectorFitness = New VectorFitness(New Vector2D(10, 10))

    Public Sub Start() Implements IEvolutionStrategy(Of Vector2D).StartEvolution
        _Solution = _VectorInitializer.Initialize()
        RaiseEvent BestSolutionImproved(Me, New SolutionEventArgs(Of Vector2D)(_Solution))

        Dim stepsWithoutFitnessGrowth As Integer = 0

        Do
            Dim oldfitness = _Fitness

            Dim mutantSolution = _VectorMutator.Mutate(_Solution)
            If _VectorFitness.Fitness(mutantSolution) >= _VectorFitness.Fitness(_Solution) Then
                _Solution = mutantSolution
            Else
                RaiseEvent BadSolutionGenerated(Me, New SolutionEventArgs(Of Vector2D)(mutantSolution))
            End If

            _Fitness = _VectorFitness.Fitness(_Solution)
            RaiseEvent BestSolutionImproved(Me, New SolutionEventArgs(Of Vector2D)(_Solution))

            If oldfitness = _Fitness Then
                stepsWithoutFitnessGrowth += 1
            Else
                stepsWithoutFitnessGrowth = 0
            End If

        Loop Until _Fitness > -1 OrElse stepsWithoutFitnessGrowth >= _MaxStepsWithoutFitnessGrowth
    End Sub

    Private _Solution As Vector2D
    Public ReadOnly Property CurrentBestSolution() As Vector2D Implements IEvolutionStrategy(Of Vector2D).CurrentBestSolution
        Get
            Return _Solution
        End Get
    End Property

    Private _Fitness As Double
    Public ReadOnly Property CurrentBestFitness() As Double Implements IEvolutionStrategy(Of Vector2D).CurrentBestFitness
        Get
            Return _Fitness
        End Get
    End Property

    Public Event BadSolutionGenerated(sender As Object, e As SolutionEventArgs(Of Vector2D))

End Class
