Public Interface ISurface(Of TMaterial)
    Inherits ISurface

    Function MaterialIntersections(ByVal ray As Ray) As IEnumerable(Of SurfacePoint(Of TMaterial))

    Function FirstMaterialIntersection(ByVal ray As Ray) As SurfacePoint(Of TMaterial)

End Interface