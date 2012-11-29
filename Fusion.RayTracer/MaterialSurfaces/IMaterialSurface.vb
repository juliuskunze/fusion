Public Interface ISurface(Of TMaterial)
    Inherits ISurface

    Function MaterialIntersections(sightRay As SightRay) As IEnumerable(Of SurfacePoint(Of TMaterial))
    Function FirstMaterialIntersection(sightRay As SightRay) As SurfacePoint(Of TMaterial)
End Interface