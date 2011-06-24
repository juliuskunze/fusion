Imports System.IO

Public Class SpectrumToRgbConverter

    Public Const RedWavelength = 700.0 * 10 ^ -9
    Public Const GreenWavelength = 546.1 * 10 ^ -9
    Public Const BlueWavelength = 435.8 * 10 ^ -9

    Public Const LowerWavelengthBound = 380 * 10 ^ -9
    Public Const UpperWavelengthBound = 765 * 10 ^ -9

    ' when integrate the constant intensity 1 multiplied with the resulting intensity dense over wavelength, 
    ' the result must be white --> this normalization factor.
    Private Const _NormalizeMatchFactor = 20.37

    Private Const _WavelengthStep = 5 * 10 ^ -9
    Private _WavelengthRgbDictionary As Dictionary(Of Double, RgbLight)
    Private ReadOnly _TestStepCount As Integer

    Public Sub New(ByVal testStepCount As Integer)
        _TestStepCount = testStepCount
        Me.ReadWavelengthRgbDictionary()
    End Sub

    Private Sub ReadWavelengthRgbDictionary()
        _WavelengthRgbDictionary = New Dictionary(Of Double, RgbLight)

        Dim lines = IO.File.ReadLines(IO.Directory.GetCurrentDirectory & "\Data\CIE 1931 RGB tristimulus values.txt")
        Dim numbersCollection = From line In lines Select line.Split(count:=4, separator:={ControlChars.Tab})
        For Each numbers In numbersCollection
            _WavelengthRgbDictionary.Add(key:=CDbl(numbers(0)) * 10 ^ -9, value:=New RgbLight(red:=CDbl(numbers(1)), green:=CDbl(numbers(2)), blue:=CDbl(numbers(3))))
        Next
    End Sub

    Public Function GetIntensityPerWavelength(ByVal wavelength As Double, ByVal intensity As Double) As RgbLight
        If wavelength < LowerWavelengthBound OrElse wavelength > UpperWavelengthBound Then Return New RgbLight

        '!!!If wavelength Mod _WavelengthStep < 1.0E-19 Then
        '    If wavelength = LowerWavelengthBound Then Return _WavelengthRgbDictionary.Values.First

        '    Dim rgbLight As RgbLight
        '    If Not _WavelengthRgbDictionary.TryGetValue(key:=wavelength, value:=rgbLight) Then Throw New InvalidOperationException
        '    Return intensity * rgbLight
        'End If

        Dim neighbors = From wavelengthRgbPair In _WavelengthRgbDictionary Where Abs(wavelength - wavelengthRgbPair.Key) < _WavelengthStep

        If neighbors.Count = 1 Then Return neighbors.Single.Value

        If neighbors.Count <> 2 Then Throw New InvalidOperationException
        Dim first = neighbors.First
        Dim second = neighbors.Last

        If first.Key < second.Key Then
            Return _NormalizeMatchFactor * intensity * InterpolateIntensityPerWavelength(wavelength:=wavelength, lower:=first, upper:=second)
        Else
            Return _NormalizeMatchFactor * intensity * InterpolateIntensityPerWavelength(wavelength:=wavelength, lower:=second, upper:=first)
        End If
    End Function

    Public Function ConvertSpectrum(ByVal wavelength As Double, ByVal intensity As Double) As RgbLight
        If wavelength < LowerWavelengthBound OrElse wavelength > UpperWavelengthBound Then Return New RgbLight

        If wavelength Mod _WavelengthStep < 1.0E-19 Then
            If wavelength = LowerWavelengthBound Then Return _WavelengthRgbDictionary.Values.First

            Dim rgbLight As RgbLight
            If Not _WavelengthRgbDictionary.TryGetValue(key:=wavelength, value:=rgbLight) Then Throw New InvalidOperationException
            Return intensity * rgbLight
        End If

        Dim neighbors = From wavelengthRgbPair In _WavelengthRgbDictionary Where Abs(wavelength - wavelengthRgbPair.Key) < _WavelengthStep

        If neighbors.Count <> 2 Then Throw New InvalidOperationException
        Dim first = neighbors.First
        Dim second = neighbors.Last

        If first.Key < second.Key Then
            Return intensity * InterpolateIntensityPerWavelength(wavelength:=wavelength, lower:=first, upper:=second)
        Else
            Return intensity * InterpolateIntensityPerWavelength(wavelength:=wavelength, lower:=second, upper:=first)
        End If
    End Function

    Public Function Convert(ByVal spectrum As ILightSpectrum) As RgbLight
        Return Me.GetIntegral(spectrum)
    End Function

    Private Function GetIntegral(ByVal spectrum As ILightSpectrum) As RgbLight
        Dim rgbLight = New RgbLight

        Dim interval = (UpperWavelengthBound - LowerWavelengthBound) / (_TestStepCount - 1)

        For index = 0 To _TestStepCount - 1
            Dim wavelength = LowerWavelengthBound + index * interval
            Dim intensity = spectrum.GetIntensityPerWavelength(wavelength)
            rgbLight += Me.GetIntensityPerWavelength(wavelength:=wavelength, intensity:=intensity)
        Next

        Return rgbLight / _TestStepCount
    End Function

    Private Shared Function InterpolateIntensityPerWavelength(ByVal wavelength As Double, ByVal lower As KeyValuePair(Of Double, RgbLight), ByVal upper As KeyValuePair(Of Double, RgbLight)) As RgbLight
        Return lower.Value + (upper.Value - lower.Value) * (wavelength - lower.Key) / _WavelengthStep
    End Function

End Class