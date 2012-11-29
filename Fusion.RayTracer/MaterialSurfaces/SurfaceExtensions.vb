
Imports System.Runtime.CompilerServices

Public Module SurfaceExtensions
    <Extension>
    Public Function MaterialIntersections(Of TMaterial)(surface As ISurface, sightRay As SightRay, materialFunction As Func(Of SpaceTimeEvent, SurfacePoint, TMaterial)) As IEnumerable(Of SurfacePoint(Of TMaterial))
        Return From intersection In surface.Intersections(sightRay.Ray)
               Select GetMaterialSurfacePoint(sightRay, intersection, materialFunction)
    End Function

    <Extension>
    Public Function FirstMaterialIntersection(Of TMaterial)(surface As ISurface, sightRay As SightRay, materialFunction As Func(Of SpaceTimeEvent, SurfacePoint, TMaterial)) As SurfacePoint(Of TMaterial)
        Dim intersection = surface.FirstIntersection(sightRay.Ray)

        If intersection Is Nothing Then Return Nothing

        Return GetMaterialSurfacePoint(sightRay, intersection, materialFunction)
    End Function

    Private Function GetMaterialSurfacePoint(Of TMaterial)(sightRay As SightRay, intersection As SurfacePoint, materialFunction As Func(Of SpaceTimeEvent, SurfacePoint, TMaterial)) As SurfacePoint(Of TMaterial)
        Dim intersectionTime = sightRay.GetTime((intersection.Location - sightRay.OriginLocation).Length)

        Return New SurfacePoint(Of TMaterial)(surfacePoint:=intersection, Material:=materialFunction(New SpaceTimeEvent(intersection.Location, intersectionTime), intersection), time:=intersectionTime)
    End Function
End Module