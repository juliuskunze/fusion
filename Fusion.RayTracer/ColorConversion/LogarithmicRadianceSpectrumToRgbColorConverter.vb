Public Class LogarithmicRadianceSpectrumToRgbColorConverter
    Implements ILightToRgbColorConverter(Of RadianceSpectrum)

    Private ReadOnly _LinearConverter As LinearRadianceSpectrumToRgbColorConverter

    Public Sub New(linearConverter As LinearRadianceSpectrumToRgbColorConverter)
        _LinearConverter = linearConverter
    End Sub

    Public Function Convert(light As RadianceSpectrum) As System.Drawing.Color Implements ILightToRgbColorConverter(Of RadianceSpectrum).Convert
        Dim rgbLight = _LinearConverter.ConvertToRgbLight(light:=light)
    End Function

End Class