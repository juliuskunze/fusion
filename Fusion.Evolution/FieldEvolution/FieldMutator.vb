Public Class FieldMutator
    Implements IMutator(Of ParticleField2D)

    Public Sub New()
        Me.New(MoveStepLength:=1)
    End Sub

    Public Sub New(ByVal moveStepLength As Double)
        _random = New Random
        _moveStepLength = moveStepLength
    End Sub

    Private _random As Random

    Private _moveStepLength As Double
    Public Property MoveStepLength() As Double
        Get
            Return _moveStepLength
        End Get
        Set(ByVal value As Double)
            _moveStepLength = value
        End Set
    End Property

    Public Function Mutate(ByVal solution As Physics.ParticleField2D) As Physics.ParticleField2D Implements IMutator(Of Physics.ParticleField2D).Mutate
        Dim mutatedSolution = solution.Clone


        Dim particleToChange = mutatedSolution.Particles(_random.Next(mutatedSolution.Particles.Count))
        moveParticle(particleToChange)

        Return mutatedSolution
    End Function

    Private Sub moveParticle(ByVal particle As Particle2D)
        Dim directionAngle As Double = 2 * PI * _random.NextDouble
        Dim stepVector As Vector2D = Vector2D.FromLengthAndArgument(length:=Me.MoveStepLength, argument:=directionAngle)

        particle.Location += stepVector
    End Sub
End Class
