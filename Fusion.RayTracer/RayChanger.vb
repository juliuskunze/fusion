Public Class RayChanger(Of TLight)
    Private ReadOnly _SourceRay As SightRay
    Private ReadOnly _Intersection As SurfacePoint(Of Material2D(Of TLight))

    Public Sub New(sourceRay As SightRay, intersection As SurfacePoint(Of Material2D(Of TLight)))
        _SourceRay = sourceRay
        _Intersection = intersection
    End Sub

    Public Function ReflectedRay() As SightRay
        Dim normalizedNormal = _Intersection.NormalizedNormal
        Return RayWithNewDirectionAndSafetyDistance(newDirection:=_SourceRay.NormalizedDirection - 2 * _SourceRay.NormalizedDirection.OrthogonalProjectionOn(normalizedNormal))
    End Function

    Public Function RefractedRay() As SightRay
        Dim normalizedNormal = _Intersection.NormalizedNormal
        Dim refractionIndexQuotient = _Intersection.Material.RefractionIndexQuotient

        Dim startSinusVector = _SourceRay.NormalizedDirection - _SourceRay.NormalizedDirection.OrthogonalProjectionOn(normalizedNormal)
        Dim startSinus = startSinusVector.Length
        Dim finalSinus = startSinus * refractionIndexQuotient
        Dim finalCosinus = Sqrt(1 - finalSinus ^ 2)
        Dim finalDirection = finalCosinus * -normalizedNormal + finalSinus * startSinusVector.Normalized

        Return RayWithNewDirectionAndSafetyDistance(newDirection:=finalDirection)
    End Function

    Public Function PassedRay() As SightRay
        Return RayWithNewDirectionAndSafetyDistance(newDirection:=_SourceRay.NormalizedDirection)
    End Function

    Private Shared ReadOnly _Random As New Random
    Public Function ScatteredRay() As SightRay
        Dim scatteredRayDirection = NormalizedRandomDirection()
        If scatteredRayDirection * _Intersection.NormalizedNormal < 0 Then
            scatteredRayDirection *= -1
        End If

        Return RayWithNewDirectionAndSafetyDistance(newDirection:=scatteredRayDirection)
    End Function

    Private Function RayWithNewDirectionAndSafetyDistance(newDirection As Vector3D) As SightRay
        Return WithSafetyDistance(New SightRay(originEvent:=_Intersection.SpaceTimeEvent, direction:=newDirection))
    End Function

    Private Shared Function NormalizedRandomDirection() As Vector3D
        ' The z coordinate of random sphere surface points is uniform distributed in [-1; 1].
        Dim z = _Random.NextDouble * 2 - 1
        Dim phi = _Random.NextDouble * 2 * PI
        Dim rho = Sqrt(1 - z ^ 2)
        Dim y = rho * Sin(phi)
        Dim x = rho * Cos(phi)

        Return New Vector3D(z, y, x)
    End Function

    ''' <summary>
    ''' Adds a safety distance vector to the ray start location to avoid a double intersection with the same surface.
    ''' </summary>
    Private Shared Function WithSafetyDistance(sightRay As SightRay) As SightRay
        Return New SightRay(New Ray(origin:=sightRay.OriginLocation + sightRay.NormalizedDirection * SaftyDistance,
                                    direction:=sightRay.NormalizedDirection),
                            originTime:=sightRay.OriginTime)
    End Function

    Public Const SaftyDistance = 0.0000000001
End Class
