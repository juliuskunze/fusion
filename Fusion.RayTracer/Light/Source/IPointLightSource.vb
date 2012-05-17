Public Interface IPointLightSource(Of TLight)
    Inherits ILightSource(Of TLight)

    ReadOnly Property Location As Vector3D

    Overloads Function GetLight(point As Vector3D) As TLight

End Interface
