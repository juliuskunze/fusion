<Serializable()>
Public Class ParticleSystem2D
    Inherits Graph(Of Particle2D, IForce2D)

    Public Sub New()
        Me.New(StartTime:=New DateTime(0))
    End Sub

    Public Sub New(ByVal startTime As DateTime)
        Me.New(Particles:=New List(Of Particle2D), startTime:=startTime)
    End Sub

    Public Sub New(ByVal particles As List(Of Particle2D))
        Me.New(particles:=particles, StartTime:=New DateTime(0))
    End Sub

    Public Sub New(ByVal particles As List(Of Particle2D), ByVal startTime As DateTime)
        _StartTime = startTime
        Me.AddNodes(particles)
        Me.SingleForces = New List(Of ISingleForce2D)
    End Sub

    Private _StartTime As Date
    Private _ElapsedSeconds As Double

    Public ReadOnly Property Time() As Double
        Get
            Return _ElapsedSeconds
        End Get
    End Property

    Public ReadOnly Property ElapsedTimeSpan() As TimeSpan
        Get
            Return TimeSpan.FromSeconds(Time)
        End Get
    End Property

    Public ReadOnly Property CurrentTime() As DateTime
        Get
            Return _StartTime.AddSeconds(Time)
        End Get
    End Property

    Public ReadOnly Property StartTime As Date
        Get
            Return _StartTime
        End Get
    End Property


    Public ReadOnly Property Particles() As ObjectModel.ReadOnlyCollection(Of Particle2D)
        Get
            Return MyBase.Nodes
        End Get
    End Property

    Public ReadOnly Property Forces() As ObjectModel.ReadOnlyCollection(Of IForce2D)
        Get
            Return MyBase.Edges
        End Get
    End Property

    Public Property SingleForces As List(Of ISingleForce2D)

    Public Sub DoEulerStep(ByVal timeSpan As Double)
        _ElapsedSeconds += timeSpan

        For Each force In Me.Edges
            force.ChangeSystem(timeSpan)
        Next

        For Each singleForce In Me.SingleForces
            singleForce.ChangeSystem(timeSpan)
        Next

        For Each particle In Me.Nodes
            particle.Move(timeSpan)
        Next

    End Sub


    Public ReadOnly Property Energy() As Double
        Get
            Energy = 0

            For Each particle In Particles
                Energy += particle.KineticEngergy
            Next

            For Each force In Forces
                Energy += force.PotentialEnergy
            Next

            Return Energy
        End Get
    End Property

    Public ReadOnly Property Momentum() As Vector2D
        Get
            Momentum = New Vector2D

            For Each particle In Particles
                Momentum += particle.Momentum
            Next

            Return Momentum
        End Get
    End Property

    Public ReadOnly Property Mass() As Double
        Get
            Mass = 0

            For Each particle In Particles
                Mass += particle.Mass()
            Next

            Return Mass
        End Get
    End Property

    Public ReadOnly Property Charge() As Double
        Get
            Charge = 0

            For Each particle In Particles
                Charge += particle.Charge()
            Next

            Return Charge
        End Get
    End Property


    Public ReadOnly Property SumOfPositiveCharges() As Double
        Get
            SumOfPositiveCharges = 0

            For Each particle In Particles
                If particle.Charge > 0 Then
                    SumOfPositiveCharges += particle.Charge()
                End If

            Next

            Return SumOfPositiveCharges
        End Get
    End Property

    Public Sub ConnectEachParticleWithEachByForces(ByVal forceGenerator As IForceGenerator2D)
        Me.ConnectEachParticleWithEachByForces(Particles:=Me.Particles.ToList, forceGenerator:=forceGenerator)
    End Sub

    Public Sub ConnectEachParticleWithEachByForces(ByVal particles As List(Of Particle2D), ByVal forceGenerator As IForceGenerator2D)
        For particle1Index = 0 To particles.Count - 2
            For particle2Index = particle1Index + 1 To Me.Particles.Count - 1
                Me.AddEdge(New Force2D(forceGenerator, New EndNodes(Of Particle2D)(particles(particle1Index), particles(particle2Index))))
            Next
        Next
    End Sub

End Class
