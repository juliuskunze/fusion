Public Class StaticFlowBox2DLE
    Inherits FlowBox2DLE

    Public Property KeptVelocity As Vector2D
    Private Property KeptDensity As Double

    Public Sub New(flowBoxToKeep As FlowBox2DLE)
        MyBase.New(IsWall:=flowBoxToKeep.IsWall, Density:=flowBoxToKeep.Density, Velocity:=flowBoxToKeep.Velocity, viscosity:=0)
    End Sub

    Public Sub New(isWall As Boolean, keptDensity As Double, keptVelocity As Vector2D, viscosity As Double)
        MyBase.New(isWall:=isWall, Density:=keptDensity, Velocity:=keptVelocity, viscosity:=viscosity)

        Me.KeptVelocity = keptVelocity
        Me.KeptDensity = keptDensity
    End Sub

    Public Overrides Sub NextTimeStep()
        If Not Me.IsWall Then
            MyBase.NextTimeStep()

            Me.Density = Me.KeptDensity
            Me.Velocity = New Vector2D
            setFEqFromVelocity()
            Me.FOld = DirectCast(Me.FEq.Clone, Double(,))
        End If
    End Sub
End Class
