Public Class SingleMaterialSurface(Of TMaterial)
    Implements ISurface(Of TMaterial)

    Public Property Surface As ISurface
    Public Property Material As TMaterial

    Public Sub New(surface As ISurface, material As TMaterial)
        Me.Surface = surface
        Me.Material = material
    End Sub

    Public Function Intersections(ray As Ray) As IEnumerable(Of SurfacePoint) Implements ISurface.Intersections
        Return Me.Surface.Intersections(ray)
    End Function

    Public Function MaterialIntersections(ray As Ray) As IEnumerable(Of SurfacePoint(Of TMaterial)) Implements ISurface(Of TMaterial).MaterialIntersections
        Return From intersection In Me.Intersections(ray)
               Where intersection IsNot Nothing
               Select Me.MaterialSurfacePointFromSurfacePoint(surfacePoint:=intersection)
    End Function

    Public Function FirstIntersection(ray As Math.Ray) As Math.SurfacePoint Implements Math.ISurface.FirstIntersection
        Return Me.Surface.FirstIntersection(ray)
    End Function

    Public Function FirstMaterialIntersection(ray As Math.Ray) As SurfacePoint(Of TMaterial) Implements ISurface(Of TMaterial).FirstMaterialIntersection
        Dim intersection = Me.FirstIntersection(ray)
        If intersection Is Nothing Then Return Nothing
        Return Me.MaterialSurfacePointFromSurfacePoint(surfacePoint:=intersection)
    End Function

    Private Function MaterialSurfacePointFromSurfacePoint(surfacePoint As SurfacePoint) As SurfacePoint(Of TMaterial)
        Return New SurfacePoint(Of TMaterial)(surfacePoint:=surfacePoint, Material:=Me.Material)
    End Function
End Class
