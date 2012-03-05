''' <summary>
''' Represents the events moving from an origin event into one direction backward in time with light velocity.
''' </summary>
Public Class SightRay
    Private ReadOnly _Ray As Ray
    Private ReadOnly _OriginTime As Double

    Public Sub New(ray As Ray, originTime As Double)
        _Ray = ray
        _OriginTime = originTime
    End Sub

    Public Sub New(originEvent As SpaceTimeEvent, direction As Vector3D)
        Me.New(Ray:=New Ray(originEvent.Location, direction), OriginTime:=originEvent.Time)
    End Sub

    Public ReadOnly Property OriginEvent As SpaceTimeEvent
        Get
            Return New SpaceTimeEvent(_OriginTime, _Ray.Origin)
        End Get
    End Property

    Public ReadOnly Property OriginTime As Double
        Get
            Return _OriginTime
        End Get
    End Property

    Public ReadOnly Property OriginLocation As Vector3D
        Get
            Return _Ray.Origin
        End Get
    End Property

    Public ReadOnly Property Ray As Ray
        Get
            Return _Ray
        End Get
    End Property

    Public Function GetTime(distanceFromOrigin As Double) As Double
        Return _OriginTime - SpeedOfLight * distanceFromOrigin
    End Function

    Public Function GetEvent(distanceFromOrigin As Double) As SpaceTimeEvent
        Return New SpaceTimeEvent(GetTime(distanceFromOrigin), Ray.PointOnRay(distanceFromOrigin))
    End Function

End Class
