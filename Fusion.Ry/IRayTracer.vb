Public Interface IRayTracer(Of TLight As {ILight(Of TLight), New})

    Function GetColor(ByVal startRay As Ray) As TLight

End Interface
