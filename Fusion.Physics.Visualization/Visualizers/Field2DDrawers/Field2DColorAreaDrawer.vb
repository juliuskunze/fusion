Public Class Field2DColorAreaDrawer
    Implements IDrawer2D

    Public Sub New(ByVal visualizer As Visualizer2D, ByVal field As IField2D)
        Me.Visualizer = visualizer
        Me.Field = field
    End Sub

    Public Property Visualizer As Visualizer2D Implements IDrawer2D.Visualizer
    Public Property Field As IField2D

    Public Sub Draw() Implements IDrawer2D.Draw
        Dim bitmap = New Bitmap(CInt(Me.Visualizer.DrawingGraphics.VisibleClipBounds.Width), CInt(Me.Visualizer.DrawingGraphics.VisibleClipBounds.Height))

        For xIndex = 0 To CInt(Me.Visualizer.Graphics.VisibleClipBounds.Width) - 1
            For yIndex = 0 To CInt(Me.Visualizer.Graphics.VisibleClipBounds.Height) - 1
                Try
                    Dim screenLocation = New Vector2D(xIndex, yIndex)
                    Dim simulationLocation = Me.Visualizer.InverseMap.Apply(screenLocation)
                    Dim field = Me.Field.Field(simulationLocation)
                    Dim fieldAngle = field.Argument
                    Dim color = New HsbColor(hue:=fieldAngle, saturation:=1, value:=1).ToRgbColor

                    bitmap.SetPixel(xIndex, yIndex, color)
                Catch ex As OverflowException
                End Try
            Next
        Next

        Me.Visualizer.DrawingGraphics.DrawImageUnscaled(image:=bitmap, point:=New Point(0, 0))
    End Sub

End Class
