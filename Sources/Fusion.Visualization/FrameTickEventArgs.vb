Public Class FrameTickEventArgs
    Inherits EventArgs

    Public Sub New(timeStep As Double, calcsPerFrame As Double)
        Me.TimeStep = timeStep
        Me.CalcsPerFrame = calcsPerFrame
    End Sub

    Public TimeStep As Double
    Public CalcsPerFrame As Double
End Class
