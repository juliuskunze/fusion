Public Class MaterialSurface(Of TLight)
    Implements ISurface(Of TLight)

    Private ReadOnly _Surface As ISurface
    Private ReadOnly _MaterialFunction As Func(Of SpaceTimeEvent, SurfacePoint, Material2D(Of TLight))

    Public Sub New(surface As ISurface,
                   materialFunction As Func(Of SpaceTimeEvent, SurfacePoint, Material2D(Of TLight)))
        _Surface = surface
        _MaterialFunction = materialFunction
    End Sub

    Public Sub New(surface As ISurface,
               materialFunction As Func(Of SpaceTimeEvent, Material2D(Of TLight)))
        _Surface = surface
        _MaterialFunction = Function(spaceTimeEvent, surfacePoint) materialFunction(spaceTimeEvent)
    End Sub

    Public Sub New(surface As ISurface,
                   material As Material2D(Of TLight))
        Me.New(surface, Function(spaceTimeEvent) material)
    End Sub

    Public Function Intersections(ray As Ray) As IEnumerable(Of SurfacePoint) Implements ISurface.Intersections
        Return _Surface.Intersections(ray)
    End Function

    Public Function MaterialIntersections(sightRay As SightRay) As IEnumerable(Of SurfacePoint(Of TLight)) Implements ISurface(Of TLight).MaterialIntersections
        Return _Surface.MaterialIntersections(sightRay, _MaterialFunction)
    End Function

    Public Function FirstIntersection(ray As Ray) As SurfacePoint Implements ISurface.FirstIntersection
        Return _Surface.FirstIntersection(ray)
    End Function

    Public Function FirstMaterialIntersection(sightRay As SightRay) As SurfacePoint(Of TLight) Implements ISurface(Of TLight).FirstMaterialIntersection
        Return _Surface.FirstMaterialIntersection(sightRay, _MaterialFunction)
    End Function
End Class