Public Class Vector2DEvolutionDrawer
    Implements IDrawer2D

    Private _Solutions As List(Of Vector2D)
    Private _BadSolutions As List(Of List(Of Vector2D))

    Private WithEvents _VectorEvolutionStrategy As VectorEvolutionStrategy
    Public Property VectorEvolutionStrategy() As VectorEvolutionStrategy
        Get
            Return _VectorEvolutionStrategy
        End Get
        Set(value As VectorEvolutionStrategy)
            _VectorEvolutionStrategy = value

            _Solutions = New List(Of Vector2D)
            _BadSolutions = New List(Of List(Of Vector2D))
        End Set
    End Property


    Public Sub New(graphics As Graphics, vectorEvolutionStrategy As VectorEvolutionStrategy)
        Me.Visualizer = New Visualizer2D(graphics)

        Me.Visualizer.BackColor = Color.White
        Me.Visualizer.ProjectionMap = Me.Visualizer.ProjectionMap.After(AffineMap2D.Scaling(1 / 100))

        Me.VectorEvolutionStrategy = vectorEvolutionStrategy

        _BadSolutionPen = New Pen(Color.Gray)
        _BadSolutionPen.EndCap = Drawing2D.LineCap.ArrowAnchor

        AddHandler _VectorEvolutionStrategy.BestSolutionImproved, AddressOf vectorEvolutionStrategy_BestSolutionImproved
        AddHandler _VectorEvolutionStrategy.BadSolutionGenerated, AddressOf vectorEvolutionStrategy_BadSolutionGenerated
    End Sub

    Private Sub VectorEvolutionStrategy_BestSolutionImproved(sender As Object, e As SolutionEventArgs(Of Vector2D))
        _Solutions.Add(e.Solution)
        _BadSolutions.Add(New List(Of Vector2D))
    End Sub

    Private Sub VectorEvolutionStrategy_BadSolutionGenerated(sender As Object, e As Evolution.SolutionEventArgs(Of Math.Vector2D)) Handles _VectorEvolutionStrategy.BadSolutionGenerated
        _BadSolutions(_Solutions.Count - 1).Add(e.Solution)
    End Sub

    Public Sub Draw() Implements IDrawer2D.Draw
        drawConnections()
        drawBadSolutions()
        drawSolutions()
        drawTarget()
    End Sub

    Private Sub DrawTarget()
        Me.Visualizer.DrawingGraphics.FillEllipse(New SolidBrush(Color.Red), Me.Visualizer.GenerateCircleRect(New Vector2D(50, 50), 0.1))
    End Sub

    Private Sub DrawSolutions()
        For Each solution In _Solutions
            Me.Visualizer.DrawingGraphics.FillEllipse(New SolidBrush(Color.Black), Me.Visualizer.GenerateCircleRect(solution, 0.1))
        Next
    End Sub

    Private Sub DrawConnections()
        Dim connectionPen = New Pen(Color.Black)
        connectionPen.EndCap = Drawing2D.LineCap.ArrowAnchor

        For i = 0 To _Solutions.Count - 2
            Me.Visualizer.DrawingGraphics.DrawLine(connectionPen, Me.Visualizer.Map.Apply(_Solutions(i)).ToPointF, Me.Visualizer.Map.Apply(_Solutions(i + 1)).ToPointF)
        Next
    End Sub

    Private Sub DrawBadSolutions()
        Dim connectionPen = New Pen(Color.Gray)
        connectionPen.EndCap = Drawing2D.LineCap.ArrowAnchor

        For i = 0 To _Solutions.Count - 1
            For Each badSolution In _BadSolutions(i)
                drawBadSolution(i, badSolution)
            Next
        Next
    End Sub

    Private ReadOnly _BadSolutionPen As Pen

    Private Sub drawBadSolution(goodSolutionIndex As Integer, badSolution As Vector2D)
        Me.Visualizer.DrawingGraphics.DrawLine(_BadSolutionPen, Me.Visualizer.Map.Apply(_Solutions(goodSolutionIndex)).ToPointF, Me.Visualizer.Map.Apply(badSolution).ToPointF)
    End Sub

    Public Property Visualizer As Visualizer2D Implements IDrawer2D.Visualizer
End Class
