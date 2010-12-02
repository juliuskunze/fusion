Public Class FrameTickEventArgs
    Inherits EventArgs

    Public Sub New(ByVal timeStep As Double, ByVal calcsPerFrame As Double)
        Me.TimeStep = timeStep
        Me.CalcsPerFrame = calcsPerFrame
    End Sub

    Public TimeStep As Double
    Public CalcsPerFrame As Double
End Class
