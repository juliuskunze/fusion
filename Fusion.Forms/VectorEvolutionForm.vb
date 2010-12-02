Public Class VectorEvolutionForm

    Private WithEvents _strategy As VectorEvolutionStrategy

    Private _drawer As Vector2DEvolutionDrawer
    Private _renderer As Renderer2D
    Private _viewController As ViewController2D

    Private Sub btnStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStart.Click
        RemoveHandler pbx.MouseMove, AddressOf pbx_MouseMove
        RemoveHandler pbx.MouseWheel, AddressOf pbx_MouseWheel
        _strategy = New VectorEvolutionStrategy()
        If _drawer Is Nothing Then
            _drawer = New Vector2DEvolutionDrawer(pbx.CreateGraphics, _strategy)
        End If

        _renderer = New Renderer2D(_drawer)
        _drawer.VectorEvolutionStrategy = _strategy

        _viewController = New ViewController2D(_drawer.Visualizer)
        _strategy.Start()

        pbx.Focus()

        AddHandler pbx.MouseMove, AddressOf pbx_MouseMove
        AddHandler pbx.MouseWheel, AddressOf pbx_MouseWheel
    End Sub

    Private Sub _strategy_BestSolutionImproved(ByVal sender As Object, ByVal e As Evolution.SolutionEventArgs(Of Math.Vector2D)) Handles _strategy.BestSolutionImproved
        _renderer.Render()
    End Sub

    Private Sub pbx_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        _viewController.ScreenMouseLocation = New Vector2D(e.Location)
        _renderer.Render()
    End Sub

    Private Const _deltaPerMouseWheelStep As Double = 120
    Private Sub pbx_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        _viewController.Zoom(zoomSteps:=e.Delta / _deltaPerMouseWheelStep)
        _renderer.Render()
    End Sub
End Class