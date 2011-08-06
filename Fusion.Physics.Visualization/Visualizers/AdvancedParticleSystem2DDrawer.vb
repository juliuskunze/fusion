Public Class AdvancedParticleSystem2DDrawer
    Implements IDrawer2D

    Public Sub New(graphics As Graphics)
        Me.New(Visualizer:=New Visualizer2D(graphics), particleSystem:=New ParticleSystem2D)
    End Sub

    Public Sub New(visualizer As Visualizer2D)
        Me.New(visualizer:=visualizer, particleSystem:=New ParticleSystem2D)
    End Sub

    Public Sub New(graphics As Graphics, particleSystem As ParticleSystem2D)
        Me.New(Visualizer:=New Visualizer2D(graphics), particleSystem:=particleSystem)
    End Sub

    Public Sub New(visualizer As Visualizer2D, particleSystem As ParticleSystem2D)
        Me.Visualizer = visualizer

        Me.CoordinateSystemDrawer = New CoordinateSystem2DDrawer(Me.Visualizer)
        Me.ParticleSystemDrawer = New ParticleSystem2DDrawer(Me.Visualizer, particleSystem)
        Me.FieldDrawer = New Field2DDrawer(Me.Visualizer, particles:=particleSystem.Particles)

        Me.ShowParticleSystem = True
        Me.ShowCoordinateSystem = True
        Me.ShowField = True
    End Sub

    Public Property Visualizer As Visualizer2D Implements IDrawer2D.Visualizer

    Public Property CoordinateSystemDrawer As CoordinateSystem2DDrawer
    Public Property ParticleSystemDrawer As ParticleSystem2DDrawer
    Public Property FieldDrawer As Field2DDrawer

    Public Property ShowParticleSystem As Boolean
    Public Property ShowCoordinateSystem As Boolean
    Public Property ShowField As Boolean

    Public Sub Draw() Implements IDrawer2D.Draw
        If Me.ShowCoordinateSystem Then Me.CoordinateSystemDrawer.Draw()
        If Me.ShowField Then Me.FieldDrawer.Draw()
        If Me.ShowParticleSystem Then Me.ParticleSystemDrawer.Draw()
    End Sub

End Class
