<Serializable()>
Public Class ParticleSystem3D
    Inherits Graph(Of Particle3D, Force3D)

    Public Sub New()
        Me.New(Starttime:=New DateTime(0))
    End Sub

    Public Sub New(startTime As DateTime)
        _StartTime = startTime
    End Sub

    Private _StartTime As Date
    Private _ElapsedSeconds As Double

    Public ReadOnly Property ElapsedTime() As Double
        Get
            Return _ElapsedSeconds
        End Get
    End Property

    Public ReadOnly Property ElapsedTimeSpan() As TimeSpan
        Get
            Return TimeSpan.FromSeconds(ElapsedTime)
        End Get
    End Property

    Public ReadOnly Property CurrentTime() As DateTime
        Get
            Return _StartTime.AddSeconds(ElapsedTime)
        End Get
    End Property

    Public ReadOnly Property Starttime As Date
        Get
            Return _StartTime
        End Get
    End Property


    Public ReadOnly Property Particles() As ObjectModel.ReadOnlyCollection(Of Particle3D)
        Get
            Return MyBase.Nodes
        End Get
    End Property

    Public ReadOnly Property Forces() As ObjectModel.ReadOnlyCollection(Of Force3D)
        Get
            Return MyBase.Edges
        End Get
    End Property


    Public Sub DoEulerStep(timeSpan As Double)
        _ElapsedSeconds += timeSpan

        For Each force In Me.Edges
            force.AccelerateParticles(timeSpan)
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

End Class
