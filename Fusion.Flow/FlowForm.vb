Public Class FlowForm

    Private _FlowPanel As FlowPanel2D
    Private WithEvents _Timer As FrameTimer

    Private _Graphics As Graphics
    Private _Visualizer As Visualizer2D
    Private _Drawer As FlowDrawer2D(Of FlowBox2D)
    Private WithEvents _ViewController As ViewController2D
    Private _Renderer As Renderer2D

    Private _ParaviewWriter As FlowPanel2DParaviewWriter(Of FlowBox2D)

    Private Sub PointSet2DPlotterForm_MouseClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseClick
        _Renderer.Render()
    End Sub

    Private Sub pictureBox_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles pictureBox.MouseDown
        Select Case e.Button
            Case MouseButtons.Left
                setWallAtScreenLocation(New Vector2D(e.Location))
            Case MouseButtons.Middle
                _ViewController.StartRotate()
        End Select
    End Sub

    Private Sub setWallAtScreenLocation(screenLocation As Vector2D)
        Dim simulationLocation = _Visualizer.InverseMap.Apply(screenLocation)
        Dim arrayLocation = (simulationLocation - _FlowPanel.LowerVertex) / _FlowPanel.GridLength
        Dim columnIndex = CInt(arrayLocation.X)
        Dim rowIndex = CInt(arrayLocation.Y)
        If 0 <= columnIndex AndAlso columnIndex < _FlowPanel.ColumnCount AndAlso 0 <= rowIndex AndAlso rowIndex < _FlowPanel.RowCount Then
            _FlowPanel.Array(columnIndex, rowIndex).IsWall = True
        End If
    End Sub

    Private Sub PointSet2DPlotterForm_MouseWheel(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseWheel
        If _Drawer IsNot Nothing Then
            _ViewController.Zoom(zoomSteps:=e.Delta / 120)
            _Renderer.Render()
        End If
    End Sub

    Private Sub pictureBox_MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles pictureBox.MouseMove
        If _Drawer IsNot Nothing Then
            Dim oldScreenMouseLocation = _ViewController.ScreenMouseLocation
            Dim oldSimulationMouseLocation = _ViewController.SimulationMouseLocation
            _ViewController.ScreenMouseLocation = New Vector2D(e.Location)
            Select Case e.Button
                Case MouseButtons.Left
                    setWallAtScreenLocation(New Vector2D(e.Location))
                Case MouseButtons.Middle
                    _ViewController.Rotate(oldScreenMouseLocation)
                Case MouseButtons.Right
                    _ViewController.Relocate(oldSimulationMouseLocation)
            End Select
            _Renderer.Render()
        End If
    End Sub

    Private ReadOnly _Directory As String = My.Computer.FileSystem.SpecialDirectories.Desktop

    Private Sub _Timer_FrameTick(sender As Object, e As Visualization.FrameTickEventArgs) Handles _Timer.FrameTick
        For i = 0 To e.CalcsPerFrame - 1
            _FlowPanel.NextTimeStep()
        Next
        _Renderer.Render()
        '_ParaviewWriter.WriteToTextFile()
        Dim smokeSum As Double = 0
        For Each flowBox In _FlowPanel.Array
            smokeSum += flowBox.SmokeNew
        Next

        testLabel.Text = smokeSum.ToString
        'testLabel.Text = _FlowPanel.Array(0, CInt((_FlowPanel.RowCount - 1) / 2)).Velocity.X.ToString & ControlChars.CrLf &
        '                (((_FlowPanel.RowCount - 2) / 2) ^ 2 * force.X / (_FlowPanel.Viscosity * 2)).ToString
        calculationCountLabel.Text = "Calculation count: " & _FlowPanel.CalculationCount.ToString
        frameCountLabel.Text = "Frame count: " & CInt(_FlowPanel.CalculationCount / e.CalcsPerFrame).ToString
    End Sub

    Private Sub startButton_Click(sender As System.Object, e As System.EventArgs) Handles restartButton.Click
        IO.Directory.CreateDirectory(_Directory)

        _Graphics = pictureBox.CreateGraphics()
        _Graphics.Clear(Color.Black)
        _Visualizer = New Visualizer2D(_Graphics)
        _Visualizer.BackColor = Color.Black

        Dim arraySize As New Vector2D(1, 0.4)
        Dim rectangleSize = New Vector2D(0.2, 0.3)

        _FlowPanel = New FlowPanel2D(lowerVertex:=-arraySize / 2, Size:=arraySize, gridLength:=0.01, viscosity:=0.03, startVelocity:=New Vector2D(0.1, 0))

        Dim pointSet1 = New PointSet2DTransformer(New Math.Rectangle2D(New Vector2D, New Vector2D(0.15, 0.15)), New AffineMap2D(New LinearMap2D(), New Vector2D(-0.3, -0.2)))
        Dim pointSet2 = New PointSet2DTransformer(New Circle2D(0.08), New AffineMap2D(New LinearMap2D(), New Vector2D(-0.15, 0)))
        Dim pointSet3 = New LinkedPointSet2D(pointSet1, pointSet2, Function(b1 As Boolean, b2 As Boolean) b1 OrElse b2)
        Dim pointSet4 = New PointSet2DTransformer(New Circle2D(0.08), New AffineMap2D(New LinearMap2D(), New Vector2D(0.3, -0.1)))
        Dim pointSet = New LinkedPointSet2D(pointSet3, pointSet4, Function(b1 As Boolean, b2 As Boolean) b1 OrElse b2)

        'Dim pointSet = New PointSet2DTransformer(New Airfoil2D(chordLength:=0.6, relativeThickness:=0.2, maxCamber:=0.05, relativeMaxCamberLocation:=0.7), map:=New AffineMap2D(LinearMap2D.Rotation(-PI / 12), New Vector2D(-0.4, 0.1)))

        'Dim pointSet = New LinkedPointSet2D(New PointSet2DTransformer(New HalfRing2D(0.15, 0.02), map:=New AffineMap2D(New LinearMap2D, New Vector2D(-0.2, 0))), New PointSet2DTransformer(New Circle(0.01), New AffineMap2D(New LinearMap2D, New Vector2D(-0.2, 0.15))), Function(bool1, bool2) bool1 OrElse bool2)
        'Dim pointSet = New PointSet2DTransformer(
        'New LinkedPointSet2D(New Math.HalfCircle(0.1),
        '                     New PointSet2DTransformer(New Math.Circle(0.01), map:=New AffineMap2D(New LinearMap2D, New Vector2D(0.02, 0.08))),
        '                     Function(bool1 As Boolean, bool2 As Boolean) bool1 Or bool2), New AffineMap2D(New LinearMap2D, New Vector2D(-0.3, 0)))
        '_FlowPanel.SetWallFromPointSet(pointSet)
        'For i = 0 To _FlowPanel.RowCount - 1
        '    _FlowPanel.Array(10, i).SmokeNew = 1
        'Next

        _FlowPanel.SetKeptVelocityBoxes() 'viscosity:=0)
        _FlowPanel.SetWallFromPointSet(pointSet:=pointSet)
        _FlowPanel.SetBoundWall()

        _Drawer = New FlowDrawer2D(Of FlowBox2D)(_Visualizer, _FlowPanel, maxVelocityLength:=0.2, arrowSimulationLengthPerVelocity:=0.5)
        _Drawer.VelocityDisplayMode = FlowDrawer2D(Of FlowBox2D).VelocityDisplayModes.Arrows
        _Drawer.ShowVelocity = True
        _Drawer.ShowDensity = False

        _ViewController = New ViewController2D(_Visualizer)

        _Renderer = New Renderer2D(_Drawer)

        _ParaviewWriter = New FlowPanel2DParaviewWriter(Of FlowBox2D)(_FlowPanel, _Directory, " _Airfoil")

        _Timer = New FrameTimer(framerate:=CInt(frameRateTextBox.Text), calcRate:=CInt(calculationRateTextBox.Text))
        _Timer.Start()
    End Sub

    Private Sub startStopButton_Click(sender As System.Object, e As System.EventArgs) Handles startStopButton.Click
        _Timer.Enabled = Not _Timer.Enabled
    End Sub

    Private Sub refreshButton_Click(sender As System.Object, e As System.EventArgs) Handles refreshButton.Click
        _Timer.CalcRate = CInt(calculationRateTextBox.Text)
        _Timer.Framerate = CInt(frameRateTextBox.Text)
    End Sub

End Class