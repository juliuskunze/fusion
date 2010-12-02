Public Class VectorEvolutionStrategy
    Implements IEvolutionStrategy(Of Vector2D)

    Public Event BestSolutionImproved(ByVal sender As Object, ByVal e As SolutionEventArgs(Of Math.Vector2D)) Implements IEvolutionStrategy(Of Vector2D).BestSolutionImproved

    Public Sub New(Optional ByVal maxStepsWithoutFitnessGrowth As Integer = 100)
        _maxStepsWithoutFitnessGrowth = maxStepsWithoutFitnessGrowth
    End Sub

    Private _maxStepsWithoutFitnessGrowth As Integer
    Public Property MaxStepsWithoutFitnessGrowth() As Integer
        Get
            Return _maxStepsWithoutFitnessGrowth
        End Get
        Set(ByVal value As Integer)
            _maxStepsWithoutFitnessGrowth = value
        End Set
    End Property


    Private _vectorInitializer As IInitializer(Of Vector2D) = New VectorInitializer
    Private _vectorMutator As IMutator(Of Vector2D) = New VectorMutator(1)
    Private _vectorFitness As VectorFitness = New VectorFitness(New Vector2D(10, 10))

    Public Sub Start() Implements IEvolutionStrategy(Of Vector2D).StartEvolution
        _solution = _vectorInitializer.Initialize()
        RaiseEvent BestSolutionImproved(Me, New SolutionEventArgs(Of Vector2D)(_solution))

        Dim stepsWithoutFitnessGrowth As Integer = 0

        Do
            Dim oldfitness = _fitness

            Dim mutantSolution = _vectorMutator.Mutate(_solution)
            If _vectorFitness.Fitness(mutantSolution) >= _vectorFitness.Fitness(_solution) Then
                _solution = mutantSolution
            Else
                RaiseEvent BadSolutionGenerated(Me, New SolutionEventArgs(Of Vector2D)(mutantSolution))
            End If

            _fitness = _vectorFitness.Fitness(_solution)
            RaiseEvent BestSolutionImproved(Me, New SolutionEventArgs(Of Vector2D)(_solution))

            If oldfitness = _fitness Then
                stepsWithoutFitnessGrowth += 1
            Else
                stepsWithoutFitnessGrowth = 0
            End If

        Loop Until _fitness > -1 OrElse stepsWithoutFitnessGrowth >= _maxStepsWithoutFitnessGrowth
    End Sub

    Private _solution As Vector2D
    Public ReadOnly Property CurrentBestSolution() As Vector2D Implements IEvolutionStrategy(Of Vector2D).CurrentBestSolution
        Get
            Return _solution
        End Get
    End Property

    Private _fitness As Double
    Public ReadOnly Property CurrentBestFitness() As Double Implements IEvolutionStrategy(Of Vector2D).CurrentBestFitness
        Get
            Return _fitness
        End Get
    End Property

    Public Event BadSolutionGenerated(ByVal sender As Object, ByVal e As SolutionEventArgs(Of Vector2D))

End Class
