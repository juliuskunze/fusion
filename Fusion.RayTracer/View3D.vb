Public Class View3D
    Public Sub New(observerEvent As SpaceTimeEvent, lookAt As Vector3D, upDirection As Vector3D, horizontalViewAngle As Double)
        If observerEvent.Location = lookAt Then Throw New ArgumentException("Observer location must not be lookAt location.")
        _NormalizedLookDirection = (lookAt - observerEvent.Location).Normalized

        If upDirection.LengthSquared = 0 Then Throw New ArgumentException("Up direction must not be null vector.")
        _NormalizedUpDirection = upDirection.Normalized

        If horizontalViewAngle < 0 OrElse horizontalViewAngle >= PI Then Throw New ArgumentException("Horizontal view angle must be in [0,pi).")

        _ObserverEvent = observerEvent
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

    Private ReadOnly _ObserverEvent As SpaceTimeEvent
    Public ReadOnly Property ObserverEvent As SpaceTimeEvent
        Get
            Return _ObserverEvent
        End Get
    End Property

    Private ReadOnly _LookAt As Vector3D
    Public ReadOnly Property LookAt As Vector3D
        Get
            Return _LookAt
        End Get
    End Property

    Private ReadOnly _NormalizedLookDirection As Vector3D
    Public ReadOnly Property NormalizedLookDirection As Vector3D
        Get
            Return _NormalizedLookDirection
        End Get
    End Property

    Private ReadOnly _NormalizedUpDirection As Vector3D
    Public ReadOnly Property NormalizedUpDirection As Vector3D
        Get
            Return _NormalizedUpDirection
        End Get
    End Property

    Private ReadOnly _ViewPlaneDistanceVector As Vector3D
    Private ReadOnly _ViewPlaneToCameraDistance As Double
    Private ReadOnly _NormalizedRightVectorInViewPlane As Vector3D
    Private ReadOnly _NormalizedUpVectorInViewPlane As Vector3D

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="viewPlaneLocation">The view plane is visible if viewPlaneLocation.X and viewPlaneLocation.Y are in [-1; 1].</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SightRay(viewPlaneLocation As Vector2D) As SightRay
        Dim sightVectorInViewPlane = _NormalizedRightVectorInViewPlane * viewPlaneLocation.X + _NormalizedUpVectorInViewPlane * viewPlaneLocation.Y
        Return New SightRay(ObserverEvent, Direction:=_ViewPlaneDistanceVector + sightVectorInViewPlane)
    End Function
End Class
