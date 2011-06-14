Public Class VectorEvolutionForm

    Private WithEvents _Strategy As VectorEvolutionStrategy

    Private _Drawer As Vector2DEvolutionDrawer
    Private _Renderer As Renderer2D
    Private _ViewController As ViewController2D

    Private Sub btnStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStart.Click
        RemoveHandler pbx.MouseMove, AddressOf pbx_MouseMove
        RemoveHandler pbx.MouseWheel, AddressOf pbx_MouseWheel
        _Strategy = New VectorEvolutionStrategy()
        If _Drawer Is Nothing Then
            _Drawer = New Vector2DEvolutionDrawer(pbx.CreateGraphics, _Strategy)
        End If

        _Renderer = New Renderer2D(_Drawer)
        _Drawer.VectorEvolutionStrategy = _Strategy

        _ViewController = New ViewController2D(_Drawer.Visualizer)
        _Strategy.Start()

        pbx.Focus()

        AddHandler pbx.MouseMove, AddressOf pbx_MouseMove
        AddHandler pbx.MouseWheel, AddressOf pbx_MouseWheel
    End Sub

    Private Sub _Strategy_BestSolutionImproved(ByVal sender As Object, ByVal e As Evolution.SolutionEventArgs(Of Math.Vector2D)) Handles _Strategy.BestSolutionImproved
        _Renderer.Render()
    End Sub

    Private Sub pbx_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        _ViewController.ScreenMouseLocation = New Vector2D(e.Location)
        _Renderer.Render()
    End Sub

    Private Const _DeltaPerMouseWheelStep As Double = 120
    Private Sub pbx_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        _ViewController.Zoom(zoomSteps:=e.Delta / _DeltaPerMouseWheelStep)
        _Renderer.Render()
    End Sub
End Class