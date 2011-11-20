Public Class MaterialSurface(Of TMaterial)
    Implements ISurface(Of TMaterial)

    Private ReadOnly _Surface As ISurface
    Private ReadOnly _MaterialFunction As Func(Of SurfacePoint, TMaterial)

    Public Sub New(surface As ISurface,
                   materialFunction As Func(Of SurfacePoint, TMaterial))
        _Surface = surface
        _MaterialFunction = materialFunction
    End Sub

    Public Sub New(surface As ISurface,
                   materialFunction As Func(Of Vector3D, TMaterial))
        _Surface = surface
        _MaterialFunction = Function(surfacePoint) materialFunction(surfacePoint.Location)
    End Sub

    Public ReadOnly Property Surface As ISurface
        Get
            Return _Surface
        End Get
    End Property

    Public Function Intersections(ray As Math.Ray) As IEnumerable(Of SurfacePoint) Implements ISurface.Intersections
        Return Me.Surface.Intersections(ray)
    End Function

    Public Function MaterialIntersections(ray As Math.Ray) As IEnumerable(Of SurfacePoint(Of TMaterial)) Implements ISurface(Of TMaterial).MaterialIntersections
        Return From surfacePoint In Me.Intersections(ray)
               Select MaterialSurfacePointFromSurfacePoint(surfacePoint:=surfacePoint)
    End Function

    Private Function MaterialSurfacePointFromSurfacePoint(surfacePoint As SurfacePoint) As SurfacePoint(Of TMaterial)
        Return New SurfacePoint(Of TMaterial)(surfacePoint:=surfacePoint, Material:=_MaterialFunction(surfacePoint))
    End Function

    Public Function FirstIntersection(ray As Math.Ray) As Math.SurfacePoint Implements ISurface.FirstIntersection
        Return Me.Surface.FirstIntersection(ray)
    End Function

    Public Function FirstMaterialIntersection(ray As Math.Ray) As SurfacePoint(Of TMaterial) Implements ISurface(Of TMaterial).FirstMaterialIntersection
        Dim intersection = Me.FirstIntersection(ray)

        If intersection Is Nothing Then Return Nothing

        Return MaterialSurfacePointFromSurfacePoint(intersection)
    End Function

End Class
