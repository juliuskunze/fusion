Public Class StaticFlowBox2D
    Inherits FlowBox2D

    Public Property KeptVelocity As Vector2D
    Private Property KeptDensity As Double

    Public Sub New(flowBoxToKeep As FlowBox2D)
        MyBase.New(IsWall:=flowBoxToKeep.IsWall, Density:=flowBoxToKeep.Density, Velocity:=flowBoxToKeep.Velocity)
    End Sub

    Public Sub New(isWall As Boolean, keptDensity As Double, keptVelocity As Vector2D)
        MyBase.New(isWall:=isWall, Density:=keptDensity, Velocity:=keptVelocity)

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