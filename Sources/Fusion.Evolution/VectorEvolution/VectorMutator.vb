Public Class VectorMutator
    Implements IMutator(Of Vector2D)

    Private _StepLength As Double
    Public Property StepLength() As Double
        Get
            Return _StepLength
        End Get
        Set(value As Double)
            _StepLength = value
        End Set
    End Property

    Public Sub New(stepLength As Double)
        _StepLength = stepLength
    End Sub

    Public Function Mutate(solution As Math.Vector2D) As Math.Vector2D Implements IMutator(Of Math.Vector2D).Mutate
        Dim rnd = New Random
        Dim stepVector As Vector2D = Vector2D.FromLengthAndArgument(_StepLength, rnd.NextDouble() * 2 * PI)

        Return solution + stepVector
    End Function
End Class
