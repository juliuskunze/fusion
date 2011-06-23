Public Class SpectrumToRgbConverter

    Public Const RedWavelength = 700.0 * 10 ^ -9
    Public Const GreenWavelength = 546.1 * 10 ^ -9
    Public Const BlueWavelength = 435.8 * 10 ^ -9

    Private Const _LowerWavelengthBound = 380 * 10 ^ -9
    Private Const _UpperWavelengthBound = 760 * 10 ^ -9

    Private ReadOnly _WavelengthStep As Double
    Private ReadOnly _WavelengthRgbDictionary As Dictionary(Of Double, RgbLight)

    Public Sub New(ByVal wavelengthRgbDictionary As Dictionary(Of Double, RgbLight), ByVal wavelengthStep As Double)
        _WavelengthRgbDictionary = wavelengthRgbDictionary
        _WavelengthStep = wavelengthStep
    End Sub

    Public Function Convert(ByVal wavelength As Double, ByVal intensity As Double) As RgbLight
        If wavelength < _LowerWavelengthBound OrElse wavelength > _UpperWavelengthBound Then Return New RgbLight

        If wavelength Mod 5 = 0 Then
            Dim rgbLight As RgbLight
            If Not _WavelengthRgbDictionary.TryGetValue(key:=wavelength, value:=rgbLight) Then Throw New InvalidOperationException
            Return rgbLight
        End If

        Dim neighbors = From wavelengthRgbPair In _WavelengthRgbDictionary Where Abs(wavelength - wavelengthRgbPair.Key) < _WavelengthStep

        If neighbors.Count <> 2 Then Throw New InvalidOperationException
        Dim first = neighbors.First
        Dim second = neighbors.Last

        If first.Key < second.Key Then
            Return intensity * InterpolateWavelength(wavelength:=wavelength, lower:=first, upper:=second)
        Else
            Return intensity * InterpolateWavelength(wavelength:=wavelength, lower:=first, upper:=second)
        End If
    End Function

    Private Function InterpolateWavelength(ByVal wavelength As Double, ByVal lower As KeyValuePair(Of Double, RgbLight), ByVal upper As KeyValuePair(Of Double, RgbLight)) As RgbLight
        Return lower.Value + upper.Value * (wavelength - lower.Key) / _WavelengthStep
    End Function

End Class
