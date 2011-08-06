Public Interface IRayTracer(Of TLight As {ILight(Of TLight), New})

    Function GetLight(viewRay As Ray) As TLight

End Interface
