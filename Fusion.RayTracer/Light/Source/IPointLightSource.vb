Public Interface IPointLightSource(Of TLight)
    Inherits ILightSource(Of TLight)

    ReadOnly Property Location As Vector3D

    Overloads Function GetLight(point As Point) As TLight
End Interface
