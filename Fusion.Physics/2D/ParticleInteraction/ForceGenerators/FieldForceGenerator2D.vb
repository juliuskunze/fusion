<Serializable()>
Public Class FieldForceGenerator2D
    Implements IForceGenerator2D

    Public Property FieldGenerator As IParticleFieldGenerator2D

    Public Sub New(ByVal fieldGenerator As IParticleFieldGenerator2D)
        Me.FieldGenerator = fieldGenerator
    End Sub

    Public Function Force(ByVal targetParticle As Particle2D, ByVal causeParticle As Particle2D) As Vector2D Implements IForceGenerator2D.Force
        Return Me.FieldGenerator.Force(Me.FieldGenerator.Field(causeParticle, targetParticle.Location), targetParticle)
    End Function

    Public Function PotentialEnergy(ByVal particle1 As Particle2D, ByVal particle2 As Particle2D) As Double Implements IForceGenerator2D.PotentialEnergy
        Return Me.FieldGenerator.PotentialEnergy(Me.FieldGenerator.Potential(particle2, particle1.Location), particle1)
    End Function
End Class
