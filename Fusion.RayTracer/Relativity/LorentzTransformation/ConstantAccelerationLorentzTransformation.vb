Public Class ConstantAccelerationLorentzTransformation
    Implements IAcceleratedLorentzTransformation

    Private ReadOnly _Acceleration As Vector3D
    Private ReadOnly a As Double
    Private ReadOnly _NormalizedAcceleration As Vector3D
    Private Const c = SpeedOfLight

    Public Sub New(acceleration As Vector3D)
        _Acceleration = acceleration
        _NormalizedAcceleration = acceleration.Normalized
        a = _Acceleration.Length
    End Sub

    ''' <summary>
    ''' The transformation from the inertial system into the accelerating system.
    ''' </summary>
    Public Function GetTransformationAtTime(acceleratedFrameTime As Double) As LorentzTransformation Implements IAcceleratedLorentzTransformation.GetTransformationAtTime
        Dim T = acceleratedFrameTime

        ' see http://math.ucr.edu/home/baez/physics/Relativity/SR/rocket.html
        Dim restTime = (c / a) * Sinh(a * T / c)
        Dim d = (c ^ 2 / a) * (Cosh(a * T / c) - 1)
        Dim v = c * Tanh(a * T / c)
        Dim velocity = v * _NormalizedAcceleration
        Dim location = d * _NormalizedAcceleration

        Return LorentzTransformation.FromLinkEvent(velocity, linkEvent:=New SpaceTimeEvent(location, time:=restTime), transformedLinkEvent:=New SpaceTimeEvent(time:=acceleratedFrameTime))
    End Function
End Class