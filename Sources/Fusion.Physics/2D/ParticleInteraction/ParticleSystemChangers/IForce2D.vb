Public Interface IForce2D
    Inherits IParticleSystemChanger, IEdge(Of Particle2D)

    Property ForceGenerator As IForceGenerator2D
    ReadOnly Property ConnectedParticles As EndNodes(Of Particle2D)
    ReadOnly Property PotentialEnergy As Double

End Interface
