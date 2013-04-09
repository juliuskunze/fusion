Public Class ConstantAccelerationLorentzTransformation
    Private ReadOnly _Acceleration As Double

    Public Sub New(acceleration As Double)
        _Acceleration = acceleration
    End Sub

    Public Function GetTransformation(acceleratedFrameTime As Double) As LorentzTransformation
        Dim velocity = SpeedOfLight * Tanh(_Acceleration * acceleratedFrameTime / SpeedOfLight)

        Return New LorentzTransformation(New Vector3D)
    End Function
End Class