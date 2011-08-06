Public Class FieldEvolutionForm

    Private WithEvents _EvolutionStrategy As FieldEvolutionStrategy

    Private _ViewController As ViewController2D

    Private _Drawer As ParticleSystem2DDrawer
    Private _Renderer As Renderer2D


    Private Sub startButton_Click(sender As System.Object, e As System.EventArgs) Handles startButton.Click
        Dim fitness = New MultiFitness(Of ParticleField2D)
        fitness.FitnessFunctions.Add(New FieldFitnessTarget(Location:=New Vector2D(5, 5), targetField:=New Vector2D(10 ^ 5, 10 ^ 5)))
        'fitness.FitnessFunctions.Add(New FieldFitnessTarget(Location:=New Vector2D(0, 0), targetField:=New Vector2D(10 ^ 6, 10 ^ 6)))

        Dim startParticles = New List(Of Particle2D)
        startParticles.Add(New SphereParticle2D(mass:=1, charge:=1, Location:=New Vector2D(-5, -5), velocity:=New Vector2D, Color:=Drawing.Color.Red, radius:=0.1))
        'startParticles.Add(New SphereParticle2D(mass:=1, charge:=-1, Location:=New Vector2D(-4, -4), velocity:=New Vector2D, Color:=Drawing.Color.Blue, radius:=0.1))
        Dim initializer = New FieldInitializer(startParticles:=startParticles)
        Dim mutator = New FieldMutator(1)

        _EvolutionStrategy = New FieldEvolutionStrategy(fitness, initializer, mutator)

        _Drawer = New ParticleSystem2DDrawer(New Visualizer2D(pictureBox.CreateGraphics), New ParticleSystem2D(initializer.Initialize.Particles.ToList))
        _Renderer = New Renderer2D(_Drawer)

        _ViewController = New ViewController2D(_Drawer.Visualizer)

        _Drawer.Visualizer.ProjectionMap = AffineMap2D.Scaling(1 / 10)

        pictureBox.Focus()

        _EvolutionStrategy.StartEvolution()

        _Renderer.Render()
    End Sub

    Private Sub startEvolutionButton_Click(sender As Object, e As System.EventArgs) Handles startEvolutionButton.Click
        _EvolutionStrategy.Evolute()
    End Sub

    Private Sub evolutionStratedy_BestSolutionImproved(sender As Object, e As Evolution.SolutionEventArgs(Of ParticleField2D)) Handles _EvolutionStrategy.BestSolutionImproved
        _Drawer.ParticleSystem = New ParticleSystem2D(e.Solution.Particles.ToList)
        _Renderer.Render()
        fitnessLabel.Text = _EvolutionStrategy.CurrentBestFitness.ToString
    End Sub



    Private Sub pictureBox_MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles pictureBox.MouseMove
        If _ViewController IsNot Nothing Then
            _ViewController.ScreenMouseLocation = New Vector2D(e.Location)
        End If
    End Sub

    Private Const _DeltaPerMouseWheelStep As Double = 120
    Private Sub FieldEvolutionForm_MouseWheel(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseWheel
        If _ViewController IsNot Nothing Then
            Dim mouseWheelSteps = e.Delta / _DeltaPerMouseWheelStep
            _ViewController.Zoom(zoomSteps:=mouseWheelSteps)
            _Renderer.Render()
        End If
    End Sub
End Class