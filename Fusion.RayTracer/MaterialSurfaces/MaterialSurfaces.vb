Public Class Surfaces(Of TMaterial)
    Implements ISurface(Of TMaterial), IEnumerable(Of ISurface(Of TMaterial))
    
    Private ReadOnly _Surfaces As IEnumerable(Of ISurface(Of TMaterial))

    Public Sub New(surfaces As IEnumerable(Of ISurface(Of TMaterial)))
        _Surfaces = surfaces
    End Sub


    Public Function Intersections(ray As Math.Ray) As System.Collections.Generic.IEnumerable(Of Math.SurfacePoint) Implements Math.ISurface.Intersections
        Return Me.MaterialIntersections(ray)
    End Function

    Public Function MaterialIntersections(ray As Math.Ray) As System.Collections.Generic.IEnumerable(Of SurfacePoint(Of TMaterial)) Implements ISurface(Of TMaterial).MaterialIntersections
        Return Me.SelectMany(Function(surface) surface.MaterialIntersections(ray)).
               OrderBy(Function(intersection) (intersection.Location - ray.Origin).LengthSquared)
    End Function

    Public Function FirstIntersection(ray As Math.Ray) As Math.SurfacePoint Implements Math.ISurface.FirstIntersection
        Return Me.Select(Function(surface) surface.FirstIntersection(ray)).
               Where(Function(intersection) intersection IsNot Nothing).
               MinItem(Function(intersection) (intersection.Location - ray.Origin).LengthSquared)
    End Function

    Public Function FirstMaterialIntersection(ray As Math.Ray) As SurfacePoint(Of TMaterial) Implements ISurface(Of TMaterial).FirstMaterialIntersection
        Return Me.Select(Function(surface) surface.FirstMaterialIntersection(ray)).
               Where(Function(intersection) intersection IsNot Nothing).
               MinItem(Function(intersection) (intersection.Location - ray.Origin).LengthSquared)
    End Function

    Public Function GetEnumerator() As System.Collections.Generic.IEnumerator(Of ISurface(Of TMaterial)) Implements System.Collections.Generic.IEnumerable(Of ISurface(Of TMaterial)).GetEnumerator
        Return _Surfaces.GetEnumerator
    End Function

    Public Function GetEnumeratorObj() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Throw New NotImplementedException
    End Function
End Class
