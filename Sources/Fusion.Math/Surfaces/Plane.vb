Public Class Plane
    Implements ISurfacedPointSet3D

    Public Sub New(location As Vector3D, normal As Vector3D)
        _Location = location
        _NormalizedNormal = normal.Normalized
    End Sub

    Public Sub New(point1 As Vector3D, point2 As Vector3D, point3 As Vector3D)
        Me.New(Location:=point1, Normal:=(point2 - point1).CrossProduct(point3 - point1))
    End Sub

    Private ReadOnly _NormalizedNormal As Vector3D
    Public ReadOnly Property NormalizedNormal As Vector3D
        Get
            Return _NormalizedNormal
        End Get
    End Property

    Private ReadOnly _Location As Vector3D
    Public ReadOnly Property Location As Vector3D
        Get
            Return _Location
        End Get
    End Property

    Public Function FirstIntersection(ray As Ray) As SurfacePoint Implements ISurface.FirstIntersection
        Dim relativeRayOrigin = ray.Origin - Location

        Dim signedRelativeRayOriginDistance = relativeRayOrigin * _NormalizedNormal
        Dim rayOriginCorrect = (signedRelativeRayOriginDistance > 0)
        If Not rayOriginCorrect Then Return Nothing

        Dim signedRayDirectionDistance = ray.NormalizedDirection * _NormalizedNormal
        Dim rayDirectionCorrect = (signedRayDirectionDistance < 0)
        If Not rayDirectionCorrect Then Return Nothing

        'follows from "pointOnRay * normal = 0":
        Dim rayOriginToIntersectionDistance = -signedRelativeRayOriginDistance / signedRayDirectionDistance
        Dim intersectionLocation = ray.Origin + ray.NormalizedDirection * rayOriginToIntersectionDistance

        Return New SurfacePoint(Location:=intersectionLocation, Normal:=_NormalizedNormal)
    End Function

    Public Function CoveredHalfSpaceContains(point As Vector3D) As Boolean Implements IPointSet3D.Contains
        Return NormalizedNormal * (point - Location) < 0
    End Function

    Public Function Intersections(ray As Ray) As IEnumerable(Of SurfacePoint) Implements ISurface.Intersections
        Dim intersection = FirstIntersection(ray)

        If intersection Is Nothing Then Return Enumerable.Empty(Of SurfacePoint)()
        Return {intersection}
    End Function
End Class