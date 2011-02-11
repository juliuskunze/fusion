Public Class MaterialSurfaces(Of MaterialType)
    Inherits List(Of IMaterialSurface(Of MaterialType))
    Implements IMaterialSurface(Of MaterialType)

    Public Function Intersections(ByVal ray As Math.Ray) As System.Collections.Generic.IEnumerable(Of Math.SurfacePoint) Implements Math.ISurface.Intersections
        Return Me.MaterialIntersections(ray)
    End Function

    Public Function MaterialIntersections(ByVal ray As Math.Ray) As System.Collections.Generic.IEnumerable(Of MaterialSurfacePoint(Of MaterialType)) Implements IMaterialSurface(Of MaterialType).MaterialIntersections
        Return Me.SelectMany(Function(surface) surface.MaterialIntersections(ray)).
               OrderBy(Function(intersection) (intersection.Location - ray.Origin).LengthSquared)
    End Function

    Public Function FirstIntersection(ByVal ray As Math.Ray) As Math.SurfacePoint Implements Math.ISurface.FirstIntersection
        Return Me.Select(Function(surface) surface.FirstIntersection(ray)).
               Where(Function(intersection) intersection IsNot Nothing).
               MinItem(Function(intersection) (intersection.Location - ray.Origin).LengthSquared)
    End Function

    Public Function FirstMaterialIntersection(ByVal ray As Math.Ray) As MaterialSurfacePoint(Of MaterialType) Implements IMaterialSurface(Of MaterialType).FirstMaterialIntersection
        Return Me.Select(Function(surface) surface.FirstMaterialIntersection(ray)).
               Where(Function(intersection) intersection IsNot Nothing).
               MinItem(Function(intersection) (intersection.Location - ray.Origin).LengthSquared)
    End Function

End Class
