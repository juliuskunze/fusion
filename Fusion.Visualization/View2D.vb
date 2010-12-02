Public Class View2D
    Implements IMap2D

    Public Property Map As AffineMap2D

    Public Sub New(ByVal map As AffineMap2D)
        Me.Map = map
    End Sub

    Public Function Apply(ByVal v As Math.Vector2D) As Math.Vector2D Implements Math.IMap2D.Apply
        Return Me.Map.Apply(v)
    End Function
End Class
