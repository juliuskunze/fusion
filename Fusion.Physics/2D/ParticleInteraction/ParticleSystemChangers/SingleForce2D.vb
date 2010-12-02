<Serializable()>
Public Class SingleForce2D
    Implements ISingleForce2D

    Public Property Particle As Particle2D Implements ISingleForce2D.Particle
    Public Property SingleForce As ISingleForceGenerator2D Implements ISingleForce2D.SingleForce

    Public Sub New(ByVal singleForceGenerator As ISingleForceGenerator2D, ByVal particle As Particle2D)
        Me.SingleForce = singleForceGenerator
        Me.Particle = particle
    End Sub

    Public Sub AccelerateParticle(ByVal timeSpan As Double) Implements IParticleSystemChanger.ChangeSystem
        Me.Particle.Accelerate(timeSpan, Me.SingleForce.Force(Me.Particle))
    End Sub

End Class
