''' <summary>
''' Transforms events, velocities and view rays of a (stationary) reference frame S into a reference frame T which relativly moves with a constant velocity.
''' </summary>
Public Class LorentzTransformation
    Private ReadOnly _OriginOfTransformed As SpaceTimeEvent
    Private ReadOnly _RelativeVelocity As Vector3D
    Private ReadOnly _NormalizedRelativeVelocity As Vector3D
    Private ReadOnly _Beta As Double
    Private ReadOnly _Gamma As Double

    ''' <param name="relativeVelocity">The relative velocity of T (transformed) in S (original).</param>
    Public Sub New(relativeVelocity As Vector3D)
        Me.New(relativeVelocity:=relativeVelocity, originOfTransformed:=New SpaceTimeEvent)
    End Sub

    ''' <param name="relativeVelocity">The relative velocity of T (transformed) in S (original).</param>
    ''' <param name="originOfTransformed">The event in S (original) where the origin event of T (transformed) is located.</param>
    Public Sub New(relativeVelocity As Vector3D, originOfTransformed As SpaceTimeEvent)
        _OriginOfTransformed = originOfTransformed
        If relativeVelocity.Length >= SpeedOfLight Then Throw New ArgumentException("A velocity of a reference frame must be smaller than light velocity.")

        _RelativeVelocity = relativeVelocity
        _NormalizedRelativeVelocity = _RelativeVelocity.Normalized
        _Beta = GetBeta(velocity:=_RelativeVelocity.Length)
        _Gamma = 1 / Sqrt(1 - _Beta ^ 2)
        _RelativeVelocityIsNull = (_RelativeVelocity.LengthSquared = 0)
    End Sub

    Public Shared Function FromLinkEvent(relativeVelocity As Vector3D, linkEvent As SpaceTimeEvent, transformedLinkEvent As SpaceTimeEvent) As LorentzTransformation
        Return New LorentzTransformation(relativeVelocity, originOfTransformed:=linkEvent).Before(New LorentzTransformation(New Vector3D, originOfTransformed:=-transformedLinkEvent))
    End Function

    Private Shared Function GetBeta(velocity As Double) As Double
        Return velocity / SpeedOfLight
    End Function

    Public Shared Function GetGamma(velocity As Double) As Double
        Return 1 / Sqrt(1 - GetBeta(velocity) ^ 2)
    End Function

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

    Public ReadOnly Property Beta As Double
        Get
            Return _Beta
        End Get
    End Property

    Public ReadOnly Property Gamma As Double
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

    Public ReadOnly Property OriginOfTransformed() As SpaceTimeEvent
        Get
            Return _OriginOfTransformed
        End Get
    End Property

    ''' <param name="event">An event in S.</param>
    ''' <returns>The corresponding event in T.</returns>
    Public Function TransformEvent([event] As SpaceTimeEvent) As SpaceTimeEvent
        Dim translated = [event] - _OriginOfTransformed

        If _RelativeVelocityIsNull Then Return translated

        Dim d = _NormalizedRelativeVelocity * translated.Location
        Return New SpaceTimeEvent(location:=translated.Location + (_Gamma - 1) * d * _NormalizedRelativeVelocity - _Gamma * translated.Time * _RelativeVelocity,
                                  time:=_Gamma * (translated.Time - (_RelativeVelocity.Length * d) / SpeedOfLight ^ 2))
    End Function

    Public Overridable Function Inverse() As LorentzTransformation
        Static lazy As New Lazy(Of LorentzTransformation)(Function() New LorentzTransformation(-_RelativeVelocity, originOfTransformed:=TransformEvent(New SpaceTimeEvent)))
        Return lazy.Value
    End Function

    ''' <param name="velocity">A velocity in S.</param>
    ''' <returns>The corresponding velocity in T.</returns>
    Public Function TransformVelocity(velocity As Vector3D) As Vector3D
        If _RelativeVelocityIsNull Then Return velocity

        Dim ux = velocity.OrthogonalProjectionOn(_NormalizedRelativeVelocity)

        Return 1 / (1 - _RelativeVelocity * velocity / SpeedOfLight ^ 2) *
            ((velocity - ux) / _Gamma + _NormalizedRelativeVelocity * (ux * _NormalizedRelativeVelocity - _RelativeVelocity.Length))
    End Function

    ''' <summary>
    ''' The transformation from the original frame (S) of this instance to transformed reference frame (T) of the second transformation.
    ''' </summary>
    ''' <param name="second"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Before(second As LorentzTransformation) As LorentzTransformation
        Return New LorentzTransformation(RelativeVelocity:=Inverse.TransformVelocity(second.RelativeVelocity), originOfTransformed:=Inverse.TransformEvent(second.OriginOfTransformed))
    End Function

    Public Function TransformSightRay(sightRay As SightRay) As SightRay
        Return New SightRay(originEvent:=TransformEvent(sightRay.OriginEvent),
                            direction:=AtSightRay(sightRay).TransformSightRayDirection)
    End Function

    Public Function AtSightRay(sightRay As SightRay) As LorentzTransformationAtSightRay
        Return New LorentzTransformationAtSightRay(RelativeVelocity:=RelativeVelocity, sightRay:=sightRay)
    End Function
End Class
