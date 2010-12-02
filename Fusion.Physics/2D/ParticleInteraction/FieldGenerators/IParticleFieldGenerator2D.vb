Public Interface IParticleFieldGenerator2D

    Function Field(ByVal causeParticle As Particle2D, ByVal location As Vector2D) As Vector2D
    Function Force(ByVal field As Vector2D, ByVal targetParticle As Particle2D) As Vector2D

    Function Potential(ByVal causeParticle As Particle2D, ByVal location As Vector2D) As Double
    Function PotentialEnergy(ByVal potential As Double, ByVal targetParticle As Particle2D) As Double

End Interface
