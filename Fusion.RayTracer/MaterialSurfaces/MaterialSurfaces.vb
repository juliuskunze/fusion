Public Class Surfaces(Of TMaterial)
    Implements ISurface(Of TMaterial), IEnumerable(Of ISurface(Of TMaterial))
    
    Private ReadOnly _Surfaces As IEnumerable(Of ISurface(Of TMaterial))

    Public Sub New(surfaces As IEnumerable(Of ISurface(Of TMaterial)))
        _Surfaces = surfaces
    End Sub

    Public Function Intersections(ray As Ray) As IEnumerable(Of SurfacePoint) Implements ISurface.Intersections
        Return MaterialIntersections(New SightRay(ray))
    End Function

    Public Function MaterialIntersections(sightRay As SightRay) As IEnumerable(Of SurfacePoint(Of TMaterial)) Implements ISurface(Of TMaterial).MaterialIntersections
        Return SelectMany(Function(surface) surface.MaterialIntersections(sightRay)).
               OrderBy(Function(intersection) (intersection.Location - sightRay.OriginLocation).LengthSquared)
    End Function

    Public Function FirstIntersection(ray As Math.Ray) As Math.SurfacePoint Implements Math.ISurface.FirstIntersection
        Return Me.Select(Function(surface) surface.FirstIntersection(ray)).
               Where(Function(intersection) intersection IsNot Nothing).
               MinItem(Function(intersection) (intersection.Location - ray.Origin).LengthSquared)
    End Function

    Public Function FirstMaterialIntersection(sightRay As SightRay) As SurfacePoint(Of TMaterial) Implements ISurface(Of TMaterial).FirstMaterialIntersection
        Return Me.Select(Function(surface) surface.FirstMaterialIntersection(sightRay)).
               Where(Function(intersection) intersection IsNot Nothing).
               MinItem(Function(intersection) (intersection.Location - sightRay.OriginLocation).LengthSquared)
    End Function

    Public Function GetEnumerator() As IEnumerator(Of ISurface(Of TMaterial)) Implements IEnumerable(Of ISurface(Of TMaterial)).GetEnumerator
        Return _Surfaces.GetEnumerator
    End Function

    Public Function GetEnumeratorObj() As Collections.IEnumerator Implements Collections.IEnumerable.GetEnumerator
        Throw New NotImplementedException
    End Function
End Class
