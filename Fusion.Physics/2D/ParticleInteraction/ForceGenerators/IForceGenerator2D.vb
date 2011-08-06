Public Interface IForceGenerator2D
    Function Force(targetParticle As Particle2D, causeParticle As Particle2D) As Vector2D
    Function PotentialEnergy(particle1 As Particle2D, particle2 As Particle2D) As Double
End Interface
