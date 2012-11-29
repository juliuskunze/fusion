Public Class PointSet2DPlotterForm
    Private _Graphics As Graphics
    Private _Visualizer As Visualizer2D
    Private _Drawer As PointSetArrayDrawer

    Private WithEvents _ViewController As ViewController2D

    Private _PointSet As IPointSet2D
    Private _PointSetToArray As PointSet2DToArray

    Private _Renderer As Renderer2D

    Private Sub form_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        'FileTest.Test()

        _Graphics = pictureBox.CreateGraphics()
        _Graphics.Clear(Color.Black)
        _Visualizer = New Visualizer2D(_Graphics)
        _Visualizer.BackColor = Color.Black

        _PointSet = New LinkedPointSet2D(New Airfoil2D(chordLength:=1, relativeThickness:=0.1, maxCamber:=0.04, relativeMaxCamberLocation:=0.7), New Circle2D(0.2), Function(bool1 As Boolean, bool2 As Boolean) bool1 AndAlso bool2)  'New HalfRing2D(innerRadius:=0.5, thickness:=0.2) ''New Geometry.Rectangle(New Vector2D(0.5, 0.5)) 'New PointSet2DTransformer(New Circle(0.5), New Vector2D(0.5, 0.5)) '  '
        _PointSetToArray = New PointSet2DToArray(lowerVertex:=New Vector2D(-1, -1), Size:=New Vector2D(2, 2), gridLength:=0.005, pointSet:=_PointSet)
        _Drawer = New PointSetArrayDrawer(_Visualizer, _PointSetToArray)

        _ViewController = New ViewController2D(_Visualizer)

        _Renderer = New Renderer2D(_Drawer)
    End Sub

    Private Sub PointSet2DPlotterForm_MouseClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseClick
        _Renderer.Render()
    End Sub

    Private Sub pictureBox_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles pictureBox.MouseDown
        Select Case e.Button
            Case MouseButtons.Middle
                _ViewController.StartRotate()
        End Select
    End Sub

    Private Sub PointSet2DPlotterForm_MouseWheel(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseWheel
        _ViewController.Zoom(zoomSteps:=e.Delta / 120)
        _Renderer.Render()
    End Sub

    Private Sub pictureBox_MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles pictureBox.MouseMove
        Dim oldScreenMouseLocation = _ViewController.ScreenMouseLocation
        Dim oldSimulationMouseLocation = _ViewController.SimulationMouseLocation
        _ViewController.ScreenMouseLocation = New Vector2D(e.Location)
        Select Case e.Button
            Case MouseButtons.Middle
                _ViewController.Rotate(oldScreenMouseLocation)
            Case MouseButtons.Right
                _ViewController.Relocate(oldSimulationMouseLocation)
        End Select
        _Renderer.Render()
    End Sub
End Class
