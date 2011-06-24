Public Interface ILightSpectrum
    Inherits ILight(Of ILightSpectrum)

    Function GetIntensityPerWavelength(ByVal wavelength As Double) As Double

End Interface
