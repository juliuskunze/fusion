Public Class Field2DArrowGridDrawer
    Implements IDrawer2D

    Public Sub New(ByVal visualizer As Visualizer2D, ByVal field As IField2D)
        Me.Visualizer = visualizer
        Me.Field = field

        Me.ArrowsCentered = False
        Me.MultiColored = False

        Me.FieldArrowPen = New Pen(Color.Beige)
        Me.FieldArrowPen.EndCap = Drawing2D.LineCap.ArrowAnchor
        Me.FieldArrowSimulationLengthPerFieldStrength = 0.00000001
        Me.FieldArrowRealGridLengthInDots = 50
    End Sub

    Public Property Visualizer As Visualizer2D Implements IDrawer2D.Visualizer
    Public Property Field As IField2D

    Public Property ArrowsCentered As Boolean

    Public Property MultiColored As Boolean

    Public Property FieldArrowSimulationLengthPerFieldStrength As Double
    Public Property FieldArrowRealGridLengthInDots As Double

    Public Property FieldArrowPen() As Pen

    Public Sub Draw() Implements IDrawer2D.Draw
        For xIndex = Me.Visualizer.Graphics.VisibleClipBounds.Left To Me.Visualizer.Graphics.VisibleClipBounds.Right Step Me.FieldArrowRealGridLengthInDots
            For yIndex = Me.Visualizer.Graphics.VisibleClipBounds.Top To Me.Visualizer.Graphics.VisibleClipBounds.Bottom Step Me.FieldArrowRealGridLengthInDots
                Try
                    Dim screenLocation = New Vector2D(xIndex, yIndex)
                    Dim simulationLocation = Me.Visualizer.Map.Inverse.Apply(screenLocation)
                    Dim field = Me.Field.Field(simulationLocation)
                    Dim finalDrawingPen As Pen = DirectCast(Me.FieldArrowPen.Clone, Pen)

                    If Me.MultiColored Then
                        finalDrawingPen.Color = New HsvColor(hue:=field.Argument, saturation:=1, value:=1).ToRgbColor
                    End If

                    drawFieldArrow(arrowLocation:=simulationLocation, fieldStrength:=field, pen:=finalDrawingPen)
                Catch ex As OverflowException
                End Try
            Next
        Next
    End Sub

    Private Sub drawFieldArrow(ByVal arrowLocation As Vector2D, ByVal fieldStrength As Vector2D, ByVal pen As Pen)
        If Me.ArrowsCentered Then
            drawCenteredFieldArrow(arrowLocation:=arrowLocation, fieldStrength:=fieldStrength, pen:=pen)
        Else
            drawUncenteredFieldArrow(arrowLocation:=arrowLocation, fieldStrength:=fieldStrength, pen:=pen)
        End If
    End Sub

    Private Sub drawUncenteredFieldArrow(ByVal arrowLocation As Vector2D, ByVal fieldStrength As Vector2D, ByVal pen As Pen)
        Dim arrowVector = _FieldArrowSimulationLengthPerFieldStrength * fieldStrength
        Me.Visualizer.DrawingGraphics.DrawLine(pen, Me.Visualizer.Map.Apply(arrowLocation).ToPointF, Me.Visualizer.Map.Apply(arrowLocation + arrowVector).ToPointF)
    End Sub

    Private Sub drawCenteredFieldArrow(ByVal arrowLocation As Vector2D, ByVal fieldStrength As Vector2D, ByVal pen As Pen)
        Dim halfArrowVector = 0.5 * _FieldArrowSimulationLengthPerFieldStrength * fieldStrength
        Me.Visualizer.DrawingGraphics.DrawLine(pen, Me.Visualizer.Map.Apply(arrowLocation - halfArrowVector).ToPointF, Me.Visualizer.Map.Apply(arrowLocation + halfArrowVector).ToPointF)
    End Sub

End Class