Public Class ScaledColorRemission
    Implements ILightRemission(Of ExactColor)

    Public Property Albedo As Double

    Public Sub New(ByVal albedo As Double)
        Me.Albedo = albedo
    End Sub

    Public Function Color(ByVal startColor As ExactColor) As ExactColor Implements ILightRemission(Of ExactColor).GetRemission
        Return Me.Albedo * startColor
    End Function

    Public ReadOnly Property NoRemission As Boolean Implements ILightRemission(Of ExactColor).NoRemission
        Get
            Return Me.Albedo = 0
        End Get
    End Property
End Class
