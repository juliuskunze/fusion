Public Interface IColorRemission

    Function Color(ByVal startColor As ExactColor) As ExactColor

    ReadOnly Property NoRemission() As Boolean

End Interface