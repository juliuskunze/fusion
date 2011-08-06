''' <summary>
''' Implemented by surfaces, where all intersections are known for each ray. 
''' </summary>
''' <remarks></remarks>
Public Interface ISurface

    Function Intersections(ray As Ray) As IEnumerable(Of SurfacePoint)

    Function FirstIntersection(ray As Ray) As SurfacePoint

End Interface
