Public Class ParticleSystem2DForm

    Private _Serializer As ParticleSystem2DSerializer

    Private _SphereParticleDialog As SphereParticle2DDialog

    Private _ParticleSystemLoaded As Boolean
    Public Property ParticleSystemLoaded() As Boolean
        Get
            Return _ParticleSystemLoaded
        End Get
        Set(ByVal value As Boolean)
            _ParticleSystemLoaded = value

            closeMenuItem.Enabled = value
            saveMenuItem.Enabled = value
            startStopButton.Enabled = value
            startStopMenuItem.Enabled = value
            addParticleButton.Enabled = value
            addForceButton.Enabled = value
        End Set
    End Property

    Private _ParticleSystem As ParticleSystem2D
    Private _ParticleGuide As ParticleGuide
    Private _ViewController As ViewController2D
    Private _Drawer As AdvancedParticleSystem2DDrawer
    Private _Renderer As Renderer2D
    Private _Timer As FrameTimer

    Private Sub saveMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles saveMenuItem.Click
        If saveFileDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            _Serializer.Serialize(_ParticleSystem, saveFileDialog.FileName)
        End If
    End Sub


    Private Sub loadParticleSystem(ByVal particleSystem As ParticleSystem2D)
        If _ParticleSystemLoaded Then
            closeParticleSystem()
        End If

        _ParticleSystem = particleSystem
        _Drawer = New AdvancedParticleSystem2DDrawer(Graphics:=pictureBox.CreateGraphics, particleSystem:=particleSystem)
        _Renderer = New Renderer2D(_Drawer)

        _Drawer.Visualizer.ProjectionMap = AffineMap2D.Scaling(1 / 10000000)
        _Drawer.ShowField = True
        _Drawer.FieldDrawer.Field.FieldType = New Electric2D()
        _Drawer.FieldDrawer.VisualizationType = Field2DDrawer.VisualizationTypes.Fieldlines
        _Drawer.FieldDrawer.FieldlineDrawer.FieldlinesPerCharge *= 4
        _Drawer.FieldDrawer.ArrowGridDrawer.ArrowsCentered = True
        _Drawer.FieldDrawer.ArrowGridDrawer.MultiColored = True

        _Drawer.ParticleSystemDrawer.ShowForces = False

        _Timer = New FrameTimer(framerate:=30, calcRate:=100, fastMotion:=2419200)
        _ViewController = New ViewController2D(_Drawer.Visualizer)
        _ParticleGuide = New ParticleGuide(_ParticleSystem)

        AddHandler _Timer.FrameTick, AddressOf frameTick
        AddHandler pictureBox.MouseUp, AddressOf pictureBox_MouseUp
        AddHandler pictureBox.MouseMove, AddressOf pictureBox_MouseMove
        AddHandler pictureBox.MouseDown, AddressOf pictureBox_MouseDown
        AddHandler Me.MouseWheel, AddressOf form_MouseWheel
        AddHandler _Drawer.Visualizer.MapChanged, AddressOf dragFixedParticleAndUpdateSystemDisplayIfTimerDisabled
        AddHandler _ParticleGuide.ParticleChanged, AddressOf updateSystemDisplayIfTimerDisabled

        Me.ParticleSystemLoaded = True
        selectedParticle = Nothing

        pictureBox.Focus()

        updateSystemDisplay()
    End Sub

    Private Sub frameTick(ByVal sender As Object, ByVal e As FrameTickEventArgs)
        For i = 0 To e.CalcsPerFrame - 1
            _ParticleSystem.DoEulerStep(timeSpan:=e.TimeStep)
            _ParticleGuide.TryStopFixedParticle()
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

        RemoveHandler _Timer.FrameTick, AddressOf frameTick
        RemoveHandler pictureBox.MouseUp, AddressOf pictureBox_MouseUp
        RemoveHandler pictureBox.MouseMove, AddressOf pictureBox_MouseMove
        RemoveHandler pictureBox.MouseDown, AddressOf pictureBox_MouseDown
        RemoveHandler Me.MouseWheel, AddressOf form_MouseWheel
        RemoveHandler _Drawer.Visualizer.MapChanged, AddressOf dragFixedParticleAndUpdateSystemDisplayIfTimerDisabled
        RemoveHandler _ParticleGuide.ParticleChanged, AddressOf updateSystemDisplayIfTimerDisabled

        _Timer.Stop()
        _Timer.Dispose()
        _ParticleSystem = Nothing
        _Drawer = Nothing
        _ViewController = Nothing

        timerEnabled = False

        pictureBox.Refresh()
    End Sub

    Private Property timerEnabled() As Boolean
        Get
            Return _Timer.Enabled
        End Get
        Set(ByVal value As Boolean)
            If value Then
                _Timer.Start()
                startStopButton.Text = "Stop"
                startStopMenuItem.Text = "Stop"
            Else
                _Timer.Stop()
                startStopButton.Text = "Start"
                startStopMenuItem.Text = "Start"
            End If
        End Set
    End Property

    Private Sub dipoleMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dipoleMenuItem.Click
        _Drawer.FieldDrawer.VisualizationType = Field2DDrawer.VisualizationTypes.Fieldlines
        _Drawer.ShowField = True

        loadParticleSystem(ParticleSystems2D.Dipole(mass:=1, positiveCharge:=0.000001, negativeCharge:=-0.000001, distance:=1, radius:=0.05))
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
    Private _ActiveChangeMode As changeModes

    Private Sub pictureBox_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        Select Case _ActiveChangeMode
            Case changeModes.DragAndView
                Select Case e.Button
                    Case MouseButtons.Left
                        _ParticleGuide.FixNearestParticleTo(_ViewController.SimulationMouseLocation)
                    Case MouseButtons.Middle
                        _ViewController.StartRotate()
                End Select
            Case changeModes.SelectAndView
                Select Case e.Button
                    Case MouseButtons.Left
                        Const maxParticleSelectScreenRadius As Double = 0.02
                        selectedParticle = _ParticleSystem.Particles.NearestParticleInRadius(_Drawer.Visualizer.InverseMap.Apply(New Vector2D(e.Location)), maxParticleSelectRadius:=_Drawer.Visualizer.ProjectionMap.LinearMap.ZoomIn * maxParticleSelectScreenRadius)
                    Case MouseButtons.Middle
                        _ViewController.StartRotate()
                End Select
        End Select
    End Sub

    Private Sub pictureBox_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        Dim oldSimulationMouseLocation = _ViewController.SimulationMouseLocation
        Dim oldScreenMouseLocation = _ViewController.ScreenMouseLocation
        _ViewController.ScreenMouseLocation = New Vector2D(e.Location)

        Select Case _ActiveChangeMode
            Case changeModes.DragAndView
                Select Case e.Button
                    Case MouseButtons.Left
                        If Not timerEnabled Then
                            _ParticleGuide.TryDragFixedParticleTo(_ViewController.SimulationMouseLocation)
                        End If
                    Case MouseButtons.Right
                        _ViewController.Relocate(oldSimulationMouseLocation:=oldSimulationMouseLocation)
                    Case MouseButtons.Middle
                        _ViewController.Rotate(oldScreenMouseLocation:=oldScreenMouseLocation)
                End Select
            Case changeModes.SelectAndView
                Select Case e.Button
                    Case MouseButtons.Right
                        _ViewController.Relocate(oldSimulationMouseLocation:=oldSimulationMouseLocation)
                    Case MouseButtons.Middle
                        _ViewController.Rotate(oldScreenMouseLocation:=oldScreenMouseLocation)
                End Select
        End Select
    End Sub

    Private Sub pictureBox_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        Select Case _ActiveChangeMode
            Case changeModes.DragAndView
                Select Case e.Button
                    Case Windows.Forms.MouseButtons.Left
                        _ParticleGuide.Unfix()
                    Case Windows.Forms.MouseButtons.Right
                End Select
            Case changeModes.SelectAndView
                Select Case e.Button
                    Case Windows.Forms.MouseButtons.Left
                End Select
        End Select
    End Sub



    Private Const _DeltaPerMouseWheelStep As Double = 120
    Private Sub form_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        Dim mouseWheelSteps = e.Delta / _DeltaPerMouseWheelStep
        _ViewController.Zoom(zoomSteps:=mouseWheelSteps)
    End Sub

    Private Property selectedParticle() As Particle2D
        Get
            Return _Drawer.ParticleSystemDrawer.SelectedParticle
        End Get
        Set(ByVal value As Particle2D)
            _Drawer.ParticleSystemDrawer.SelectedParticle = value

            changeButton.Enabled = (value IsNot Nothing)
        End Set
    End Property

    Private Sub dragFixedParticleAndUpdateSystemDisplayIfTimerDisabled(ByVal sender As Object, ByVal e As EventArgs)
        If Not timerEnabled Then
            dragFixedParticleAndUpdateSystemDisplay()
        End If
    End Sub

    Private Sub dragFixedParticleAndUpdateSystemDisplay()
        _ParticleGuide.TryDragFixedParticleTo(_ViewController.SimulationMouseLocation)
        updateSystemDisplay()
    End Sub

    Private Sub updateSystemDisplayIfTimerDisabled(ByVal sender As Object, ByVal e As EventArgs)
        If Not _Timer.Enabled Then
            updateSystemDisplay()
        End If
    End Sub

    Private Sub updateSystemDisplay()
        _Renderer.Render()
        updateEnergyAndMomentumLabel()
    End Sub

    Private Sub updateEnergyAndMomentumLabel()
        energyLabel.Text = _ParticleSystem.Energy.ToString("g4")
        momentumLabel.Text = _ParticleSystem.Momentum.ToString("g4")
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

        _SphereParticleDialog = New SphereParticle2DDialog(New SphereParticle2D(selectedParticle, selectedParticleRadius))
        If _SphereParticleDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            selectedParticle.Mass = _SphereParticleDialog.Particle.Mass
            selectedParticle.Charge = _SphereParticleDialog.Particle.Charge
            selectedParticle.Location = _SphereParticleDialog.Particle.Location
            selectedParticle.Velocity = _SphereParticleDialog.Particle.Velocity
            selectedParticle.Color = _SphereParticleDialog.Particle.Color
            If TypeOf _SphereParticleDialog.Particle Is SphereParticle2D Then
                DirectCast(selectedParticle, SphereParticle2D).Radius = _SphereParticleDialog.Particle.Radius
            End If
        End If
    End Sub

    Private Sub addParticleButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles addParticleButton.Click
        If selectedParticle Is Nothing Then
            _SphereParticleDialog = New SphereParticle2DDialog
        Else
            Dim selectedParticleRadius As Double = 0
            If TypeOf selectedParticle Is SphereParticle2D Then
                selectedParticleRadius = DirectCast(selectedParticle, SphereParticle2D).Radius
            Else
                selectedParticleRadius = 0
            End If

            _SphereParticleDialog = New SphereParticle2DDialog(New SphereParticle2D(selectedParticle, selectedParticleRadius))
        End If
        If _SphereParticleDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            _ParticleSystem.AddNode(_SphereParticleDialog.Particle)
            selectedParticle = _ParticleSystem.Particles.Last
        End If
    End Sub

    Private Sub newMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles newMenuItem.Click
        loadParticleSystem(New ParticleSystem2D)
    End Sub

    Private Sub openMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles openMenuItem.Click
        openFileDialog.FileName = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
        If openFileDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            Try
                loadParticleSystem(_Serializer.Deserialize(openFileDialog.FileName))
            Catch ex As Exception
                MessageBox.Show(text:="This file can't be opened.", caption:="Loading failed")
            End Try
        End If
    End Sub

    Private Sub ParticleSystem2DForm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Space AndAlso _Drawer.ParticleSystemDrawer.SelectedParticle IsNot Nothing Then
            _ViewController.CenterSimulationLocation(_Drawer.ParticleSystemDrawer.SelectedParticle.Location)
            e.Handled = True
        End If
    End Sub

    Private Sub form_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        _Serializer = New ParticleSystem2DSerializer
        _SphereParticleDialog = New SphereParticle2DDialog

        Dim particleSystem = New ParticleSystem2D
        For i = 1 To 9
            particleSystem.AddNode(New SphereParticle2D(mass:=1, charge:=0.000001, Location:=New Vector2D(-0.1, 0.04 * i), velocity:=New Vector2D, Color:=Color.Yellow, radius:=0.01))
            particleSystem.AddNode(New SphereParticle2D(mass:=1, charge:=-0.0000001, Location:=New Vector2D(0.1, 0.04 * i), velocity:=New Vector2D, Color:=Color.LimeGreen, radius:=0.01))
        Next
        particleSystem.AddNode(New SphereParticle2D(mass:=1, charge:=-0.0000001, Location:=New Vector2D, velocity:=New Vector2D, Color:=Color.LimeGreen, radius:=0.01))

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
            _ActiveChangeMode = changeModes.DragAndView
        End If
    End Sub
    Private Sub selectAndViewRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles selectAndViewRadioButton.CheckedChanged
        If selectAndViewRadioButton.Checked Then
            _ActiveChangeMode = changeModes.SelectAndView
        End If
    End Sub

    Private Sub pictureBox_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles pictureBox.SizeChanged
        If _Drawer IsNot Nothing Then
            _Drawer.Visualizer.Graphics = pictureBox.CreateGraphics
        End If
    End Sub



End Class
