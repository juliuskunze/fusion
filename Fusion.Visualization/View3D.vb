Public Class View3D

    Public Sub New(ByVal cameraLocation As Vector3D, ByVal lookAt As Vector3D, ByVal upVector As Vector3D, ByVal xAngleFromMinus1To1 As Double)
        Me.CameraLocation = cameraLocation
        Me.LookAt = lookAt
        Me.UpVector = upVector
        Me.AngleFromMinus1To1 = xAngleFromMinus1To1
    End Sub

    Private _angleFromMinus1To1 As Double
    Public Property AngleFromMinus1To1 As Double
        Get
            Return _angleFromMinus1To1
        End Get
        Set(ByVal value As Double)
            _angleFromMinus1To1 = value
            Me.ViewPlaneToCameraDistance = 1 / Tan(value / 2)
        End Set
    End Property

    Private _cameraLocation As Vector3D
    Public Property CameraLocation As Vector3D
        Get
            Return _cameraLocation
        End Get
        Set(ByVal value As Vector3D)
            _cameraLocation = value
            adaptDirection()
        End Set
    End Property

    Private _lookAt As Vector3D
    Public Property LookAt As Vector3D
        Get
            Return _lookAt
        End Get
        Set(ByVal value As Vector3D)
            _lookAt = value
            adaptDirection()
        End Set
    End Property

    Private Sub adaptDirection()
        Me.Direction = Me.LookAt - Me.CameraLocation
    End Sub
    Public WriteOnly Property Direction As Vector3D
        Set(ByVal value As Vector3D)
            _normalizedDirection = value.Normalized
            adaptViewPlaneDistanceVector()
            adaptNormalizedVectorsInViewPlane()
        End Set
    End Property

    Private _normalizedDirection As Vector3D
    Public ReadOnly Property NormalizedDirection As Vector3D
        Get
            Return _normalizedDirection
        End Get
    End Property

    Private _upVector As Vector3D
    Public Property UpVector As Vector3D
        Get
            Return _upVector
        End Get
        Set(ByVal value As Vector3D)
            _upVector = value
            _normalizedUpVector = Me.UpVector.Normalized
            adaptNormalizedVectorsInViewPlane()
        End Set
    End Property

    Private _normalizedUpVector As Vector3D
    Public ReadOnly Property NormalizedUpVector As Vector3D
        Get
            Return _normalizedUpVector
        End Get
    End Property

    Private Sub adaptViewPlaneDistanceVector()
        _viewPlaneDistanceVector = Me.NormalizedDirection * Me.ViewPlaneToCameraDistance
    End Sub
    Private _viewPlaneDistanceVector As Vector3D

    Private _viewPlaneToCameraDistance As Double
    Private Property ViewPlaneToCameraDistance As Double
        Get
            Return _viewPlaneToCameraDistance
        End Get
        Set(ByVal value As Double)
            _viewPlaneToCameraDistance = value
            Me.adaptViewPlaneDistanceVector()
        End Set
    End Property

    Private _normalizedRightVectorInViewPlane As Vector3D
    Private _normalizedUpVectorInViewPlane As Vector3D

    Private Sub adaptNormalizedVectorsInViewPlane()
        _normalizedRightVectorInViewPlane = Me.NormalizedDirection.CrossProduct(Me.NormalizedUpVector)
        adaptNormalizedUpVectorInViewPlane()
    End Sub

    Private Sub adaptNormalizedUpVectorInViewPlane()
        _normalizedUpVectorInViewPlane = _normalizedRightVectorInViewPlane.CrossProduct(Me.NormalizedDirection)
    End Sub

    Public Function SightRay(ByVal viewPlaneLocation As Vector2D) As Ray
        Dim sightVectorInViewPlane = _normalizedRightVectorInViewPlane * viewPlaneLocation.X + _normalizedUpVectorInViewPlane * viewPlaneLocation.Y
        Return New Ray(origin:=Me.CameraLocation, Direction:=_viewPlaneDistanceVector + sightVectorInViewPlane)
    End Function

End Class
