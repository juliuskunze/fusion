Public Interface ILightSpectrum(Of TLight As {ILight(Of TLight), New})
    Inherits ILight(Of TLight)

    Function GetIntensity(ByVal wavelength As Double) As Double

End Interface
