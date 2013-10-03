Public Class HalfRing2D
    Implements IPointSet2D

    Public Property InnerRadius As Double
    Public Property Thickness As Double

    Public Sub New(innerRadius As Double, thickness As Double)
        Me.InnerRadius = innerRadius
        Me.Thickness = thickness
    End Sub

    Public Function Contains(point As Fusion.Math.Vector2D) As Boolean Implements IPointSet2D.Contains
        Return Me.InnerRadius <= point.Length AndAlso point.Length <= Me.InnerRadius + Me.Thickness AndAlso point.X >= 0
    End Function
End Class
