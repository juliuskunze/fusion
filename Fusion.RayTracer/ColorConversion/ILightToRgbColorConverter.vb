Public Interface ILightToRgbColorConverter(Of TLight)
    Function Run(light As TLight) As Color
End Interface
