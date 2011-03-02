''' <summary>
''' A camera view course, where the camera moves on an even path with a constant velocity and looks forward.
''' </summary>
''' <remarks></remarks>
Public Class LinearViewCourse
    Implements IViewCourse

    Public Sub New(ByVal velocity As Vector3D,
                   ByVal startLocation As Vector3D,
                   ByVal visibleXAngle As Double,
                   Optional ByVal startTime As Double = 0)
        _velocity = velocity
        _startLocation = startLocation
        _startTime = startTime
        _visibleXAngle = visibleXAngle
    End Sub

    Private _velocity As Vector3D
    Public ReadOnly Property Velocity() As Vector3D
        Get
            Return _velocity
        End Get
    End Property

    Private _startLocation As Vector3D
    Public ReadOnly Property StartLocation() As Vector3D
        Get
            Return _startLocation
        End Get
    End Property

    Private _startTime As Double
    Public ReadOnly Property StartTime() As Double
        Get
            Return _startTime
        End Get
    End Property

    Private _visibleXAngle As Double
    Public ReadOnly Property VisibleXAngle() As Double
        Get
            Return _visibleXAngle
        End Get
    End Property

    Public Function GetView(ByVal pointOfTime As Double) As Visualization.View3D Implements IViewCourse.GetView
        Dim location = _startLocation + (pointOfTime - Me.StartTime) * Me.Velocity

        Return New View3D(cameraLocation:=location, lookAt:=location + Me.Velocity, upVector:=New Vector3D(0, 1, 0), xAngleFromMinus1To1:=Me.VisibleXAngle)
    End Function
End Class
