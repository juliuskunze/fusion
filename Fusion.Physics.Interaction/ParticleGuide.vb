Public Class ParticleGuide

    Public Sub New(ByVal particleSystem As ParticleSystem2D)
        _particleSystem = particleSystem
    End Sub

    Private _particleSystem As ParticleSystem2D
    Public ReadOnly Property ParticleSystem As ParticleSystem2D
        Get
            Return _particleSystem
        End Get
    End Property

    Public Property FixedParticle() As Particle2D

    Public Sub TryStopFixedParticle()
        If Me.FixedParticle IsNot Nothing Then
            Me.FixedParticle.Velocity = Vector2D.Zero

            RaiseEvent ParticleChanged(Me, Nothing)
        End If
    End Sub

    Public Sub FixNearestParticleTo(ByVal location As Vector2D)
        Me.FixedParticle = Me.ParticleSystem.Particles.NearestParticle(location)
        TryDragFixedParticleTo(location)

        RaiseEvent ParticleChanged(Me, Nothing)
    End Sub

    Private _lastDragStateLocation As Vector2D
    Private _lastDragStateElapsedTime As Double

    Public Sub TryDragFixedParticleTo(ByVal location As Vector2D)
        If Me.FixedParticle IsNot Nothing Then
            Me.FixedParticle.Location = location

            Dim timeDifference As Double = _particleSystem.Time - _lastDragStateElapsedTime
            If timeDifference = 0 Then
                Me.FixedParticle.Velocity = Vector2D.Zero
            Else
                Me.FixedParticle.Velocity = (location - _lastDragStateLocation) / timeDifference
            End If

            RaiseEvent ParticleChanged(Me, Nothing)
        End If
        _lastDragStateLocation = location
        _lastDragStateElapsedTime = _particleSystem.Time
    End Sub

    Public Sub Unfix()
        Me.FixedParticle = Nothing
    End Sub

    Public Event ParticleChanged(ByVal sender As Object, ByVal e As EventArgs)

End Class
