Public Class Vector2DEvolutionDrawer
    Implements IDrawer2D

    Private _solutions As List(Of Vector2D)
    Private _badSolutions As List(Of List(Of Vector2D))

    Private WithEvents _vectorEvolutionStrategy As VectorEvolutionStrategy
    Public Property VectorEvolutionStrategy() As VectorEvolutionStrategy
        Get
            Return _vectorEvolutionStrategy
        End Get
        Set(ByVal value As VectorEvolutionStrategy)
            _vectorEvolutionStrategy = value

            _solutions = New List(Of Vector2D)
            _badSolutions = New List(Of List(Of Vector2D))
        End Set
    End Property


    Public Sub New(ByVal graphics As Graphics, ByVal vectorEvolutionStrategy As VectorEvolutionStrategy)
        Me.Visualizer = New Visualizer2D(graphics)

        Me.Visualizer.BackColor = Color.White
        Me.Visualizer.ProjectionMap = Me.Visualizer.ProjectionMap.After(AffineMap2D.Scaling(1 / 100))

        Me.VectorEvolutionStrategy = vectorEvolutionStrategy

        _badSolutionPen = New Pen(Color.Gray)
        _badSolutionPen.EndCap = Drawing2D.LineCap.ArrowAnchor

        AddHandler _vectorEvolutionStrategy.BestSolutionImproved, AddressOf vectorEvolutionStrategy_BestSolutionImproved
        AddHandler _vectorEvolutionStrategy.BadSolutionGenerated, AddressOf vectorEvolutionStrategy_BadSolutionGenerated
    End Sub

    Private Sub vectorEvolutionStrategy_BestSolutionImproved(ByVal sender As Object, ByVal e As SolutionEventArgs(Of Vector2D))
        _solutions.Add(e.Solution)
        _badSolutions.Add(New List(Of Vector2D))
    End Sub

    Private Sub vectorEvolutionStrategy_BadSolutionGenerated(ByVal sender As Object, ByVal e As Evolution.SolutionEventArgs(Of Math.Vector2D)) Handles _vectorEvolutionStrategy.BadSolutionGenerated
        _badSolutions(_solutions.Count - 1).Add(e.Solution)
    End Sub

    Public Sub Draw() Implements IDrawer2D.Draw
        drawConnections()
        drawBadSolutions()
        drawSolutions()
        drawTarget()
    End Sub

    Private Sub drawTarget()
        Me.Visualizer.DrawingGraphics.FillEllipse(New SolidBrush(Color.Red), Me.Visualizer.GenerateCircleRect(New Vector2D(50, 50), 0.1))
    End Sub

    Private Sub drawSolutions()
        For Each solution In _solutions
            Me.Visualizer.DrawingGraphics.FillEllipse(New SolidBrush(Color.Black), Me.Visualizer.GenerateCircleRect(solution, 0.1))
        Next
    End Sub

    Private Sub drawConnections()
        Dim connectionPen = New Pen(Color.Black)
        connectionPen.EndCap = Drawing2D.LineCap.ArrowAnchor

        For i = 0 To _solutions.Count - 2
            Me.Visualizer.DrawingGraphics.DrawLine(connectionPen, Me.Visualizer.Map.Apply(_solutions(i)).ToPointF, Me.Visualizer.Map.Apply(_solutions(i + 1)).ToPointF)
        Next
    End Sub

    Private Sub drawBadSolutions()
        Dim connectionPen = New Pen(Color.Gray)
        connectionPen.EndCap = Drawing2D.LineCap.ArrowAnchor

        For i = 0 To _solutions.Count - 1
            For Each badSolution In _badSolutions(i)
                drawBadSolution(i, badSolution)
            Next
        Next
    End Sub

    Private _badSolutionPen As Pen

    Private Sub drawBadSolution(ByVal goodSolutionIndex As Integer, ByVal badSolution As Vector2D)
        Me.Visualizer.DrawingGraphics.DrawLine(_badSolutionPen, Me.Visualizer.Map.Apply(_solutions(goodSolutionIndex)).ToPointF, Me.Visualizer.Map.Apply(badSolution).ToPointF)
    End Sub

    Public Property Visualizer As Visualizer2D Implements IDrawer2D.Visualizer
End Class
