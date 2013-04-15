Public Class ConstantRotationLorentzTransformation
    Implements IAcceleratedLorentzTransformation

    Private ReadOnly _Center As Vector3D
    Private ReadOnly _StartEvent As SpaceTimeEvent
    Private ReadOnly _NormalizedAxisDirection As Vector3D
    Private ReadOnly _Velocity As Double
    Private ReadOnly _CenterToStart As Vector3D
    Private ReadOnly _Radius As Double
    Private ReadOnly _Circumference As Double
    Private ReadOnly _Gamma As Double

    Private Const _MaxRoundingDeviation = 10 ^ -6

    Public Sub New(center As Vector3D, axisDirection As Vector3D, startEvent As SpaceTimeEvent, velocity As Double)
        _Center = center
        _StartEvent = startEvent
        _NormalizedAxisDirection = axisDirection.Normalized
        _Velocity = velocity
        _CenterToStart = _StartEvent.Location - center
        _Radius = _CenterToStart.Length
        _Circumference = 2 * PI * _Radius
        _Gamma = LorentzTransformation.GetGamma(_Velocity)

        If _CenterToStart * axisDirection > _MaxRoundingDeviation Then
            Throw New InvalidOperationException("The direction from the rotation center to the start location must be orthogonal to the rotation axis.")
        End If
    End Sub

    ''' <summary>
    ''' The transformation from the inertial system into the accelerating system.
    '''  </summary>
    Public Function InertialToAcceleratedInertial(acceleratedFrameTime As Double) As LorentzTransformation Implements IAcceleratedLorentzTransformation.InertialToAcceleratedInertial
        Dim restTime = acceleratedFrameTime / _Gamma

        Dim elapsedRestTime = restTime - _StartEvent.Time
        Dim movedDistance = elapsedRestTime * _Velocity
        Dim angle = movedDistance / _Circumference * 2 * PI

        Dim current = _StartEvent.Location.RotateAroundAxis(axisOrigin:=_Center, axisDirection:=_NormalizedAxisDirection, angle:=angle)
        Dim centerToCurrent = current - _Center
        Dim velocity = _NormalizedAxisDirection.CrossProduct(centerToCurrent.Normalized) * _Velocity

        Dim rotationEvent = New SpaceTimeEvent(current, restTime)
        Dim transformedTime = LorentzTransformation.GetGamma(velocity:=velocity.Length) * elapsedRestTime
        Dim transformedRotationEvent = New SpaceTimeEvent(New Vector3D, transformedTime)

        Return LorentzTransformation.FromLinkEvent(relativeVelocity:=velocity, linkEvent:=rotationEvent, transformedLinkEvent:=transformedRotationEvent)
    End Function
End Class