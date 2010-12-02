Public Class ScaledColorRemission
    Implements IColorRemission

    Public Property Albedo As Double

    Public Sub New(ByVal albedo As Double)
        Me.Albedo = albedo
    End Sub

    Public Function Color(ByVal startColor As ExactColor) As ExactColor Implements IColorRemission.Color
        Return Me.Albedo * startColor
    End Function

    Public ReadOnly Property NoRemission As Boolean Implements IColorRemission.NoRemission
        Get
            Return Me.Albedo = 0
        End Get
    End Property
End Class
