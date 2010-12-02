Public Class Renderer2D

    Public Property Drawer As IDrawer2D

    Public Sub New(ByVal drawer As IDrawer2D)
        Me.Drawer = drawer
    End Sub

    Public Sub Render()
        Me.Drawer.Visualizer.Render(AddressOf Me.Drawer.Draw)
    End Sub

End Class
