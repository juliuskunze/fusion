Public Interface IMaterialSurface(Of MaterialType)
    Inherits ISurface

    Function MaterialIntersections(ByVal ray As Ray) As IEnumerable(Of MaterialSurfacePoint(Of MaterialType))

    Function FirstMaterialIntersection(ByVal ray As Ray) As MaterialSurfacePoint(Of MaterialType)

End Interface