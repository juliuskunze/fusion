Public Interface ILightSource(Of TLight)

    Function GetLight(ByVal surfacePoint As SurfacePoint) As TLight

End Interface
