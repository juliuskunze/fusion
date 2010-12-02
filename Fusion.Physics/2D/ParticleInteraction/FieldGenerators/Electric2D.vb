<Serializable()>
Public Class Electric2D
    Implements IParticleFieldGenerator2D

    Public Function Potential(ByVal causeParticle As Particle2D, ByVal location As Vector2D) As Double Implements IParticleFieldGenerator2D.Potential
        Dim connection = causeParticle.Location - location
        Dim distance = connection.Length

        Return +(1 / (4 * PI * ElectricConstant)) * causeParticle.Charge / distance
    End Function

    Public Function Energy(ByVal potential As Double, ByVal targetParticle As Particle2D) As Double Implements IParticleFieldGenerator2D.PotentialEnergy
        Return potential * targetParticle.Charge
    End Function


    Public Function Field(ByVal causeParticle As Particle2D, ByVal location As Math.Vector2D) As Math.Vector2D Implements IParticleFieldGenerator2D.Field
        Dim connection = location - causeParticle.Location
        Dim distance = connection.Length

        Return +(1 / (4 * PI * ElectricConstant)) * causeParticle.Charge * connection / distance ^ 3
    End Function

    Public Function Force(ByVal field As Math.Vector2D, ByVal targetParticle As Particle2D) As Math.Vector2D Implements IParticleFieldGenerator2D.Force
        Return field * targetParticle.Charge
    End Function
End Class