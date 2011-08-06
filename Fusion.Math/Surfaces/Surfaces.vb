Public Class Surfaces
    Inherits List(Of ISurface)
    Implements ISurface

    Public Function Intersections(ray As Ray) As IEnumerable(Of SurfacePoint) Implements ISurface.Intersections
        Return Me.SelectMany(Function(surface) surface.Intersections(ray)).
               OrderBy(Function(intersection) (intersection.Location - ray.Origin).LengthSquared)
    End Function

    Public Function FirstIntersection(ray As Ray) As SurfacePoint Implements ISurface.FirstIntersection
        Return Me.Select(Function(surface) surface.FirstIntersection(ray)).
               Where(Function(intersection) intersection IsNot Nothing).
               MinItem(Function(intersection) (intersection.Location - ray.Origin).LengthSquared)
    End Function

End Class