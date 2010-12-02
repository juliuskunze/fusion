Public Interface IFitness(Of SolutionType)
    Function Fitness(ByVal solution As SolutionType) As Double
End Interface