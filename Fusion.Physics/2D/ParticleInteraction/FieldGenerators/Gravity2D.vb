Public Class Gravity2D
    Implements IParticleFieldGenerator2D

    Public Function Field(ByVal causeParticle As Particle2D, ByVal location As Math.Vector2D) As Math.Vector2D Implements IParticleFieldGenerator2D.Field
        Dim connection = location - causeParticle.Location
        Dim distance = connection.Length

        Return -Constants.GravitationalConstant * causeParticle.Mass * connection / distance ^ 3
    End Function

    Public Function Force(ByVal field As Math.Vector2D, ByVal targetParticle As Particle2D) As Math.Vector2D Implements IParticleFieldGenerator2D.Force
        Return field * targetParticle.Mass
    End Function

    Public Function Potential(ByVal causeParticle As Particle2D, ByVal location As Math.Vector2D) As Double Implements IParticleFieldGenerator2D.Potential
        Dim connection = location - causeParticle.Location
        Dim distance = connection.Length
        Return -Constants.GravitationalConstant * causeParticle.Mass / distance
    End Function

    Public Function PotentialEnergy(ByVal potential As Double, ByVal targetParticle As Particle2D) As Double Implements IParticleFieldGenerator2D.PotentialEnergy
        Return potential * targetParticle.Mass
    End Function
End Class