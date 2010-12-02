Public Class VectorInitializer
    Implements IInitializer(Of Vector2D)

    Public Function Initialize() As Math.Vector2D Implements IInitializer(Of Math.Vector2D).Initialize
        Return New Vector2D
    End Function
End Class
