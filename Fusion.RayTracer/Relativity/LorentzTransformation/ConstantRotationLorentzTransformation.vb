Public Class ConstantRotationLorentzTransformation
    Implements IAcceleratedLorentzTransformation

    Private ReadOnly _Center As Vector3D
    Private ReadOnly _StartEvent As SpaceTimeEvent
    Private ReadOnly _NormalizedAxisDirection As Vector3D
    Private ReadOnly _Velocity As Double
    Private ReadOnly _CenterToStart As Vector3D
    Private ReadOnly _Radius As Double

    Private Const _MaxRoundingDeviation = 10 ^ -6

    Public Sub New(center As Vector3D, axisDirection As Vector3D, startEvent As SpaceTimeEvent, velocity As Double)
        _Center = center
        _StartEvent = startEvent
        _NormalizedAxisDirection = axisDirection.Normalized
        _Velocity = velocity
        _CenterToStart = _StartEvent.Location - center
        _Radius = _CenterToStart.Length

        If _CenterToStart * axisDirection > _MaxRoundingDeviation Then
            Throw New InvalidOperationException("The direction from the rotation center to the start location must be orthogonal to the rotation axis.")
        End If
    End Sub

    ''' <summary>
    ''' The transformation from the inertial system into the accelerating system.
    '''  </summary>
    ''' <param name="restTime">The point of time at the rest frame.</param>
    Public Function GetTransformationAtTime(restTime As Double) As LorentzTransformation Implements IAcceleratedLorentzTransformation.GetTransformationAtTime
        Dim elapsedRestTime = restTime - _StartEvent.Time
        Dim movedDistance = elapsedRestTime * _Velocity
        Dim angle = NonnegativeNormalizedMod(movedDistance, modulo:=2 * PI * _Radius) * 2 * PI

        Dim current = _StartEvent.Location.RotateAroundAxis(axisOrigin:=_Center, axisDirection:=_NormalizedAxisDirection, angle:=angle)
        Dim centerToCurrent = current - _Center
        Dim velocity = _NormalizedAxisDirection.CrossProduct(centerToCurrent.Normalized) * _Velocity

        Dim rotationEvent = New SpaceTimeEvent(current, restTime)
        Dim rotationTime = LorentzTransformation.GetGamma(velocity:=velocity.Length) * elapsedRestTime
        Dim transformedRotationEvent = New SpaceTimeEvent(New Vector3D, rotationTime)

        Return LorentzTransformation.FromLinkEvent(relativeVelocity:=velocity, linkEvent:=rotationEvent, transformedLinkEvent:=transformedRotationEvent)
    End Function
End Class