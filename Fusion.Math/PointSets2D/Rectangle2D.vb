Public Class Rectangle2D
    Implements IPointSet2D

    Public Property Size As Vector2D

    Public Sub New(ByVal size As Vector2D)
        Me.Size = size
    End Sub

    Public Function Contains(ByVal point As Vector2D) As Boolean Implements IPointSet2D.Contains
        Return point.X >= 0 AndAlso point.Y >= 0 AndAlso point.X <= Me.Size.X AndAlso point.Y <= Me.Size.Y
    End Function
End Class
