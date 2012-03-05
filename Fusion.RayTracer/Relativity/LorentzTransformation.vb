﻿''' <summary>
''' Transforms events, velocities and view rays of a (stationary) reference frame S into a reference frame T which relativly moves with a constant velocity.
''' The events (0, 0, 0, 0) of both reference frames are the same.
''' </summary>
Public Class LorentzTransformation
    Private ReadOnly _RelativeVelocity As Vector3D
    Private ReadOnly _NormalizedRelativeVelocity As Vector3D
    Private ReadOnly _Beta As Double
    Private ReadOnly _Gamma As Double

    ''' <param name="relativeVelocity">The relative velocity of T in S.</param>
    Public Sub New(relativeVelocity As Vector3D)
        If relativeVelocity.Length >= SpeedOfLight Then Throw New ArgumentException("A velocity must be smaller than light velocity.")

        _RelativeVelocity = relativeVelocity
        _NormalizedRelativeVelocity = _RelativeVelocity.Normalized
        _Beta = _RelativeVelocity.Length / SpeedOfLight
        _Gamma = 1 / Sqrt(1 - _Beta ^ 2)
        _RelativeVelocityIsNull = (_RelativeVelocity.LengthSquared = 0)
    End Sub

    Public ReadOnly Property RelativeVelocity As Vector3D
        Get
            Return _RelativeVelocity
        End Get
    End Property

    Public ReadOnly Property NormalizedRelativeVelocity As Vector3D
        Get
            Return _NormalizedRelativeVelocity
        End Get
    End Property

    Public ReadOnly Property Beta() As Double
        Get
            Return _Beta
        End Get
    End Property

    Public ReadOnly Property Gamma() As Double
        Get
            Return _Gamma
        End Get
    End Property

    Private ReadOnly _RelativeVelocityIsNull As Boolean

    Protected ReadOnly Property RelativeVelocityIsNull As Boolean
        Get
            Return _RelativeVelocityIsNull
        End Get
    End Property

    ''' <summary>
    ''' Transforms the direction, but not the origin location of the sight ray.
    ''' </summary>
    Public Function InverseSemiTransformSightRay(sightRayInTWithOriginInS As Ray) As Ray
        Return New Ray(origin:=sightRayInTWithOriginInS.Origin,
                       direction:=Inverse.TransformSightRayDirection(sightRayInTWithOriginInS.NormalizedDirection))
    End Function

    Private Function TransformSightRayDirection(sightRayDirection As Vector3D) As Vector3D
        Return -TransformVelocity(-sightRayDirection.Normalized.ScaledToLength(SpeedOfLight))
    End Function

    ''' <param name="event">An event in S.</param>
    ''' <returns>The corresponding event in T.</returns>
    Public Function TransformEvent([event] As SpaceTimeEvent) As SpaceTimeEvent
        If _RelativeVelocityIsNull Then Return [event]

        Return New SpaceTimeEvent(time:=_Gamma * ([event].Time - (_RelativeVelocity.Length * (_NormalizedRelativeVelocity * [event].Location)) / SpeedOfLight ^ 2),
                                  location:=[event].Location + (_Gamma - 1) * (_NormalizedRelativeVelocity * [event].Location) * _NormalizedRelativeVelocity - _Gamma * [event].Time * _RelativeVelocity)
    End Function

    Public Function Inverse() As LorentzTransformation
        Static state As LorentzTransformation

        If state Is Nothing Then state = New LorentzTransformation(-_RelativeVelocity)

        Return state
    End Function

    ''' <param name="velocity">A velocity in S.</param>
    ''' <returns>The corresponding velocity in T.</returns>
    Public Function TransformVelocity(velocity As Vector3D) As Vector3D
        If _RelativeVelocityIsNull Then Return velocity

        Dim ux = velocity.OrthogonalProjectionOn(_NormalizedRelativeVelocity)

        Return 1 / (1 - _RelativeVelocity * velocity / SpeedOfLight ^ 2) *
            ((velocity - ux) / _Gamma + _NormalizedRelativeVelocity * (ux * _NormalizedRelativeVelocity - _RelativeVelocity.Length))
    End Function

    Public Function Before(second As LorentzTransformation) As LorentzTransformation
        Return New LorentzTransformation(RelativeVelocity:=Inverse.TransformVelocity(second.RelativeVelocity))
    End Function

    Public Function TransformSightRay(sightRay As SightRay) As SightRay
        Return New SightRay(originEvent:=TransformEvent(sightRay.OriginEvent), direction:=TransformSightRayDirection(sightRay.Ray.NormalizedDirection))
    End Function

    Public Function AtDirection(sightRayDirectionInS As Vector3D) As LorentzTransformationAtSightRayDirection
        Return New LorentzTransformationAtSightRayDirection(RelativeVelocity:=RelativeVelocity, sightRayDirectionInS:=sightRayDirectionInS)
    End Function
End Class