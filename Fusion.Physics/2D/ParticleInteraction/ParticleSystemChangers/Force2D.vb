<Serializable()>
Public Class Force2D
    Implements IForce2D

    Public Property ForceGenerator As IForceGenerator2D Implements IForce2D.ForceGenerator

    Public Sub New(forceGenerator As IForceGenerator2D, particlesToConnect As EndNodes(Of Particle2D))
        Me.New(forceGenerator, particlesToConnect, New Pen(Color.Black))
    End Sub

    Public Sub New(forceGenerator As IForceGenerator2D, particlesToConnect As EndNodes(Of Particle2D), color As Color)
        Me.New(forceGenerator, particlesToConnect, New Pen(color))
    End Sub

    Public Sub New(forceGenerator As IForceGenerator2D, particlesToConnect As EndNodes(Of Particle2D), pen As Pen)
        Me.ForceGenerator = forceGenerator
        _ConnectedParticles = particlesToConnect
        Me.Pen = pen
    End Sub

    Private ReadOnly _ConnectedParticles As EndNodes(Of Particle2D)
    Public ReadOnly Property ConnectedParticles() As Math.EndNodes(Of Particle2D) Implements Math.IEdge(Of Particle2D).EndNodes, IForce2D.ConnectedParticles
        Get
            Return _ConnectedParticles
        End Get
    End Property

    Private ReadOnly Property ForceOnParticle1() As Vector2D
        Get
            Return Me.ForceGenerator.Force(targetParticle:=Me.ConnectedParticles.Node1, causeParticle:=Me.ConnectedParticles.Node2)
        End Get
    End Property

    Public ReadOnly Property PotentialEnergy() As Double Implements IForce2D.PotentialEnergy
        Get
            Return Me.ForceGenerator.PotentialEnergy(particle1:=Me.ConnectedParticles.Node1, particle2:=Me.ConnectedParticles.Node2)
        End Get
    End Property

    Public Sub AccelerateParticles(timeSpan As Double) Implements IParticleSystemChanger.ChangeSystem
        Dim forceOnParticle1 = Me.ForceOnParticle1

        Me.ConnectedParticles.Node1.Accelerate(timeSpan, forceOnParticle1)
        Me.ConnectedParticles.Node2.Accelerate(timeSpan, -forceOnParticle1)
    End Sub

    <NonSerialized()>
    Private _Pen As Pen
    Public Property Pen() As Pen
        Get
            Return _Pen
        End Get
        Set(value As Pen)
            _Pen = value
        End Set
    End Property
End Class