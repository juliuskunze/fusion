Public Interface ISurface(Of TMaterial)
    Inherits ISurface

    Function MaterialIntersections(ray As Ray) As IEnumerable(Of SurfacePoint(Of TMaterial))

    Function FirstMaterialIntersection(ray As Ray) As SurfacePoint(Of TMaterial)

End Interface