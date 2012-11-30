Public Interface ISurface(Of TLight)
    Inherits ISurface

    Function MaterialIntersections(sightRay As SightRay) As IEnumerable(Of SurfacePoint(Of TLight))
    Function FirstMaterialIntersection(sightRay As SightRay) As SurfacePoint(Of TLight)
End Interface