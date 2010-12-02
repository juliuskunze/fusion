Public Class PointSetArrayDrawer
    Implements IDrawer2D

    Public Property PointSetToArray As PointSet2DToArray

    Public Sub New(ByVal visualizer As Visualizer2D, ByVal pointSetToArray As PointSet2DToArray)
        Me.Visualizer = visualizer
        Me.PointSetToArray = pointSetToArray
    End Sub

    Public Sub Draw() Implements Fusion.Visualization.IDrawer2D.Draw
        Dim grid = Me.PointSetToArray.Array
        For columnIndex = 0 To Me.PointSetToArray.ColumnCount - 1
            For rowIndex = 0 To Me.PointSetToArray.RowCount - 1
                If grid(columnIndex, rowIndex) Then
                    Dim center = Me.Visualizer.Map.Apply(Me.PointSetToArray.PointFromRowColumn(columnIndex, rowIndex))
                    Dim halfDiagonalVector = New Vector2D(Me.PointSetToArray.GridLength / 2, Me.PointSetToArray.GridLength / 2) * Me.Visualizer.Map.LinearMap.ZoomOut
                    Dim brush = New SolidBrush(Color.Beige)
                    Me.Visualizer.DrawingGraphics.FillEllipse(brush, New RectangleF((center - halfDiagonalVector).ToPointF, (2 * halfDiagonalVector).ToSizeF))
                    brush.Dispose()
                End If
            Next
        Next
    End Sub

    Public Property Visualizer As Fusion.Visualization.Visualizer2D Implements Fusion.Visualization.IDrawer2D.Visualizer
End Class
