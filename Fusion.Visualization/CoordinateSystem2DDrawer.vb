Public Class CoordinateSystem2DDrawer
    Implements IDrawer2D

    Public Property OriginCrossScreenRadiusInPixels As Double = 20
    Public Property OriginDotScreenRadiusInPixels As Double = 1

    Public Property CoordinateSystemPen() As Pen

    Public Sub New(ByVal visualizer As Visualizer2D)
        Me.Visualizer = visualizer

        Me.CoordinateSystemPen = New Pen(Color.Gray)
        Me.CoordinateSystemPen.EndCap = Drawing2D.LineCap.ArrowAnchor
    End Sub

    Public Sub Draw() Implements IDrawer2D.Draw
        drawOriginCross()
        drawVertexLabels()
    End Sub

    Private Sub drawOriginCross()
        Try
            Dim v1 = Me.Visualizer.Map.Apply(New Vector2D(-1, 0)) - Me.Visualizer.Map.TranslationVector
            v1.Length = _OriginCrossScreenRadiusInPixels
            v1 += Me.Visualizer.Map.TranslationVector

            Dim v2 = Me.Visualizer.Map.Apply(New Vector2D(1, 0)) - Me.Visualizer.Map.TranslationVector
            v2.Length = _OriginCrossScreenRadiusInPixels
            v2 += Me.Visualizer.Map.TranslationVector

            Me.Visualizer.DrawingGraphics.DrawLine(Me.CoordinateSystemPen, v1.ToPointF, v2.ToPointF)

            Dim w1 = Me.Visualizer.Map.Apply(New Vector2D(0, -1)) - Me.Visualizer.Map.TranslationVector
            w1.Length = _OriginCrossScreenRadiusInPixels
            w1 += Me.Visualizer.Map.TranslationVector

            Dim w2 = Me.Visualizer.Map.Apply(New Vector2D(0, 1)) - Me.Visualizer.Map.TranslationVector
            w2.Length = _OriginCrossScreenRadiusInPixels
            w2 += Me.Visualizer.Map.TranslationVector

            Me.Visualizer.DrawingGraphics.DrawLine(Me.CoordinateSystemPen, w1.ToPointF, w2.ToPointF)

            Dim diagonalDotVector = New Vector2D(Me.OriginDotScreenRadiusInPixels, Me.OriginDotScreenRadiusInPixels)

            Me.Visualizer.DrawingGraphics.FillEllipse(New SolidBrush(Me.CoordinateSystemPen.Color), New RectangleF((Me.Visualizer.Map.TranslationVector - diagonalDotVector).ToPointF, (2 * diagonalDotVector).ToSizeF))
        Catch ex As OverflowException
        End Try
    End Sub

    Private Sub drawVertexLabels()
        Me.Visualizer.DrawingGraphics.DrawString(Me.Visualizer.Map.Inverse.Apply(New Vector2D).ToString("#.0E00"), New Drawing.Font("Courier", 10), New SolidBrush(Color.White), New PointF(1, 1))
    End Sub

    Public Property Visualizer As Visualizer2D Implements IDrawer2D.Visualizer
End Class
