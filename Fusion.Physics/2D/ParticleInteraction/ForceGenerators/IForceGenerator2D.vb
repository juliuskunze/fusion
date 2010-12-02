Public Interface IForceGenerator2D
    Function Force(ByVal targetParticle As Particle2D, ByVal causeParticle As Particle2D) As Vector2D
    Function PotentialEnergy(ByVal particle1 As Particle2D, ByVal particle2 As Particle2D) As Double
End Interface
