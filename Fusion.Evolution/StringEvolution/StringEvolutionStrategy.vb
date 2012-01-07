Public Class StringEvolutionStrategy
    Implements IEvolutionStrategy(Of String)

    Public Event BestSolutionImproved(sender As Object, e As SolutionEventArgs(Of String)) Implements IEvolutionStrategy(Of String).BestSolutionImproved

    Public Sub New(maxStepsWithoutFitnessGrowth As Integer)
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


    Private ReadOnly _StringInitializer As IInitializer(Of String) = New StringInitializer
    Private ReadOnly _StringMutator As IMutator(Of String) = New StringMutator
    Private ReadOnly _StringFitness As StringFitness = New StringFitness("Hello evolutionary world!")

    Public Sub Start() Implements IEvolutionStrategy(Of String).StartEvolution
        _Solution = _StringInitializer.Initialize()

        Dim stepsWithoutFitnessGrowth As Integer = 0

        Do
            Dim oldfitness = _Fitness

            Dim mutantSolution = _StringMutator.Mutate(_Solution)
            If _StringFitness.Fitness(mutantSolution) >= _StringFitness.Fitness(_Solution) Then
                _Solution = mutantSolution
            End If

            _Fitness = _StringFitness.Fitness(_Solution)
            RaiseEvent BestSolutionImproved(Me, New SolutionEventArgs(Of String)(_Solution))

            If oldfitness = _Fitness Then
                stepsWithoutFitnessGrowth += 1
            Else
                stepsWithoutFitnessGrowth = 0
            End If

        Loop Until _Fitness >= 1 OrElse stepsWithoutFitnessGrowth >= _MaxStepsWithoutFitnessGrowth
    End Sub

    Private _Solution As String
    Public ReadOnly Property CurrentBestSolution() As String Implements IEvolutionStrategy(Of String).CurrentBestSolution
        Get
            Return _Solution
        End Get
    End Property

    Private _Fitness As Double
    Public ReadOnly Property CurrentBestFitness() As Double Implements IEvolutionStrategy(Of String).CurrentBestFitness
        Get
            Return _Fitness
        End Get
    End Property
End Class
