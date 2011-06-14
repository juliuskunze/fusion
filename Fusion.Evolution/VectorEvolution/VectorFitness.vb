Public Class VectorFitness
    Implements IFitness(Of Vector2D)

    Private _TargetVector As Vector2D
    Public Property TargetVector() As Vector2D
        Get
            Return _TargetVector
        End Get
        Set(ByVal value As Vector2D)
            _TargetVector = value
        End Set
    End Property

    Public Sub New(ByVal targetVector As Vector2D)
        _TargetVector = targetVector
    End Sub

    Public Function Fitness(ByVal solution As Math.Vector2D) As Double Implements IFitness(Of Math.Vector2D).Fitness
        Return -((solution - _TargetVector).Length)
    End Function
End Class
