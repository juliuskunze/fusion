''' <summary>
''' Implemented by surfaces, where all intersections are known for each ray. 
''' </summary>
''' <remarks></remarks>
Public Interface ISurface

    Function Intersections(ByVal ray As Ray) As IEnumerable(Of SurfacePoint)

    Function FirstIntersection(ByVal ray As Ray) As SurfacePoint

End Interface
