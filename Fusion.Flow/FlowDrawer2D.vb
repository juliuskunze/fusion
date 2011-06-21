Public Class FlowDrawer2D(Of T As IFlowBox2D)
    Implements IDrawer2D

    Public Property ArrowSimulationLengthPerVelocity As Double
    Public Property MaxVelocityLength As Double

    Public Property FlowPanel As ICoordinateSystemToArray(Of T)

    Public Sub New(ByVal visualizer As Visualizer2D, ByVal flowPanel As ICoordinateSystemToArray(Of T), ByVal arrowSimulationLengthPerVelocity As Double, ByVal maxVelocityLength As Double, Optional ByVal displayMode As VelocityDisplayModes = VelocityDisplayModes.Arrows, Optional ByVal showVelocity As Boolean = True, Optional ByVal showDensity As Boolean = True)
        Me.Visualizer = visualizer
        Me.FlowPanel = flowPanel
        Me.VelocityDisplayMode = displayMode
        Me.ArrowSimulationLengthPerVelocity = arrowSimulationLengthPerVelocity
        Me.MaxVelocityLength = maxVelocityLength
        Me.ShowVelocity = showVelocity
        Me.ShowDensity = showDensity
    End Sub

    Public Enum VelocityDisplayModes
        Arrows
        Rectangles
    End Enum
    Public Property VelocityDisplayMode As VelocityDisplayModes

    Public Property ShowDensity As Boolean
    Public Property ShowVelocity As Boolean

    Public Sub Draw() Implements Visualization.IDrawer2D.Draw
        For columnIndex = 0 To Me.FlowPanel.ColumnCount - 1
            For rowIndex = 0 To Me.FlowPanel.RowCount - 1
                Dim center = Me.Visualizer.Map.Apply(Me.FlowPanel.PointFromRowColumn(columnIndex, rowIndex))
                Dim halfDiagonalVector = New Vector2D(Me.FlowPanel.GridLength / 2, Me.FlowPanel.GridLength / 2) * Me.Visualizer.Map.LinearMap.ZoomOut
                Dim rectangle = New RectangleF((center - halfDiagonalVector).ToPointF, (2 * halfDiagonalVector).ToSizeF)
                If Me.FlowPanel.Array(columnIndex, rowIndex).IsWall Then
                    Using brush = New SolidBrush(color.Beige)
                        Me.Visualizer.DrawingGraphics.FillRectangle(brush, rectangle)
                    End Using
                Else
                    If Me.ShowVelocity Then
                        Dim simulationLocation = Me.FlowPanel.PointFromRowColumn(columnIndex, rowIndex)
                        Dim screenLocation = Me.Visualizer.Map.Apply(simulationLocation)

                        Dim velocity = Me.FlowPanel.Array(columnIndex, rowIndex).Velocity
                        Dim color = New HsbColor(hue:=velocity.Argument, saturation:=1, value:=Min(velocity.Length / Me.MaxVelocityLength, 1)).ToRgbColor

                        Select Case Me.VelocityDisplayMode
                            Case VelocityDisplayModes.Arrows
                                Using pen = New Pen(color)
                                    pen.EndCap = Drawing2D.LineCap.ArrowAnchor

                                    Dim halfArrowVelocity = 0.5 * velocity * Me.ArrowSimulationLengthPerVelocity
                                    Me.Visualizer.DrawingGraphics.DrawLine(pen, Me.Visualizer.Map.Apply(simulationLocation - halfArrowVelocity).ToPointF, Me.Visualizer.Map.Apply(simulationLocation + halfArrowVelocity).ToPointF)
                                End Using
                            Case VelocityDisplayModes.Rectangles
                                Using brush = New SolidBrush(color)
                                    Me.Visualizer.DrawingGraphics.FillRectangle(brush, rectangle)
                                End Using
                        End Select
                    End If

                    If Me.ShowDensity Then
                        Dim maxDensity = 1.5
                        Dim density = Me.FlowPanel.Array(columnIndex, rowIndex).Density
                        Dim color = New HsbColor(hue:=0, saturation:=0, value:=Max(0, Min(1, density / maxDensity))).ToRgbColor
                        Me.Visualizer.DrawingGraphics.FillRectangle(New SolidBrush(color), rectangle)
                    End If
                End If
            Next
        Next
    End Sub

    Public Property Visualizer As Visualization.Visualizer2D Implements Visualization.IDrawer2D.Visualizer

End Class
