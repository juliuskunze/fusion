Public Interface IRayTracer(Of TLight As {ILight(Of TLight), New})
    Function GetLight(sightRay As Ray) As TLight
End Interface
