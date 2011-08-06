Public Class View3D

    Public Sub New(observerLocation As Vector3D, lookAt As Vector3D, upDirection As Vector3D, horizontalViewAngle As Double)
        Me.CameraLocation = observerLocation
        Me.LookAt = lookAt
        Me.UpDirection = upDirection
        Me.HorizontalViewAngle = horizontalViewAngle
    End Sub

    Private _HorizontalViewAngle As Double
    Public Property HorizontalViewAngle As Double
        Get
            Return _HorizontalViewAngle
        End Get
        Set(value As Double)
            _HorizontalViewAngle = value
            Me.ViewPlaneToCameraDistance = 1 / Tan(value / 2)
        End Set
    End Property

    Private _CameraLocation As Vector3D
    Public Property CameraLocation As Vector3D
        Get
            Return _CameraLocation
        End Get
        Set(value As Vector3D)
            _CameraLocation = value
            Me.AdaptDirection()
        End Set
    End Property

    Private _LookAt As Vector3D
    Public Property LookAt As Vector3D
        Get
            Return _LookAt
        End Get
        Set(value As Vector3D)
            _LookAt = value
            Me.AdaptDirection()
        End Set
    End Property

    Private Sub AdaptDirection()
        Me.Direction = Me.LookAt - Me.CameraLocation
    End Sub
    Public WriteOnly Property Direction As Vector3D
        Set(value As Vector3D)
            _NormalizedDirection = value.Normalized
            AdaptViewPlaneDistanceVector()
            AdaptNormalizedVectorsInViewPlane()
        End Set
    End Property

    Private _NormalizedDirection As Vector3D
    Public ReadOnly Property NormalizedDirection As Vector3D
        Get
            Return _NormalizedDirection
        End Get
    End Property

    Private _UpDirection As Vector3D
    Public Property UpDirection As Vector3D
        Get
            Return _UpDirection
        End Get
        Set(value As Vector3D)
            _UpDirection = value
            _NormalizedUpVector = Me.UpDirection.Normalized
            AdaptNormalizedVectorsInViewPlane()
        End Set
    End Property

    Private _NormalizedUpVector As Vector3D
    Public ReadOnly Property NormalizedUpVector As Vector3D
        Get
            Return _NormalizedUpVector
        End Get
    End Property

    Private Sub AdaptViewPlaneDistanceVector()
        _ViewPlaneDistanceVector = Me.NormalizedDirection * Me.ViewPlaneToCameraDistance
    End Sub
    Private _ViewPlaneDistanceVector As Vector3D

    Private _ViewPlaneToCameraDistance As Double
    Private Property ViewPlaneToCameraDistance As Double
        Get
            Return _ViewPlaneToCameraDistance
        End Get
        Set(value As Double)
            _ViewPlaneToCameraDistance = value
            Me.AdaptViewPlaneDistanceVector()
        End Set
    End Property

    Private _NormalizedRightVectorInViewPlane As Vector3D
    Private _NormalizedUpVectorInViewPlane As Vector3D

    Private Sub AdaptNormalizedVectorsInViewPlane()
        _NormalizedRightVectorInViewPlane = Me.NormalizedDirection.CrossProduct(Me.NormalizedUpVector)
        Me.AdaptNormalizedUpVectorInViewPlane()
    End Sub

    Private Sub AdaptNormalizedUpVectorInViewPlane()
        _NormalizedUpVectorInViewPlane = _NormalizedRightVectorInViewPlane.CrossProduct(Me.NormalizedDirection)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="viewPlaneLocation">The view plane is visible if viewPlaneLocation.X and viewPlaneLocation.Y are in [-1; 1].</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SightRay(viewPlaneLocation As Vector2D) As Ray
        Dim sightVectorInViewPlane = _NormalizedRightVectorInViewPlane * viewPlaneLocation.X + _NormalizedUpVectorInViewPlane * viewPlaneLocation.Y
        Return New Ray(origin:=Me.CameraLocation, Direction:=_ViewPlaneDistanceVector + sightVectorInViewPlane)
    End Function

End Class
