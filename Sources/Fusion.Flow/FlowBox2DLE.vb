''' <summary>
''' For the Large Eddie Simulation.
''' </summary>
''' <remarks></remarks>
Public Class FlowBox2DLE
    Implements IFlowBox2D

    Public Property IsWall As Boolean Implements IFlowBox2D.IsWall

    Public Property Density As Double Implements IFlowBox2D.Density
    Public Property Velocity As Vector2D Implements IFlowBox2D.Velocity

    Public Property Smoke As Double Implements IFlowBox2D.Smoke

    Public Property Lambda As Double
    Private _DefaultLambda As Double
    Private _DefaultViscosity As Double
    Private Property defaultViscosity As Double
        Get
            Return _DefaultViscosity
        End Get
        Set(value As Double)
            _DefaultViscosity = value
            _DefaultLambda = 1 / (1 / 2 + 3 * _DefaultViscosity)
        End Set
    End Property


    Public Property FEq As Double(,)
    Public Property FNew As Double(,)
    Public Property FOld As Double(,)
    Private Property Omega As Double(,)

    Public Sub New(isWall As Boolean, density As Double, velocity As Vector2D, viscosity As Double)
        Me.IsWall = isWall
        Me.Density = density

        ReDim FEq(2, 2), FNew(2, 2), Omega(1, 1)

        defaultViscosity = viscosity

        Me.Velocity = velocity
        setFEqFromVelocity()
        copyFNewFromFeq()
    End Sub

    Private Sub copyFNewFromFeq()
        Me.FNew = DirectCast(Me.FEq.Clone, Double(,))
    End Sub


    Public Overridable Sub NextTimeStep()
        If Not Me.IsWall Then
            copyFOldFromFNew()
            setVelocityAndDensityFromFOld()
            setFEqFromVelocity()
            setOmega()
            setLambda()
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
                    Me.FEq(columnIndex, rowIndex) = _VelocityWeights(columnIndex, rowIndex) * _Density * (1 + 3 * velocityPart + 9 / 2 * velocityPart ^ 2 - 3 / 2 * (Me.Velocity.LengthSquared))
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
        End If
    End Sub

    Private Shared ReadOnly _VelocityWeights As Double(,) = New Double(2, 2) {{1 / 36, 1 / 9, 1 / 36},
                                                                              {1 / 9, 4 / 9, 1 / 9},
                                                                              {1 / 36, 1 / 9, 1 / 36}}
    Private Shared ReadOnly _NeighborVectors As Vector2D(,) = New Vector2D(2, 2) {{New Vector2D(-1, -1), New Vector2D(-1, 0), New Vector2D(-1, 1)},
                                                                                  {New Vector2D(0, -1), New Vector2D(0, 0), New Vector2D(0, 1)},
                                                                                  {New Vector2D(1, -1), New Vector2D(1, 0), New Vector2D(1, 1)}}

    Private Sub setLambda()
        Me.Lambda = 1 / (1 / _DefaultLambda + (Sqrt(1 / _DefaultLambda ^ 2 + Sqrt(2) * 18 * 0.18 ^ 2 * getOmega()) - 1 / _DefaultLambda) / 2)
    End Sub


    Private Function getOmega() As Double
        getOmega = 0

        For Each component In Me.Omega
            getOmega += component ^ 2
        Next

        Return Sqrt(getOmega)
    End Function

    Private Sub setOmega()
        For x = 0 To 1
            For y = 0 To 1
                Dim sum As Double = 0
                For innerX = 0 To 2
                    For innerY = 0 To 2
                        Dim factor As Double
                        If x = 0 Then
                            If y = 0 Then
                                factor = _NeighborVectors(innerX, innerY).X * _NeighborVectors(innerX, innerY).Y
                            Else
                                factor = _NeighborVectors(innerX, innerY).X * _NeighborVectors(innerX, innerY).X
                            End If
                        Else
                            If y = 0 Then
                                factor = _NeighborVectors(innerX, innerY).Y * _NeighborVectors(innerX, innerY).Y
                            Else
                                factor = _NeighborVectors(innerX, innerY).Y * _NeighborVectors(innerX, innerY).X
                            End If
                        End If

                        sum += (Me.FNew(innerX, innerY) - Me.FEq(innerX, innerY)) * factor
                    Next
                Next
                Me.Omega(x, y) = sum
            Next
        Next
    End Sub
End Class
