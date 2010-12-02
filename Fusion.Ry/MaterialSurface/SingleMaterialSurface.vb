Public Class SingleMaterialSurface(Of MaterialType)
    Implements IMaterialSurface(Of MaterialType)
    
    Public Property Surface As ISurface
    Public Property Material As MaterialType

    Public Sub New(ByVal surface As ISurface, ByVal material As MaterialType)
        Me.Surface = surface
        Me.Material = material
    End Sub

    Public Function Intersections(ByVal ray As Ray) As IEnumerable(Of SurfacePoint) Implements ISurface.Intersections
        Return Me.Surface.Intersections(ray)
    End Function

    Public Function MaterialIntersections(ByVal ray As Ray) As IEnumerable(Of MaterialSurfacePoint(Of MaterialType)) Implements IMaterialSurface(Of MaterialType).MaterialIntersections
        Return From intersection In Me.Intersections(ray)
               Where intersection IsNot Nothing
               Select Me.MaterialSurfacePointFromSurfacePoint(surfacePoint:=intersection)
    End Function

    Public Function FirstIntersection(ByVal ray As Math.Ray) As Math.SurfacePoint Implements Math.ISurface.FirstIntersection
        Return Me.Surface.FirstIntersection(ray)
    End Function

    Public Function FirstMaterialIntersection(ByVal ray As Math.Ray) As MaterialSurfacePoint(Of MaterialType) Implements IMaterialSurface(Of MaterialType).FirstMaterialIntersection
        Dim intersection = Me.FirstIntersection(ray)
        If intersection Is Nothing Then Return Nothing
        Return Me.MaterialSurfacePointFromSurfacePoint(surfacePoint:=intersection)
    End Function

    Private Function MaterialSurfacePointFromSurfacePoint(ByVal surfacePoint As SurfacePoint) As MaterialSurfacePoint(Of MaterialType)
        Return New MaterialSurfacePoint(Of MaterialType)(surfacePoint:=surfacePoint, Material:=Me.Material)
    End Function
End Class
