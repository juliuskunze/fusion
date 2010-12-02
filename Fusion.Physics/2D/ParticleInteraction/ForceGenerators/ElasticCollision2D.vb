<Serializable()>
Public Class ElasticCollision2D
    Implements IForceGenerator2D

    Public Property CollsionSpringConstant As Double

    Public Sub New(ByVal collisionSpringConstant As Double)
        Me.CollsionSpringConstant = collisionSpringConstant
    End Sub

    Public Function Force(ByVal targetParticle As Particle2D, ByVal causeParticle As Particle2D) As Math.Vector2D Implements IForceGenerator2D.Force
        If TypeOf targetParticle Is SphereParticle2D AndAlso TypeOf causeParticle Is SphereParticle2D Then
            Dim targetSphereParticle = DirectCast(targetParticle, SphereParticle2D)
            Dim causeSphereParticle = DirectCast(causeParticle, SphereParticle2D)

            Dim connection = targetParticle.Location - causeParticle.Location
            Dim distance = connection.Length
            Dim direction = connection / distance

            Dim deep As Double = collisionDeep(targetSphereParticle, causeSphereParticle)
            If deep > 0 Then
                Return CollsionSpringConstant * deep * direction
            Else
                Return New Vector2D
            End If
        Else
            Throw New ArgumentException("The particle arguments have to be sphere particles.")
        End If
    End Function

    Public Function PotentialEnergy(ByVal particle1 As Particle2D, ByVal particle2 As Particle2D) As Double Implements IForceGenerator2D.PotentialEnergy
        If TypeOf particle1 Is SphereParticle2D AndAlso TypeOf particle2 Is SphereParticle2D Then
            Dim sphereParticle1 = DirectCast(particle1, SphereParticle2D)
            Dim sphereParticle2 = DirectCast(particle1, SphereParticle2D)

            Dim deep As Double = collisionDeep(sphereParticle1, sphereParticle2)
            If deep > 0 Then
                Return 0.5 * CollsionSpringConstant * deep ^ 2
            Else
                Return 0
            End If
        Else
            Throw New ArgumentException("The particle arguments have to be sphere particles.")
        End If
    End Function

    Private Function collisionDeep(ByVal particle1 As SphereParticle2D, ByVal particle2 As SphereParticle2D) As Double
        Return particle1.Radius + particle2.Radius - (particle1.Location - particle2.Location).Length
    End Function

End Class
