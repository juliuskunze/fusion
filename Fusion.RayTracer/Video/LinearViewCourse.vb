''' <summary>
''' A camera view course, where an observer moves on an even path with a constant velocity and looks forward.
''' </summary>
''' <remarks></remarks>
Public Class LinearViewCourse
    Implements IViewCourse

    Public Sub New(velocity As Vector3D,
                    startLocation As Vector3D,
                    visibleXAngle As Double,
                   Optional startTime As Double = 0)
        _Velocity = velocity
        _StartLocation = startLocation
        _StartTime = startTime
        _VisibleXAngle = visibleXAngle
    End Sub

    Private _Velocity As Vector3D
    Public ReadOnly Property Velocity() As Vector3D
        Get
            Return _Velocity
        End Get
    End Property

    Private _StartLocation As Vector3D
    Public ReadOnly Property StartLocation() As Vector3D
        Get
            Return _StartLocation
        End Get
    End Property

    Private _StartTime As Double
    Public ReadOnly Property StartTime() As Double
        Get
            Return _StartTime
        End Get
    End Property

    Private _VisibleXAngle As Double
    Public ReadOnly Property VisibleXAngle() As Double
        Get
            Return _VisibleXAngle
        End Get
    End Property

    Public Function GetView(pointOfTime As Double) As Visualization.View3D Implements IViewCourse.GetView
        Dim location = _StartLocation + (pointOfTime - Me.StartTime) * Me.Velocity

        Return New View3D(observerLocation:=location, lookAt:=location + Me.Velocity, upDirection:=New Vector3D(0, 1, 0), horizontalViewAngle:=Me.VisibleXAngle)
    End Function
End Class
