Public Class View3D

    Public Sub New(ByVal observerLocation As Vector3D, ByVal lookAt As Vector3D, ByVal upVector As Vector3D, ByVal horizontalViewAngle As Double)
        Me.CameraLocation = observerLocation
        Me.LookAt = lookAt
        Me.UpVector = upVector
        Me.HorizontalViewAngle = horizontalViewAngle
    End Sub

    Private _HorizontalViewAngle As Double
    Public Property HorizontalViewAngle As Double
        Get
            Return _HorizontalViewAngle
        End Get
        Set(ByVal value As Double)
            _HorizontalViewAngle = value
            Me.ViewPlaneToCameraDistance = 1 / Tan(value / 2)
        End Set
    End Property

    Private _CameraLocation As Vector3D
    Public Property CameraLocation As Vector3D
        Get
            Return _CameraLocation
        End Get
        Set(ByVal value As Vector3D)
            _CameraLocation = value
            Me.AdaptDirection()
        End Set
    End Property

    Private _LookAt As Vector3D
    Public Property LookAt As Vector3D
        Get
            Return _LookAt
        End Get
        Set(ByVal value As Vector3D)
            _LookAt = value
            Me.AdaptDirection()
        End Set
    End Property

    Private Sub AdaptDirection()
        Me.Direction = Me.LookAt - Me.CameraLocation
    End Sub
    Public WriteOnly Property Direction As Vector3D
        Set(ByVal value As Vector3D)
            _NormalizedDirection = value.Normalized
            adaptViewPlaneDistanceVector()
            adaptNormalizedVectorsInViewPlane()
        End Set
    End Property

    Private _NormalizedDirection As Vector3D
    Public ReadOnly Property NormalizedDirection As Vector3D
        Get
            Return _NormalizedDirection
        End Get
    End Property

    Private _UpVector As Vector3D
    Public Property UpVector As Vector3D
        Get
            Return _UpVector
        End Get
        Set(ByVal value As Vector3D)
            _UpVector = value
            _NormalizedUpVector = Me.UpVector.Normalized
            adaptNormalizedVectorsInViewPlane()
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
        Set(ByVal value As Double)
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
    Public Function SightRay(ByVal viewPlaneLocation As Vector2D) As Ray
        Dim sightVectorInViewPlane = _NormalizedRightVectorInViewPlane * viewPlaneLocation.X + _NormalizedUpVectorInViewPlane * viewPlaneLocation.Y
        Return New Ray(origin:=Me.CameraLocation, Direction:=_ViewPlaneDistanceVector + sightVectorInViewPlane)
    End Function

End Class
