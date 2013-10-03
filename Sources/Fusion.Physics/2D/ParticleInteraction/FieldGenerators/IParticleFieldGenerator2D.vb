Public Interface IParticleFieldGenerator2D

    Function Field(causeParticle As Particle2D, location As Vector2D) As Vector2D
    Function Force(field As Vector2D, targetParticle As Particle2D) As Vector2D

    Function Potential(causeParticle As Particle2D, location As Vector2D) As Double
    Function PotentialEnergy(potential As Double, targetParticle As Particle2D) As Double

End Interface
