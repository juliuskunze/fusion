Public Interface IFitness(Of SolutionType)
    Function Fitness(solution As SolutionType) As Double
End Interface