Public Class FieldEvolutionForm

    Private WithEvents _evolutionStrategy As FieldEvolutionStrategy

    Private _viewController As ViewController2D

    Private _drawer As ParticleSystem2DDrawer
    Private _renderer As Renderer2D


    Private Sub startButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles startButton.Click
        Dim fitness = New MultiFitness(Of ParticleField2D)
        fitness.FitnessFunctions.Add(New FieldFitnessTarget(Location:=New Vector2D(5, 5), targetField:=New Vector2D(10 ^ 5, 10 ^ 5)))
        'fitness.FitnessFunctions.Add(New FieldFitnessTarget(Location:=New Vector2D(0, 0), targetField:=New Vector2D(10 ^ 6, 10 ^ 6)))

        Dim startParticles = New List(Of Particle2D)
        startParticles.Add(New SphereParticle2D(mass:=1, charge:=1, Location:=New Vector2D(-5, -5), velocity:=New Vector2D, Color:=Drawing.Color.Red, radius:=0.1))
        'startParticles.Add(New SphereParticle2D(mass:=1, charge:=-1, Location:=New Vector2D(-4, -4), velocity:=New Vector2D, Color:=Drawing.Color.Blue, radius:=0.1))
        Dim initializer = New FieldInitializer(startParticles:=startParticles)
        Dim mutator = New FieldMutator(1)

        _evolutionStrategy = New FieldEvolutionStrategy(fitness, initializer, mutator)

        _drawer = New ParticleSystem2DDrawer(New Visualizer2D(pictureBox.CreateGraphics), New ParticleSystem2D(initializer.Initialize.Particles.ToList))
        _renderer = New Renderer2D(_drawer)

        _viewController = New ViewController2D(_drawer.Visualizer)

        _drawer.Visualizer.ProjectionMap = AffineMap2D.Scaling(1 / 10)

        pictureBox.Focus()

        _evolutionStrategy.StartEvolution()

        _renderer.Render()
    End Sub

    Private Sub startEvolutionButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles startEvolutionButton.Click
        _evolutionStrategy.Evolute()
    End Sub

    Private Sub evolutionStratedy_BestSolutionImproved(ByVal sender As Object, ByVal e As Evolution.SolutionEventArgs(Of ParticleField2D)) Handles _evolutionStrategy.BestSolutionImproved
        _drawer.ParticleSystem = New ParticleSystem2D(e.Solution.Particles.ToList)
        _renderer.Render()
        fitnessLabel.Text = _evolutionStrategy.CurrentBestFitness.ToString
    End Sub



    Private Sub pictureBox_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pictureBox.MouseMove
        If _viewController IsNot Nothing Then
            _viewController.ScreenMouseLocation = New Vector2D(e.Location)
        End If
    End Sub

    Private Const _deltaPerMouseWheelStep As Double = 120
    Private Sub FieldEvolutionForm_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseWheel
        If _viewController IsNot Nothing Then
            Dim mouseWheelSteps = e.Delta / _deltaPerMouseWheelStep
            _viewController.Zoom(zoomSteps:=mouseWheelSteps)
            _renderer.Render()
        End If
    End Sub
End Class