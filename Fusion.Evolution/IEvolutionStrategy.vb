Public Interface IEvolutionStrategy(Of SolutionType)

    Event BestSolutionImproved(ByVal sender As Object, ByVal e As SolutionEventArgs(Of SolutionType))

    Sub StartEvolution()

    ReadOnly Property CurrentBestSolution() As SolutionType

    ReadOnly Property CurrentBestFitness() As Double

End Interface
