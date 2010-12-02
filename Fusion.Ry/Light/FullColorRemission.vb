Public Class FullColorRemission
    Implements IColorRemission

    Public ReadOnly Property NoRemission As Boolean Implements IColorRemission.NoRemission
        Get
            Return False
        End Get
    End Property

    Public Function Color(ByVal startColor As Visualization.ExactColor) As Visualization.ExactColor Implements IColorRemission.Color
        Return startColor
    End Function
End Class
