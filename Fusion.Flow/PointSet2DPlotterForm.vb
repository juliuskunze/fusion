Public Class PointSet2DPlotterForm

    Private _graphics As Graphics
    Private _visualizer As Visualizer2D
    Private _drawer As PointSetArrayDrawer

    Private WithEvents _viewController As ViewController2D

    Private _pointSet As IPointSet2D
    Private _pointSetToArray As PointSet2DToArray

    Private _renderer As Renderer2D

    Private Sub form_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'FileTest.Test()

        _graphics = pictureBox.CreateGraphics()
        _graphics.Clear(Color.Black)
        _visualizer = New Visualizer2D(_graphics)
        _visualizer.BackColor = Color.Black

        _pointSet = New LinkedPointSet2D(New Airfoil2D(chordLength:=1, relativeThickness:=0.10000000000000001, maxCamber:=0.040000000000000001, relativeMaxCamberLocation:=0.69999999999999996), New Circle2D(0.20000000000000001), Function(bool1 As Boolean, bool2 As Boolean) bool1 AndAlso bool2)  'New HalfRing2D(innerRadius:=0.5, thickness:=0.2) ''New Geometry.Rectangle(New Vector2D(0.5, 0.5)) 'New PointSet2DTransformer(New Circle(0.5), New Vector2D(0.5, 0.5)) '  '
        _pointSetToArray = New PointSet2DToArray(lowerVertex:=New Vector2D(-1, -1), Size:=New Vector2D(2, 2), gridLength:=0.005, pointSet:=_pointSet)
        _drawer = New PointSetArrayDrawer(_visualizer, _pointSetToArray)

        _viewController = New ViewController2D(_visualizer)

        _renderer = New Renderer2D(_drawer)
    End Sub

    Private Sub PointSet2DPlotterForm_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseClick
        _renderer.Render()
    End Sub

    Private Sub pictureBox_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pictureBox.MouseDown  
        Select Case e.Button
            Case MouseButtons.Middle
                _viewController.StartRotate()
        End Select
    End Sub

    Private Sub PointSet2DPlotterForm_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseWheel
        _viewController.Zoom(zoomSteps:=e.Delta / 120)
        _renderer.Render()
    End Sub

    Private Sub pictureBox_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pictureBox.MouseMove
        Dim oldScreenMouseLocation = _viewController.ScreenMouseLocation
        Dim oldSimulationMouseLocation = _viewController.SimulationMouseLocation
        _viewController.ScreenMouseLocation = New Vector2D(e.Location)
        Select Case e.Button
            Case MouseButtons.Middle
                _viewController.Rotate(oldScreenMouseLocation)
            Case MouseButtons.Right
                _viewController.Relocate(oldSimulationMouseLocation)
        End Select
        _renderer.Render()
    End Sub

End Class
