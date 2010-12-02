Public Class StringEvolutionStrategy
    Implements IEvolutionStrategy(Of String)

    Public Event BestSolutionImproved(ByVal sender As Object, ByVal e As SolutionEventArgs(Of String)) Implements IEvolutionStrategy(Of String).BestSolutionImproved

    Public Sub New(ByVal maxStepsWithoutFitnessGrowth As Integer)
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


    Private _stringInitializer As IInitializer(Of String) = New StringInitializer
    Private _stringMutator As IMutator(Of String) = New StringMutator
    Private _stringFitness As StringFitness = New StringFitness("Hello evolutionary world!")

    Public Sub Start() Implements IEvolutionStrategy(Of String).StartEvolution
        _solution = _stringInitializer.Initialize()

        Dim stepsWithoutFitnessGrowth As Integer = 0

        Do
            Dim oldfitness = _fitness

            Dim _mutantSolution = _stringMutator.Mutate(_solution)
            If _stringFitness.Fitness(_mutantSolution) >= _stringFitness.Fitness(_solution) Then
                _solution = _mutantSolution
            End If

            _fitness = _stringFitness.Fitness(_solution)
            RaiseEvent BestSolutionImproved(Me, New SolutionEventArgs(Of String)(_solution))

            If oldfitness = _fitness Then
                stepsWithoutFitnessGrowth += 1
            Else
                stepsWithoutFitnessGrowth = 0
            End If

        Loop Until _fitness >= 1 OrElse stepsWithoutFitnessGrowth >= _maxStepsWithoutFitnessGrowth
    End Sub

    Private _solution As String
    Public ReadOnly Property CurrentBestSolution() As String Implements IEvolutionStrategy(Of String).CurrentBestSolution
        Get
            Return _solution
        End Get
    End Property

    Private _fitness As Double
    Public ReadOnly Property CurrentBestFitness() As Double Implements IEvolutionStrategy(Of String).CurrentBestFitness
        Get
            Return _fitness
        End Get
    End Property
End Class
