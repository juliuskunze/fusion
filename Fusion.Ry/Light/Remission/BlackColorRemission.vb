Public Class BlackColorRemission
    Implements IRemission(Of ExactColor)

    Public ReadOnly Property NoRemission As Boolean Implements IRemission(Of ExactColor).NoRemission
        Get
            Return True
        End Get
    End Property

    Public Function Color(ByVal startColor As Visualization.ExactColor) As Visualization.ExactColor Implements IRemission(Of ExactColor).Remission
        Return ExactColor.Black
    End Function
End Class
