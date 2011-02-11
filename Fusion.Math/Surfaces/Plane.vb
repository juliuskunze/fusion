Public Class Plane
    Implements ISurfacedPointSet3D

    Public Sub New(ByVal location As Vector3D, ByVal normal As Vector3D)
        Me.Location = location
        Me.Normal = normal
    End Sub

    Public Sub New(ByVal point1 As Vector3D, ByVal point2 As Vector3D, ByVal point3 As Vector3D)
        Me.New(Location:=point1, Normal:=(point2 - point1).CrossProduct(point3 - point1))
    End Sub

    Public WriteOnly Property Normal As Vector3D
        Set(ByVal value As Vector3D)
            _normalizedNormal = value.Normalized
        End Set
    End Property

    Private _normalizedNormal As Vector3D
    Public ReadOnly Property NormalizedNormal As Vector3D
        Get
            Return _normalizedNormal
        End Get
    End Property

    Public Property Location As Vector3D

    Public Function Intersection(ByVal ray As Ray) As SurfacePoint Implements ISurfacedPointSet3D.FirstIntersection
        Dim relativeRayOrigin = ray.Origin - Me.Location

        Dim signedRelativeRayOriginDistance = relativeRayOrigin * Me.NormalizedNormal
        Dim rayOriginCorrect = (signedRelativeRayOriginDistance > 0)
        If Not rayOriginCorrect Then Return Nothing

        Dim signedRayDirectionDistance = ray.NormalizedDirection * Me.NormalizedNormal
        Dim rayDirectionCorrect = (signedRayDirectionDistance < 0)
        If Not rayDirectionCorrect Then Return Nothing

        'follows from "pointOnRay * normal = 0":
        Dim rayOriginToIntersectionDistance = -signedRelativeRayOriginDistance / signedRayDirectionDistance
        Dim intersectionLocation = ray.Origin + ray.NormalizedDirection * rayOriginToIntersectionDistance

        Return New SurfacePoint(Location:=intersectionLocation, Normal:=Me.NormalizedNormal)
    End Function

    Public Function CoveredHalfSpaceContains(ByVal point As Vector3D) As Boolean Implements IPointSet3D.Contains
        Return Me.NormalizedNormal * (point - Me.Location) < 0
    End Function

    Public Function Intersections(ByVal ray As Ray) As IEnumerable(Of SurfacePoint) Implements ISurface.Intersections
        Dim intersection = Me.Intersection(ray)

        If intersection Is Nothing Then Return Enumerable.Empty(Of SurfacePoint)()
        Return {intersection}
    End Function

End Class