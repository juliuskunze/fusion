Public Class SpaceTimeEvent
    Private ReadOnly _Time As Double
    Private ReadOnly _Location As Vector3D

    Public Sub New(Optional location As Vector3D = Nothing, Optional time As Double = 0)
        _Time = time
        _Location = location
    End Sub

    Public ReadOnly Property Location() As Vector3D
        Get
            Return _Location
        End Get
    End Property

    Public ReadOnly Property Time() As Double
        Get
            Return _Time
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return ToString("0.00")
    End Function

    Public Overloads Function ToString(numberFormat As String) As String
        Return String.Format("({0}, {1})", _Time.ToString(numberFormat), _Location.ToString(numberFormat))
    End Function

    Public Overrides Function Equals(obj As Object) As Boolean
        Dim other = TryCast(obj, SpaceTimeEvent)
        If other Is Nothing Then Return False
        Return Me = other
    End Function

    Public Shared Operator =(e1 As SpaceTimeEvent, e2 As SpaceTimeEvent) As Boolean
        Return e1.Time = e2.Time AndAlso e1.Location = e2.Location
    End Operator

    Public Shared Operator <>(e1 As SpaceTimeEvent, e2 As SpaceTimeEvent) As Boolean
        Return Not e1 = e2
    End Operator

    Public Shared Operator +(e1 As SpaceTimeEvent, e2 As SpaceTimeEvent) As SpaceTimeEvent
        Return New SpaceTimeEvent(e1.Location + e2.Location, e1.Time + e2.Time)
    End Operator

    Public Shared Operator -(e1 As SpaceTimeEvent, e2 As SpaceTimeEvent) As SpaceTimeEvent
        Return New SpaceTimeEvent(e1.Location - e2.Location, e1.Time - e2.Time)
    End Operator

    Public Shared Operator -(e As SpaceTimeEvent) As SpaceTimeEvent
        Return New SpaceTimeEvent(-e.Location, -e.Time)
    End Operator
End Class

