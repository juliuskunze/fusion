Public Class LogarithmicRadianceSpectrumToRgbColorConverter
    Implements ILightToRgbColorConverter(Of RadianceSpectrum)

    Private ReadOnly _Converter As RadianceSpectrumToRgbColorConverter

    Public Sub New(converter As RadianceSpectrumToRgbColorConverter)
        _Converter = converter
    End Sub

    Public Function Convert(light As RadianceSpectrum) As System.Drawing.Color Implements ILightToRgbColorConverter(Of RadianceSpectrum).Convert
        Dim rgbLight = _Converter.ConvertToRgbLight(light:=light)
    End Function

End Class