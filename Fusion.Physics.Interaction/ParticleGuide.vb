Public Class ParticleGuide

    Public Sub New(particleSystem As ParticleSystem2D)
        _ParticleSystem = particleSystem
    End Sub

    Private ReadOnly _ParticleSystem As ParticleSystem2D
    Public ReadOnly Property ParticleSystem As ParticleSystem2D
        Get
            Return _ParticleSystem
        End Get
    End Property

    Public Property FixedParticle() As Particle2D

    Public Sub TryStopFixedParticle()
        If Me.FixedParticle IsNot Nothing Then
            Me.FixedParticle.Velocity = Vector2D.Zero

            RaiseEvent ParticleChanged(Me, Nothing)
        End If
    End Sub

    Public Sub FixNearestParticleTo(location As Vector2D)
        Me.FixedParticle = Me.ParticleSystem.Particles.NearestParticle(location)
        TryDragFixedParticleTo(location)

        RaiseEvent ParticleChanged(Me, Nothing)
    End Sub

    Private _LastDragStateLocation As Vector2D
    Private _LastDragStateElapsedTime As Double

    Public Sub TryDragFixedParticleTo(location As Vector2D)
        If Me.FixedParticle IsNot Nothing Then
            Me.FixedParticle.Location = location

            Dim timeDifference As Double = _ParticleSystem.Time - _LastDragStateElapsedTime
            If timeDifference = 0 Then
                Me.FixedParticle.Velocity = Vector2D.Zero
            Else
                Me.FixedParticle.Velocity = (location - _LastDragStateLocation) / timeDifference
            End If

            RaiseEvent ParticleChanged(Me, Nothing)
        End If
        _LastDragStateLocation = location
        _LastDragStateElapsedTime = _ParticleSystem.Time
    End Sub

    Public Sub Unfix()
        Me.FixedParticle = Nothing
    End Sub

    Public Event ParticleChanged(sender As Object, e As EventArgs)

End Class
