Public Class FullColorRemission
    Implements ILightRemission(Of ExactColor)

    Public ReadOnly Property NoRemission As Boolean Implements ILightRemission(Of ExactColor).NoRemission
        Get
            Return False
        End Get
    End Property

    Public Function Color(ByVal startColor As ExactColor) As ExactColor Implements ILightRemission(Of ExactColor).GetRemission
        Return startColor
    End Function
End Class
