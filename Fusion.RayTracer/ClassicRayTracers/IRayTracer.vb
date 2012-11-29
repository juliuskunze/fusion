Public Interface IRayTracer(Of TLight As {ILight(Of TLight), New})
    Function GetLight(sightRay As SightRay) As TLight
End Interface
