Public Class Circle2D
    Implements IPointSet2D

    Public Property Radius As Double

    Public Sub New(radius As Double)
        Me.Radius = radius
    End Sub

    Public Function Contains(point As Fusion.Math.Vector2D) As Boolean Implements IPointSet2D.Contains
        Return Me.Radius >= point.Length
    End Function
End Class
