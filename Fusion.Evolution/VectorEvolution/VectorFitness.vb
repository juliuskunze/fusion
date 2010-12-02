Public Class VectorFitness
    Implements IFitness(Of Vector2D)

    Private _targetVector As Vector2D
    Public Property TargetVector() As Vector2D
        Get
            Return _targetVector
        End Get
        Set(ByVal value As Vector2D)
            _targetVector = value
        End Set
    End Property

    Public Sub New(ByVal targetVector As Vector2D)
        _targetVector = targetVector
    End Sub

    Public Function Fitness(ByVal solution As Math.Vector2D) As Double Implements IFitness(Of Math.Vector2D).Fitness
        Return -((solution - _targetVector).Length)
    End Function
End Class
