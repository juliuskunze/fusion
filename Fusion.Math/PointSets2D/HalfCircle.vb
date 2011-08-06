Public Class HalfCircle
    Implements IPointSet2D

    Public Property Radius As Double

    Public Sub New(radius As Double)
        Me.Radius = radius
    End Sub

    Public Function Contains(point As Fusion.Math.Vector2D) As Boolean Implements IPointSet2D.Contains
        Return point.Length <= Me.Radius AndAlso point.X <= 0
    End Function
End Class
