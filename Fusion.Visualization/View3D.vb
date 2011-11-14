Public Class View3D

    Public Sub New(observerLocation As Vector3D, lookAt As Vector3D, upDirection As Vector3D, horizontalViewAngle As Double)
        If observerLocation = lookAt Then Throw New ArgumentException("Observer location must not be lookAt location.")
        _NormalizedLookDirection = (lookAt - observerLocation).Normalized

        If upDirection.LengthSquared = 0 Then Throw New ArgumentException("Up direction must not be null vector.")
        _NormalizedUpDirection = upDirection.Normalized

        If horizontalViewAngle < 0 OrElse horizontalViewAngle >= PI Then Throw New ArgumentException("Horizontal view angle must be in [0,pi).")

        _ObserverLocation = observerLocation
        _LookAt = lookAt
        _HorizontalViewAngle = horizontalViewAngle
        _NormalizedRightVectorInViewPlane = _NormalizedLookDirection.CrossProduct(_NormalizedUpDirection)
        If _NormalizedRightVectorInViewPlane.LengthSquared = 0 Then Throw New ArgumentException("Look direction must not be parallel to up direction.")

        _ViewPlaneToCameraDistance = 1 / Tan(horizontalViewAngle / 2)
        _ViewPlaneDistanceVector = _NormalizedLookDirection * _ViewPlaneToCameraDistance
        _NormalizedUpVectorInViewPlane = _NormalizedRightVectorInViewPlane.CrossProduct(_NormalizedLookDirection)
    End Sub

    Private ReadOnly _HorizontalViewAngle As Double
    Public ReadOnly Property HorizontalViewAngle As Double
        Get
            Return _HorizontalViewAngle
        End Get
    End Property

    Private ReadOnly _ObserverLocation As Vector3D
    Public ReadOnly Property ObserverLocation As Vector3D
        Get
            Return _ObserverLocation
        End Get
    End Property

    Private ReadOnly _LookAt As Vector3D
    Public ReadOnly Property LookAt As Vector3D
        Get
            Return _LookAt
        End Get
    End Property

    Private _NormalizedLookDirection As Vector3D
    Public ReadOnly Property NormalizedLookDirection As Vector3D
        Get
            Return _NormalizedLookDirection
        End Get
    End Property

    Private _NormalizedUpDirection As Vector3D
    Public ReadOnly Property NormalizedUpDirection As Vector3D
        Get
            Return _NormalizedUpDirection
        End Get
    End Property

    Private _ViewPlaneDistanceVector As Vector3D
    Private _ViewPlaneToCameraDistance As Double
    Private _NormalizedRightVectorInViewPlane As Vector3D
    Private _NormalizedUpVectorInViewPlane As Vector3D

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="viewPlaneLocation">The view plane is visible if viewPlaneLocation.X and viewPlaneLocation.Y are in [-1; 1].</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SightRay(viewPlaneLocation As Vector2D) As Ray
        Dim sightVectorInViewPlane = _NormalizedRightVectorInViewPlane * viewPlaneLocation.X + _NormalizedUpVectorInViewPlane * viewPlaneLocation.Y
        Return New Ray(origin:=Me.ObserverLocation, Direction:=_ViewPlaneDistanceVector + sightVectorInViewPlane)
    End Function

End Class
