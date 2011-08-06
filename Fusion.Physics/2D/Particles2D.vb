Public Module Particles2D
    <Runtime.CompilerServices.Extension()> Public Function NearestParticleInRadius(particles As IEnumerable(Of Particle2D), location As Vector2D, maxParticleSelectRadius As Double) As Particle2D
        Dim particle = NearestParticle(particles, location)
        If particle Is Nothing Then
            Return Nothing
        End If

        If (particle.Location - location).LengthSquared <= maxParticleSelectRadius ^ 2 Then
            Return particle
        Else
            Return Nothing
        End If
    End Function

    <Runtime.CompilerServices.Extension()> Public Function NearestParticle(particles As IEnumerable(Of Particle2D), location As Vector2D) As Particle2D
        If particles.Count = 0 Then
            Return Nothing
        End If

        NearestParticle = particles.First
        Dim nearestParticleDistanceSquared = Double.PositiveInfinity
        For Each particle In particles
            Dim distanceSquared = (location - particle.Location).LengthSquared
            If distanceSquared < nearestParticleDistanceSquared Then
                nearestParticleDistanceSquared = distanceSquared
                NearestParticle = particle
            End If
        Next

        Return NearestParticle
    End Function

    <Runtime.CompilerServices.Extension()> Public Function NearestNegativeParticle(particles As IEnumerable(Of Particle2D), location As Vector2D) As Particle2D
        Dim negativeParticles As List(Of Particle2D) = (From particle In particles Where particle.Charge < 0).ToList

        Return negativeParticles.NearestParticle(location)
    End Function
End Module

