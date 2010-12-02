Public Class FlowForm

    Private _flowPanel As FlowPanel2D
    Private WithEvents _timer As FrameTimer

    Private _graphics As Graphics
    Private _visualizer As Visualizer2D
    Private _drawer As FlowDrawer2D(Of FlowBox2D)
    Private WithEvents _viewController As ViewController2D
    Private _renderer As Renderer2D

    Private _paraviewWriter As FlowPanel2DParaviewWriter(Of FlowBox2D)

    Private Sub PointSet2DPlotterForm_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseClick
        _renderer.Render()
    End Sub

    Private Sub pictureBox_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pictureBox.MouseDown
        Select Case e.Button
            Case MouseButtons.Left
                setWallAtScreenLocation(New Vector2D(e.Location))
            Case MouseButtons.Middle
                _viewController.StartRotate()
        End Select
    End Sub

    Private Sub setWallAtScreenLocation(ByVal screenLocation As Vector2D)
        Dim simulationLocation = _visualizer.InverseMap.Apply(screenLocation)
        Dim arrayLocation = (simulationLocation - _flowPanel.LowerVertex) / _flowPanel.GridLength
        Dim columnIndex = CInt(arrayLocation.X)
        Dim rowIndex = CInt(arrayLocation.Y)
        If 0 <= columnIndex AndAlso columnIndex < _flowPanel.ColumnCount AndAlso 0 <= rowIndex AndAlso rowIndex < _flowPanel.RowCount Then
            _flowPanel.Array(columnIndex, rowIndex).IsWall = True
        End If
    End Sub

    Private Sub PointSet2DPlotterForm_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseWheel
        If _drawer IsNot Nothing Then
            _viewController.Zoom(zoomSteps:=e.Delta / 120)
            _renderer.Render()
        End If
    End Sub

    Private Sub pictureBox_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pictureBox.MouseMove
        If _drawer IsNot Nothing Then
            Dim oldScreenMouseLocation = _viewController.ScreenMouseLocation
            Dim oldSimulationMouseLocation = _viewController.SimulationMouseLocation
            _viewController.ScreenMouseLocation = New Vector2D(e.Location)
            Select Case e.Button
                Case MouseButtons.Left
                    setWallAtScreenLocation(New Vector2D(e.Location))
                Case MouseButtons.Middle
                    _viewController.Rotate(oldScreenMouseLocation)
                Case MouseButtons.Right
                    _viewController.Relocate(oldSimulationMouseLocation)
            End Select
            _renderer.Render()
        End If
    End Sub

    Private _directory As String = My.Computer.FileSystem.SpecialDirectories.Desktop

    Private Sub _timer_FrameTick(ByVal sender As Object, ByVal e As Visualization.FrameTickEventArgs) Handles _timer.FrameTick
        For i = 0 To e.CalcsPerFrame - 1
            _flowPanel.NextTimeStep()
        Next
        _renderer.Render()
        '_paraviewWriter.WriteToTextFile()
        Dim smokeSum As Double = 0
        For Each flowBox In _flowPanel.Array
            smokeSum += flowBox.SmokeNew
        Next

        testLabel.Text = smokeSum.ToString
        'testLabel.Text = _flowPanel.Array(0, CInt((_flowPanel.RowCount - 1) / 2)).Velocity.X.ToString & ControlChars.CrLf &
        '                (((_flowPanel.RowCount - 2) / 2) ^ 2 * force.X / (_flowPanel.Viscosity * 2)).ToString
        calculationCountLabel.Text = "Calculation count: " & _flowPanel.CalculationCount.ToString
        frameCountLabel.Text = "Frame count: " & CInt(_flowPanel.CalculationCount / e.CalcsPerFrame).ToString
    End Sub

    Private Sub startButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles restartButton.Click
        IO.Directory.CreateDirectory(_directory)

        _graphics = pictureBox.CreateGraphics()
        _graphics.Clear(Color.Black)
        _visualizer = New Visualizer2D(_graphics)
        _visualizer.BackColor = Color.Black

        Dim arraySize As New Vector2D(1, 0.4)
        Dim rectangleSize = New Vector2D(0.2, 0.3)

        _flowPanel = New FlowPanel2D(lowerVertex:=-arraySize / 2, Size:=arraySize, gridLength:=0.01, viscosity:=0.03, startVelocity:=New Vector2D(0.1, 0))

        Dim pointSet1 = New PointSet2DTransformer(New Math.Rectangle2D(New Vector2D(0.14999999999999999, 0.14999999999999999)), New AffineMap2D(New LinearMap2D(), New Vector2D(-0.29999999999999999, -0.20000000000000001)))
        Dim pointSet2 = New PointSet2DTransformer(New Circle2D(0.080000000000000002), New AffineMap2D(New LinearMap2D(), New Vector2D(-0.14999999999999999, 0)))
        Dim pointSet3 = New LinkedPointSet2D(pointSet1, pointSet2, Function(b1 As Boolean, b2 As Boolean) b1 OrElse b2)
        Dim pointSet4 = New PointSet2DTransformer(New Circle2D(0.080000000000000002), New AffineMap2D(New LinearMap2D(), New Vector2D(0.29999999999999999, -0.10000000000000001)))
        Dim pointSet = New LinkedPointSet2D(pointSet3, pointSet4, Function(b1 As Boolean, b2 As Boolean) b1 OrElse b2)

        'Dim pointSet = New PointSet2DTransformer(New Airfoil2D(chordLength:=0.6, relativeThickness:=0.2, maxCamber:=0.05, relativeMaxCamberLocation:=0.7), map:=New AffineMap2D(LinearMap2D.Rotation(-PI / 12), New Vector2D(-0.4, 0.1)))

        'Dim pointSet = New LinkedPointSet2D(New PointSet2DTransformer(New HalfRing2D(0.15, 0.02), map:=New AffineMap2D(New LinearMap2D, New Vector2D(-0.2, 0))), New PointSet2DTransformer(New Circle(0.01), New AffineMap2D(New LinearMap2D, New Vector2D(-0.2, 0.15))), Function(bool1, bool2) bool1 OrElse bool2)
        'Dim pointSet = New PointSet2DTransformer(
        'New LinkedPointSet2D(New Math.HalfCircle(0.1),
        '                     New PointSet2DTransformer(New Math.Circle(0.01), map:=New AffineMap2D(New LinearMap2D, New Vector2D(0.02, 0.08))),
        '                     Function(bool1 As Boolean, bool2 As Boolean) bool1 Or bool2), New AffineMap2D(New LinearMap2D, New Vector2D(-0.3, 0)))
        '_flowPanel.SetWallFromPointSet(pointSet)
        'For i = 0 To _flowPanel.RowCount - 1
        '    _flowPanel.Array(10, i).SmokeNew = 1
        'Next

        _flowPanel.SetKeptVelocityBoxes() 'viscosity:=0)
        _flowPanel.SetWallFromPointSet(pointSet:=pointSet)
        _flowPanel.SetBoundWall()

        _drawer = New FlowDrawer2D(Of FlowBox2D)(_visualizer, _flowPanel, maxVelocityLength:=0.2, arrowSimulationLengthPerVelocity:=0.5)
        _drawer.VelocityDisplayMode = FlowDrawer2D(Of FlowBox2D).VelocityDisplayModes.Arrows
        _drawer.ShowVelocity = True
        _drawer.ShowDensity = False

        _viewController = New ViewController2D(_visualizer)

        _renderer = New Renderer2D(_drawer)

        _paraviewWriter = New FlowPanel2DParaviewWriter(Of FlowBox2D)(_flowPanel, _directory, " _airfoil")

        _timer = New FrameTimer(framerate:=CInt(frameRateTextBox.Text), calcRate:=CInt(calculationRateTextBox.Text))
        _timer.Start()
    End Sub

    Private Sub startStopButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles startStopButton.Click
        _timer.Enabled = Not _timer.Enabled
    End Sub

    Private Sub refreshButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles refreshButton.Click
        _timer.CalcRate = CInt(calculationRateTextBox.Text)
        _timer.Framerate = CInt(frameRateTextBox.Text)
    End Sub

End Class