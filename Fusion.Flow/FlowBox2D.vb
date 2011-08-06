Public Class FlowBox2D
    Implements IFlowBox2D

    Public Property IsWall As Boolean Implements IFlowBox2D.IsWall

    Public Property Density As Double Implements IFlowBox2D.Density
    Public Property Velocity As Vector2D Implements IFlowBox2D.Velocity

    Public Property SmokeOld As Double
    Public Property SmokeNew As Double Implements IFlowBox2D.Smoke

    Public Property FEq As Double(,)
    Public Property FNew As Double(,)
    Public Property FOld As Double(,)

    Public Sub New(isWall As Boolean, density As Double, velocity As Vector2D, Optional smoke As Double = 0)
        Me.IsWall = isWall
        Me.Density = density

        ReDim FEq(2, 2), FNew(2, 2)

        Me.Velocity = velocity
        setFEqFromVelocity()
        copyFNewFromFeq()
        Me.SmokeOld = smoke
        Me.SmokeNew = 0
    End Sub

    Private Sub copyFNewFromFeq()
        Me.FNew = DirectCast(Me.FEq.Clone, Double(,))
    End Sub


    Public Overridable Sub NextTimeStep()
        If Not Me.IsWall Then
            copyFOldFromFNew()
            setVelocityAndDensityFromFOld()
            setFEqFromVelocity()
            Me.SmokeOld = Me.SmokeNew
            Me.SmokeNew = 0
        End If
    End Sub

    Private Sub copyFOldFromFNew()
        Me.FOld = DirectCast(FNew.Clone, Double(,))
    End Sub

    Protected Sub setFEqFromVelocity()
        For columnIndex = 0 To 2
            For rowIndex = 0 To 2
                If Not Me.IsWall Then
                    Dim velocityPart = Vector2D.DotProduct(Me.Velocity, _NeighborVectors(columnIndex, rowIndex))
                    Me.FEq(columnIndex, rowIndex) = _VelocityWeights(columnIndex, rowIndex) * _Density * (1 + 3 * velocityPart + 9 / 2 * velocityPart ^ 2 - 3 / 2 * Me.Velocity.LengthSquared)
                End If
            Next
        Next
    End Sub

    Private Sub setVelocityAndDensityFromFOld()
        If Not Me.IsWall Then
            Me.Density = 0
            Me.Velocity = New Vector2D

            For xIndex = 0 To 2
                For yIndex = 0 To 2
                    Me.Density += Me.FOld(xIndex, yIndex)
                    Me.Velocity += _NeighborVectors(xIndex, yIndex) * Me.FOld(xIndex, yIndex)
                Next
            Next

            Me.Velocity /= Me.Density
            'External force: Me.Velocity += force / Sqrt(3)
        End If
    End Sub

    Private Shared ReadOnly _VelocityWeights As Double(,) = New Double(2, 2) {{1 / 36, 1 / 9, 1 / 36},
                                                                              {1 / 9, 4 / 9, 1 / 9},
                                                                              {1 / 36, 1 / 9, 1 / 36}}
    Private Shared ReadOnly _NeighborVectors As Vector2D(,) = New Vector2D(2, 2) {{New Vector2D(-1, -1), New Vector2D(-1, 0), New Vector2D(-1, 1)},
                                                                                  {New Vector2D(0, -1), New Vector2D(0, 0), New Vector2D(0, 1)},
                                                                                  {New Vector2D(1, -1), New Vector2D(1, 0), New Vector2D(1, 1)}}

End Class
