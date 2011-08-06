<Serializable()>
Public Class Force3D
    Implements IEdge(Of Particle3D)

    Public Property ParticlePairField As ParticlePairField3D

    Public Sub New(field As IField3D, endNodes As EndNodes(Of Particle3D))
        Me.New(field, endNodes, Color.Black)
    End Sub

    Public Sub New(field As IField3D, endNodes As EndNodes(Of Particle3D), color As Color)
        ParticlePairField = New ParticlePairField3D(field)
        _EndNodes = endNodes
        _Color = color
    End Sub

    Private _EndNodes As EndNodes(Of Particle3D)
    Public ReadOnly Property EndNodes() As Math.EndNodes(Of Particle3D) Implements Math.IEdge(Of Particle3D).EndNodes
        Get
            Return _EndNodes
        End Get
    End Property

    Public ReadOnly Property ForceOnParticle1() As Math.Vector3D
        Get
            Return Me.ParticlePairField.ForceOnParticle1(Me.EndNodes.Node1, Me.EndNodes.Node2)
        End Get
    End Property


    Public ReadOnly Property ForceOnParticle2() As Vector3D
        Get
            Return -ForceOnParticle1
        End Get
    End Property

    Public ReadOnly Property PotentialEnergy() As Double
        Get
            Return Me.ParticlePairField.PotentialEnergy(Me.EndNodes.Node1, Me.EndNodes.Node2)
        End Get
    End Property

    Public Sub AccelerateParticles(timeSpan As Double)
        Dim forceOnParticle1 = Me.ForceOnParticle1

        Me.EndNodes.Node1.Accelerate(timeSpan, forceOnParticle1)
        Me.EndNodes.Node2.Accelerate(timeSpan, -forceOnParticle1)
    End Sub

    Protected ReadOnly Property particleConnectionVector() As Vector3D
        Get
            Return Me.EndNodes.Node1.Location - Me.EndNodes.Node2.Location
        End Get
    End Property

    Protected ReadOnly Property particleDistance() As Double
        Get
            Return particleConnectionVector.Length
        End Get
    End Property

    Private _Color As Color
    Public Property Color() As Color
        Get
            Return _Color
        End Get
        Set(value As Color)
            _Color = value
        End Set
    End Property

    Public Property Pen() As Pen
        Get
            Return New Pen(Me.Color)
        End Get
        Set(value As Pen)
            Me.Color = value.Color
        End Set
    End Property

End Class

