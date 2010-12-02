Public Class BlackColorRemission
    Implements IColorRemission

    Public ReadOnly Property NoRemission As Boolean Implements IColorRemission.NoRemission
        Get
            Return True
        End Get
    End Property

    Public Function Color(ByVal startColor As Visualization.ExactColor) As Visualization.ExactColor Implements IColorRemission.Color
        Return ExactColor.Black
    End Function
End Class
