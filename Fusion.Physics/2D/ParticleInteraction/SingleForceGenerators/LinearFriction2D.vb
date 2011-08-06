<Serializable()>
Public Class LinearFriction2D
    Implements ISingleForceGenerator2D

    Public Property FrictionPerVelocity As Double

    Public Sub New(frictionConstant As Double)
        Me.FrictionPerVelocity = frictionConstant
    End Sub

    Public Function Force(particle As Particle2D) As Math.Vector2D Implements ISingleForceGenerator2D.Force
        Return -Me.FrictionPerVelocity * particle.Velocity
    End Function

End Class
