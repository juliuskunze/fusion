Public Class ParticleField2D
    Implements IField2D

    Public ReadOnly Property ForceType As FieldForceGenerator2D
        Get
            Return New FieldForceGenerator2D(Me.FieldType)
        End Get
    End Property

    Public Property FieldType As IParticleFieldGenerator2D

    Dim _Particles As IEnumerable(Of Particle2D)
    Public ReadOnly Property Particles() As IEnumerable(Of Particle2D)
        Get
            Return _Particles
        End Get
    End Property

    Public Sub New(ByVal forceType As IParticleFieldGenerator2D, ByVal particles As IEnumerable(Of Particle2D))
        Me.FieldType = forceType
        _Particles = particles
    End Sub

    Public Function Field(ByVal location As Math.Vector2D) As Vector2D Implements IField2D.Field
        Field = New Vector2D

        For Each particle In Particles
            Field += _FieldType.Field(particle, location)
        Next

        Return Field
    End Function

    Public Function Potential(ByVal location As Vector2D) As Double Implements IField2D.Potential
        Potential = 0

        For Each particle In Particles
            Potential += _FieldType.Potential(particle, location)
        Next

        Return Potential
    End Function

    Public Function Clone() As ParticleField2D
        Dim newParticleList = New List(Of Particle2D)

        For Each particle In Me.Particles
            Dim newParticle As Particle2D
            If TypeOf particle Is SphereParticle2D Then
                newParticle = (DirectCast(particle, SphereParticle2D)).Clone

            Else
                newParticle = particle.Clone
            End If
            newParticleList.Add(newParticle)
        Next

        Return New ParticleField2D(Me.ForceType.FieldGenerator, newParticleList)
    End Function

End Class
