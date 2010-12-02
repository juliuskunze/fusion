Public Interface IMutator(Of SolutionType)
    Function Mutate(ByVal solution As SolutionType) As SolutionType
End Interface
