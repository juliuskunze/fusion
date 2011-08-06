<Serializable()>
Public Class SingleForce2D
    Implements ISingleForce2D

    Public Property Particle As Particle2D Implements ISingleForce2D.Particle
    Public Property SingleForce As ISingleForceGenerator2D Implements ISingleForce2D.SingleForce

    Public Sub New(singleForceGenerator As ISingleForceGenerator2D, particle As Particle2D)
        Me.SingleForce = singleForceGenerator
        Me.Particle = particle
    End Sub

    Public Sub AccelerateParticle(timeSpan As Double) Implements IParticleSystemChanger.ChangeSystem
        Me.Particle.Accelerate(timeSpan, Me.SingleForce.Force(Me.Particle))
    End Sub

End Class
