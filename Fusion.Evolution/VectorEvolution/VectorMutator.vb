Public Class VectorMutator
    Implements IMutator(Of Vector2D)

    Private _stepLength As Double
    Public Property StepLength() As Double
        Get
            Return _stepLength
        End Get
        Set(ByVal value As Double)
            _stepLength = value
        End Set
    End Property

    Public Sub New(ByVal stepLength As Double)
        _stepLength = stepLength
    End Sub

    Public Function Mutate(ByVal solution As Math.Vector2D) As Math.Vector2D Implements IMutator(Of Math.Vector2D).Mutate
        Dim rnd = New Random
        Dim stepVector As Vector2D = Vector2D.FromLengthAndArgument(_stepLength, rnd.NextDouble() * 2 * PI)

        Return solution + stepVector
    End Function
End Class
