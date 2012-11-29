Public Class MaterialSurface(Of TMaterial)
    Implements ISurface(Of TMaterial)

    Private ReadOnly _Surface As ISurface
    Private ReadOnly _MaterialFunction As Func(Of SpaceTimeEvent, SurfacePoint, TMaterial)

    Public Sub New(surface As ISurface,
                   materialFunction As Func(Of SpaceTimeEvent, SurfacePoint, TMaterial))
        _Surface = surface
        _MaterialFunction = materialFunction
    End Sub

    Public Sub New(surface As ISurface,
               materialFunction As Func(Of SpaceTimeEvent, TMaterial))
        _Surface = surface
        _MaterialFunction = Function(spaceTimeEvent, surfacePoint) materialFunction(spaceTimeEvent)
    End Sub

    Public Sub New(surface As ISurface,
                   material As TMaterial)
        Me.New(surface, Function(spaceTimeEvent) material)
    End Sub

    Public Function Intersections(ray As Ray) As IEnumerable(Of SurfacePoint) Implements ISurface.Intersections
        Return _Surface.Intersections(ray)
    End Function

    Public Function MaterialIntersections(sightRay As SightRay) As IEnumerable(Of SurfacePoint(Of TMaterial)) Implements ISurface(Of TMaterial).MaterialIntersections
        Return _Surface.MaterialIntersections(sightRay, _MaterialFunction)
    End Function
    
    Public Function FirstIntersection(ray As Ray) As SurfacePoint Implements ISurface.FirstIntersection
        Return _Surface.FirstIntersection(ray)
    End Function

    Public Function FirstMaterialIntersection(sightRay As SightRay) As SurfacePoint(Of TMaterial) Implements ISurface(Of TMaterial).FirstMaterialIntersection
        Return _Surface.FirstMaterialIntersection(sightRay, _MaterialFunction)
    End Function
End Class