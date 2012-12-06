Public Interface IPointLightSource(Of TLight)
    Inherits ILightSource(Of TLight)

    ReadOnly Property Location As Vector3D

    ''' <summary>
    ''' The light in direction of the point light source at the specified event.
    ''' </summary>
    Function GetMaximumLight(spaceTimeEvent As SpaceTimeEvent) As TLight
End Interface
