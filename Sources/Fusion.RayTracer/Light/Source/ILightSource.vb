Public Interface ILightSource(Of TLight)
    Function GetLight(surfacePoint As SurfacePoint(Of TLight)) As TLight
End Interface
