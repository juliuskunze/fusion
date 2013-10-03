Public Interface IEvolutionStrategy(Of SolutionType)

    Event BestSolutionImproved(sender As Object, e As SolutionEventArgs(Of SolutionType))

    Sub StartEvolution()

    ReadOnly Property CurrentBestSolution() As SolutionType

    ReadOnly Property CurrentBestFitness() As Double

End Interface
