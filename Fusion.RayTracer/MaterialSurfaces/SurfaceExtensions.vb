
Imports System.Runtime.CompilerServices

Public Module SurfaceExtensions
    <Extension>
    Public Function MaterialIntersections(Of TLight)(surface As ISurface, sightRay As SightRay, materialFunction As Func(Of SpaceTimeEvent, SurfacePoint, Material2D(Of TLight))) As IEnumerable(Of SurfacePoint(Of TLight))
        Return From intersection In surface.Intersections(sightRay.Ray)
               Select GetMaterialSurfacePoint(sightRay, intersection, materialFunction)
    End Function

    <Extension>
    Public Function FirstMaterialIntersection(Of TLight)(surface As ISurface, sightRay As SightRay, materialFunction As Func(Of SpaceTimeEvent, SurfacePoint, Material2D(Of TLight))) As SurfacePoint(Of TLight)
        Dim intersection = surface.FirstIntersection(sightRay.Ray)

        If intersection Is Nothing Then Return Nothing

        Return GetMaterialSurfacePoint(sightRay, intersection, materialFunction)
    End Function

    Private Function GetMaterialSurfacePoint(Of TLight)(sightRay As SightRay, intersection As SurfacePoint, materialFunction As Func(Of SpaceTimeEvent, SurfacePoint, Material2D(Of TLight))) As SurfacePoint(Of TLight)
        Dim intersectionTime = sightRay.GetTime((intersection.Location - sightRay.OriginLocation).Length)

        Return New SurfacePoint(Of TLight)(surfacePoint:=intersection, Material:=materialFunction(New SpaceTimeEvent(intersection.Location, intersectionTime), intersection), time:=intersectionTime)
    End Function
End Module