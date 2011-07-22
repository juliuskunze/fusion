Public Interface IRayTracer(Of TLight As {ILight(Of TLight), New})

    Function GetLight(ByVal viewRay As Ray) As TLight

End Interface
