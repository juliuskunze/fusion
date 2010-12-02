Public Class ParticleSystem2DForm

    Private _serializer As ParticleSystem2DSerializer

    Private _sphereParticleDialog As SphereParticle2DDialog

    Private _particleSystemLoaded As Boolean
    Public Property ParticleSystemLoaded() As Boolean
        Get
            Return _particleSystemLoaded
        End Get
        Set(ByVal value As Boolean)
            _particleSystemLoaded = value

            closeMenuItem.Enabled = value
            saveMenuItem.Enabled = value
            startStopButton.Enabled = value
            startStopMenuItem.Enabled = value
            addParticleButton.Enabled = value
            addForceButton.Enabled = value
        End Set
    End Property

    Private _particleSystem As ParticleSystem2D
    Private _particleGuide As ParticleGuide
    Private _viewController As ViewController2D
    Private _drawer As AdvancedParticleSystem2DDrawer
    Private _renderer As Renderer2D
    Private _timer As FrameTimer

    Private Sub saveMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles saveMenuItem.Click
        If saveFileDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            _serializer.Serialize(_particleSystem, saveFileDialog.FileName)
        End If
    End Sub


    Private Sub loadParticleSystem(ByVal particleSystem As ParticleSystem2D)
        If _particleSystemLoaded Then
            closeParticleSystem()
        End If

        _particleSystem = particleSystem
        _drawer = New AdvancedParticleSystem2DDrawer(Graphics:=pictureBox.CreateGraphics, particleSystem:=particleSystem)
        _renderer = New Renderer2D(_drawer)

        _drawer.Visualizer.ProjectionMap = AffineMap2D.Scaling(1 / 10000000)
        _drawer.ShowField = True
        _drawer.FieldDrawer.Field.FieldType = New Electric2D()
        _drawer.FieldDrawer.VisualizationType = Field2DDrawer.VisualizationTypes.Fieldlines
        _drawer.FieldDrawer.FieldlineDrawer.FieldlinesPerCharge *= 4
        _drawer.FieldDrawer.ArrowGridDrawer.ArrowsCentered = True
        _drawer.FieldDrawer.ArrowGridDrawer.MultiColored = True

        _drawer.ParticleSystemDrawer.ShowForces = False

        _timer = New FrameTimer(framerate:=30, calcRate:=100, fastMotion:=2419200)
        _viewController = New ViewController2D(_drawer.Visualizer)
        _particleGuide = New ParticleGuide(_particleSystem)

        AddHandler _timer.FrameTick, AddressOf frameTick
        AddHandler pictureBox.MouseUp, AddressOf pictureBox_MouseUp
        AddHandler pictureBox.MouseMove, AddressOf pictureBox_MouseMove
        AddHandler pictureBox.MouseDown, AddressOf pictureBox_MouseDown
        AddHandler Me.MouseWheel, AddressOf form_MouseWheel
        AddHandler _drawer.Visualizer.MapChanged, AddressOf dragFixedParticleAndUpdateSystemDisplayIfTimerDisabled
        AddHandler _particleGuide.ParticleChanged, AddressOf updateSystemDisplayIfTimerDisabled

        Me.ParticleSystemLoaded = True
        selectedParticle = Nothing

        pictureBox.Focus()

        updateSystemDisplay()
    End Sub

    Private Sub frameTick(ByVal sender As Object, ByVal e As FrameTickEventArgs)
        For i = 0 To e.CalcsPerFrame - 1
            _particleSystem.DoEulerStep(timeSpan:=e.TimeStep)
            _particleGuide.TryStopFixedParticle()
        Next

        dragFixedParticleAndUpdateSystemDisplay()
    End Sub

    Private Sub closeMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles closeMenuItem.Click
        closeParticleSystem()
    End Sub

    Private Sub closeParticleSystem()
        If Not ParticleSystemLoaded Then
            Exit Sub
        End If

        Me.ParticleSystemLoaded = False
        selectedParticle = Nothing

        RemoveHandler _timer.FrameTick, AddressOf frameTick
        RemoveHandler pictureBox.MouseUp, AddressOf pictureBox_MouseUp
        RemoveHandler pictureBox.MouseMove, AddressOf pictureBox_MouseMove
        RemoveHandler pictureBox.MouseDown, AddressOf pictureBox_MouseDown
        RemoveHandler Me.MouseWheel, AddressOf form_MouseWheel
        RemoveHandler _drawer.Visualizer.MapChanged, AddressOf dragFixedParticleAndUpdateSystemDisplayIfTimerDisabled
        RemoveHandler _particleGuide.ParticleChanged, AddressOf updateSystemDisplayIfTimerDisabled

        _timer.Stop()
        _timer.Dispose()
        _particleSystem = Nothing
        _drawer = Nothing
        _viewController = Nothing

        timerEnabled = False

        pictureBox.Refresh()
    End Sub

    Private Property timerEnabled() As Boolean
        Get
            Return _timer.Enabled
        End Get
        Set(ByVal value As Boolean)
            If value Then
                _timer.Start()
                startStopButton.Text = "Stop"
                startStopMenuItem.Text = "Stop"
            Else
                _timer.Stop()
                startStopButton.Text = "Start"
                startStopMenuItem.Text = "Start"
            End If
        End Set
    End Property

    Private Sub dipoleMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dipoleMenuItem.Click
        _drawer.FieldDrawer.VisualizationType = Field2DDrawer.VisualizationTypes.Fieldlines
        _drawer.ShowField = True

        loadParticleSystem(ParticleSystems2D.Dipole(mass:=1, positiveCharge:=0.00000099999999999999995, negativeCharge:=-0.00000099999999999999995, distance:=1, radius:=0.050000000000000003))
    End Sub

    Private Sub moonEarthSystemMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles moonEarthSystemMenuItem.Click
        loadParticleSystem(ParticleSystems2D.MoonEarthSystem)
    End Sub

    Private Sub startStopButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles startStopButton.Click
        timerEnabled = Not timerEnabled
    End Sub

    Private Enum changeModes
        DragAndView
        SelectAndView
    End Enum
    Private _activeChangeMode As changeModes

    Private Sub pictureBox_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        Select Case _activeChangeMode
            Case changeModes.DragAndView
                Select Case e.Button
                    Case MouseButtons.Left
                        _particleGuide.FixNearestParticleTo(_viewController.SimulationMouseLocation)
                    Case MouseButtons.Middle
                        _viewController.StartRotate()
                End Select
            Case changeModes.SelectAndView
                Select Case e.Button
                    Case MouseButtons.Left
                        Const maxParticleSelectScreenRadius As Double = 0.02
                        selectedParticle = _particleSystem.Particles.NearestParticleInRadius(_drawer.Visualizer.InverseMap.Apply(New Vector2D(e.Location)), maxParticleSelectRadius:=_drawer.Visualizer.ProjectionMap.LinearMap.ZoomIn * maxParticleSelectScreenRadius)
                    Case MouseButtons.Middle
                        _viewController.StartRotate()
                End Select
        End Select
    End Sub

    Private Sub pictureBox_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        Dim oldSimulationMouseLocation = _viewController.SimulationMouseLocation
        Dim oldScreenMouseLocation = _viewController.ScreenMouseLocation
        _viewController.ScreenMouseLocation = New Vector2D(e.Location)

        Select Case _activeChangeMode
            Case changeModes.DragAndView
                Select Case e.Button
                    Case MouseButtons.Left
                        If Not timerEnabled Then
                            _particleGuide.TryDragFixedParticleTo(_viewController.SimulationMouseLocation)
                        End If
                    Case MouseButtons.Right
                        _viewController.Relocate(oldSimulationMouseLocation:=oldSimulationMouseLocation)
                    Case MouseButtons.Middle
                        _viewController.Rotate(oldScreenMouseLocation:=oldScreenMouseLocation)
                End Select
            Case changeModes.SelectAndView
                Select Case e.Button
                    Case MouseButtons.Right
                        _viewController.Relocate(oldSimulationMouseLocation:=oldSimulationMouseLocation)
                    Case MouseButtons.Middle
                        _viewController.Rotate(oldScreenMouseLocation:=oldScreenMouseLocation)
                End Select
        End Select
    End Sub

    Private Sub pictureBox_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        Select Case _activeChangeMode
            Case changeModes.DragAndView
                Select Case e.Button
                    Case Windows.Forms.MouseButtons.Left
                        _particleGuide.Unfix()
                    Case Windows.Forms.MouseButtons.Right
                End Select
            Case changeModes.SelectAndView
                Select Case e.Button
                    Case Windows.Forms.MouseButtons.Left
                End Select
        End Select
    End Sub



    Private Const _deltaPerMouseWheelStep As Double = 120
    Private Sub form_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        Dim mouseWheelSteps = e.Delta / _deltaPerMouseWheelStep
        _viewController.Zoom(zoomSteps:=mouseWheelSteps)
    End Sub

    Private Property selectedParticle() As Particle2D
        Get
            Return _drawer.ParticleSystemDrawer.SelectedParticle
        End Get
        Set(ByVal value As Particle2D)
            _drawer.ParticleSystemDrawer.SelectedParticle = value

            changeButton.Enabled = (value IsNot Nothing)
        End Set
    End Property

    Private Sub dragFixedParticleAndUpdateSystemDisplayIfTimerDisabled(ByVal sender As Object, ByVal e As EventArgs)
        If Not timerEnabled Then
            dragFixedParticleAndUpdateSystemDisplay()
        End If
    End Sub

    Private Sub dragFixedParticleAndUpdateSystemDisplay()
        _particleGuide.TryDragFixedParticleTo(_viewController.SimulationMouseLocation)
        updateSystemDisplay()
    End Sub

    Private Sub updateSystemDisplayIfTimerDisabled(ByVal sender As Object, ByVal e As EventArgs)
        If Not _timer.Enabled Then
            updateSystemDisplay()
        End If
    End Sub

    Private Sub updateSystemDisplay()
        _renderer.Render()
        updateEnergyAndMomentumLabel()
    End Sub

    Private Sub updateEnergyAndMomentumLabel()
        energyLabel.Text = _particleSystem.Energy.ToString("g4")
        momentumLabel.Text = _particleSystem.Momentum.ToString("g4")
    End Sub


    Private Sub startStopMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles startStopMenuItem.Click
        timerEnabled = Not timerEnabled
    End Sub

    Private Sub changeButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles changeButton.Click
        Dim selectedParticleRadius As Double = 0
        If TypeOf selectedParticle Is SphereParticle2D Then
            selectedParticleRadius = DirectCast(selectedParticle, SphereParticle2D).Radius
        Else
            selectedParticleRadius = 0
        End If

        _sphereParticleDialog = New SphereParticle2DDialog(New SphereParticle2D(selectedParticle, selectedParticleRadius))
        If _sphereParticleDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            selectedParticle.Mass = _sphereParticleDialog.Particle.Mass
            selectedParticle.Charge = _sphereParticleDialog.Particle.Charge
            selectedParticle.Location = _sphereParticleDialog.Particle.Location
            selectedParticle.Velocity = _sphereParticleDialog.Particle.Velocity
            selectedParticle.Color = _sphereParticleDialog.Particle.Color
            If TypeOf _sphereParticleDialog.Particle Is SphereParticle2D Then
                DirectCast(selectedParticle, SphereParticle2D).Radius = _sphereParticleDialog.Particle.Radius
            End If
        End If
    End Sub

    Private Sub addParticleButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles addParticleButton.Click
        If selectedParticle Is Nothing Then
            _sphereParticleDialog = New SphereParticle2DDialog
        Else
            Dim selectedParticleRadius As Double = 0
            If TypeOf selectedParticle Is SphereParticle2D Then
                selectedParticleRadius = DirectCast(selectedParticle, SphereParticle2D).Radius
            Else
                selectedParticleRadius = 0
            End If

            _sphereParticleDialog = New SphereParticle2DDialog(New SphereParticle2D(selectedParticle, selectedParticleRadius))
        End If
        If _sphereParticleDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            _particleSystem.AddNode(_sphereParticleDialog.Particle)
            selectedParticle = _particleSystem.Particles.Last
        End If
    End Sub

    Private Sub newMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles newMenuItem.Click
        loadParticleSystem(New ParticleSystem2D)
    End Sub

    Private Sub openMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles openMenuItem.Click
        openFileDialog.FileName = FileIO.SpecialDirectories.Desktop
        If openFileDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            Try
                loadParticleSystem(_serializer.Deserialize(openFileDialog.FileName))
            Catch ex As Exception
                MsgBox("This file can't be opened.", Title:="Loading failed")
            End Try
        End If
    End Sub

    Private Sub ParticleSystem2DForm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Space AndAlso _drawer.ParticleSystemDrawer.SelectedParticle IsNot Nothing Then
            _viewController.CenterSimulationLocation(_drawer.ParticleSystemDrawer.SelectedParticle.Location)
            e.Handled = True
        End If
    End Sub

    Private Sub form_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        _serializer = New ParticleSystem2DSerializer
        _sphereParticleDialog = New SphereParticle2DDialog

        Dim particleSystem = New ParticleSystem2D
        For i = 1 To 9
            particleSystem.AddNode(New SphereParticle2D(mass:=1, charge:=0.00000099999999999999995, Location:=New Vector2D(-0.10000000000000001, 0.040000000000000001 * i), velocity:=New Vector2D, Color:=Color.Yellow, radius:=0.01))
            particleSystem.AddNode(New SphereParticle2D(mass:=1, charge:=-0.000000099999999999999995, Location:=New Vector2D(0.10000000000000001, 0.040000000000000001 * i), velocity:=New Vector2D, Color:=Color.LimeGreen, radius:=0.01))
        Next
        particleSystem.AddNode(New SphereParticle2D(mass:=1, charge:=-0.000000099999999999999995, Location:=New Vector2D, velocity:=New Vector2D, Color:=Color.LimeGreen, radius:=0.01))

        particleSystem.ConnectEachParticleWithEachByForces(New Spring2D(springConstant:=0.5, length:=1))
        particleSystem.ConnectEachParticleWithEachByForces(New ElasticCollision2D(collisionSpringConstant:=1000))

        For Each particle In particleSystem.Particles
            particleSystem.SingleForces.Add(New SingleForce2D(New LinearFriction2D(1), particle))
        Next

        loadParticleSystem(particleSystem)
    End Sub

    Private Sub addForceButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles addForceButton.CheckedChanged
        If addForceButton.Checked Then

        End If
    End Sub

    Private Sub dragAndViewRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dragAndViewRadioButton.CheckedChanged
        If dragAndViewRadioButton.Checked Then
            _activeChangeMode = changeModes.DragAndView
        End If
    End Sub
    Private Sub selectAndViewRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles selectAndViewRadioButton.CheckedChanged
        If selectAndViewRadioButton.Checked Then
            _activeChangeMode = changeModes.SelectAndView
        End If
    End Sub

    Private Sub pictureBox_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles pictureBox.SizeChanged
        If _drawer IsNot Nothing Then
            _drawer.Visualizer.Graphics = pictureBox.CreateGraphics
        End If
    End Sub



End Class
