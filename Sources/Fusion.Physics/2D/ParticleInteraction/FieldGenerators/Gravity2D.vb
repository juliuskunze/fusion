Public Class Gravity2D
    Implements IParticleFieldGenerator2D

    Public Function Field(causeParticle As Particle2D, location As Math.Vector2D) As Math.Vector2D Implements IParticleFieldGenerator2D.Field
        Dim connection = location - causeParticle.Location
        Dim distance = connection.Length

        Return -Constants.GravitationalConstant * causeParticle.Mass * connection / distance ^ 3
    End Function

    Public Function Force(field As Math.Vector2D, targetParticle As Particle2D) As Math.Vector2D Implements IParticleFieldGenerator2D.Force
        Return field * targetParticle.Mass
    End Function

    Public Function Potential(causeParticle As Particle2D, location As Math.Vector2D) As Double Implements IParticleFieldGenerator2D.Potential
        Dim connection = location - causeParticle.Location
        Dim distance = connection.Length
        Return -Constants.GravitationalConstant * causeParticle.Mass / distance
    End Function

    Public Function PotentialEnergy(potential As Double, targetParticle As Particle2D) As Double Implements IParticleFieldGenerator2D.PotentialEnergy
        Return potential * targetParticle.Mass
    End Function
End Class