Public Class ParticleSystem2DDrawer
    Implements IDrawer2D

    Public Sub New(ByVal visualizer As Visualizer2D, ByVal particleSystem As ParticleSystem2D)
        Me.Visualizer = visualizer
        Me.ParticleSystem = particleSystem

        Me.SelectedParticle = Nothing
        Me.SelectedParticleCirclePen = New Pen(Color.White)

        Me.ShowParticles = True
        Me.ShowForces = True
    End Sub

    Public Property Visualizer As Visualizer2D Implements IDrawer2D.Visualizer
    Public Property ParticleSystem() As ParticleSystem2D

    Public Property ShowParticles As Boolean
    Public Property ShowForces As Boolean

    Public Event Changed(ByVal sender As Object, ByVal e As EventArgs)

    Public Sub Draw() Implements IDrawer2D.Draw
        If Me.ShowForces Then drawForces()
        If Me.ShowParticles Then drawParticles()
        If Me.SelectedParticle IsNot Nothing Then drawSelectedParticleCircle()
    End Sub


    Public Property DefaultParticleScreenRadiusInPixels As Double = 0.003
    Public Property MinimalParticleScreenRadiusInPixels As Double = 0.0005

    Private Sub drawParticles()
        For Each particle In Me.ParticleSystem.Particles
                drawParticle(particle)
        Next
    End Sub

    Private Sub drawParticle(ByVal particle As Particle2D)
        If TypeOf particle Is SphereParticle2D Then
            Dim sphereParticle As SphereParticle2D = DirectCast(particle, SphereParticle2D)
            Me.Visualizer.DrawingGraphics.FillEllipse(New SolidBrush(particle.Color), generateSphereParticleRect(sphereParticle))
        Else
            Me.Visualizer.DrawingGraphics.FillEllipse(New SolidBrush(particle.Color), generateParticleRect(particle, Me.DefaultParticleScreenRadiusInPixels))
        End If
    End Sub


    Private Sub drawForces()
        For Each force In Me.ParticleSystem.Forces
            drawForce(DirectCast(force, Force2D))
        Next
    End Sub

    Private Sub drawForce(ByVal force As Force2D)
        Me.Visualizer.DrawingGraphics.DrawLine(force.Pen, Me.Visualizer.Map.Apply(force.ConnectedParticles.Node1.Location).ToPointF, Me.Visualizer.Map.Apply(force.ConnectedParticles.Node2.Location).ToPointF)
    End Sub

    Public Property SelectedParticle() As Particle2D
    Public Property SelectedParticleCirclePen() As Pen
    Public Property SelectedParticleCircleScreenRadiusInPixels As Double = 15

    Public Sub drawSelectedParticleCircle()
        Dim circleRadiusVector = New Vector2D(SelectedParticleCircleScreenRadiusInPixels, SelectedParticleCircleScreenRadiusInPixels)
        Dim particleScreenLocation = Me.Visualizer.Map.Apply(SelectedParticle.Location)

        Me.Visualizer.DrawingGraphics.DrawEllipse(Me.SelectedParticleCirclePen, New RectangleF((particleScreenLocation - circleRadiusVector).ToPointF, (2 * circleRadiusVector).ToSizeF))
    End Sub


    Private Function generateSphereParticleRect(ByVal particle As SphereParticle2D) As RectangleF
        Return generateParticleRect(particle, Me.Visualizer.ProjectionMap.LinearMap.ZoomOut * particle.Radius)
    End Function

    Private Function generateParticleRect(ByVal particle As Particle2D, ByVal screenRadius As Double) As RectangleF
        screenRadius = Max(Me.MinimalParticleScreenRadiusInPixels, screenRadius)
        Return Me.Visualizer.generateScreenRadiusCircleRect(particle.Location, screenRadius)
    End Function

End Class
