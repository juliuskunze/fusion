Public Class SingleMaterialSurface(Of TMaterial)
    Implements ISurface(Of TMaterial)

    Public Property Surface As ISurface
    Public Property Material As TMaterial

    Public Sub New(ByVal surface As ISurface, ByVal material As TMaterial)
        Me.Surface = surface
        Me.Material = material
    End Sub

    Public Function Intersections(ByVal ray As Ray) As IEnumerable(Of SurfacePoint) Implements ISurface.Intersections
        Return Me.Surface.Intersections(ray)
    End Function

    Public Function MaterialIntersections(ByVal ray As Ray) As IEnumerable(Of SurfacePoint(Of TMaterial)) Implements ISurface(Of TMaterial).MaterialIntersections
        Return From intersection In Me.Intersections(ray)
               Where intersection IsNot Nothing
               Select Me.MaterialSurfacePointFromSurfacePoint(surfacePoint:=intersection)
    End Function

    Public Function FirstIntersection(ByVal ray As Math.Ray) As Math.SurfacePoint Implements Math.ISurface.FirstIntersection
        Return Me.Surface.FirstIntersection(ray)
    End Function

    Public Function FirstMaterialIntersection(ByVal ray As Math.Ray) As SurfacePoint(Of TMaterial) Implements ISurface(Of TMaterial).FirstMaterialIntersection
        Dim intersection = Me.FirstIntersection(ray)
        If intersection Is Nothing Then Return Nothing
        Return Me.MaterialSurfacePointFromSurfacePoint(surfacePoint:=intersection)
    End Function

    Private Function MaterialSurfacePointFromSurfacePoint(ByVal surfacePoint As SurfacePoint) As SurfacePoint(Of TMaterial)
        Return New SurfacePoint(Of TMaterial)(surfacePoint:=surfacePoint, Material:=Me.Material)
    End Function
End Class
