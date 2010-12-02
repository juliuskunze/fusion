<Serializable()>
Public Class ParticlePairField3D

    Public Property Field As IField3D

    Public Sub New(ByVal field As IField3D)
        Me.Field = field
    End Sub

    Public Function ForceOnParticle1(ByVal particle1 As Particle3D, ByVal particle2 As Particle3D) As Vector3D
        Return Me.Field.Force(Me.Field.Field(particle2, particle1.Location), particle1)
    End Function

    Public Function PotentialEnergy(ByVal particle1 As Particle3D, ByVal particle2 As Particle3D) As Double
        Return Me.Field.PotentialEnergy(Me.Field.Potential(particle2, particle1.Location), particle1)
    End Function

End Class
