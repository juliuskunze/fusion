Public Interface ILightSpectrum
    Inherits ILight(Of ILightSpectrum)

    Function GetIntensity(ByVal wavelength As Double) As Double

End Interface
