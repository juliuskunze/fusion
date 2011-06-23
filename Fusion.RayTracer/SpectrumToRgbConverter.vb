Imports System.IO

Public Class SpectrumToRgbConverter

    Public Const RedWavelength = 700.0 * 10 ^ -9
    Public Const GreenWavelength = 546.1 * 10 ^ -9
    Public Const BlueWavelength = 435.8 * 10 ^ -9

    Public Const LowerWavelengthBound = 380 * 10 ^ -9
    Public Const UpperWavelengthBound = 760 * 10 ^ -9

    Private Const _WavelengthStep = 5 * 10 ^ -9
    Private _WavelengthRgbDictionary As Dictionary(Of Double, RgbLight)

    Public Sub New()
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

    Public Function Convert(ByVal wavelength As Double, ByVal intensity As Double) As RgbLight
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
            Return intensity * InterpolateWavelength(wavelength:=wavelength, lower:=first, upper:=second)
        Else
            Return intensity * InterpolateWavelength(wavelength:=wavelength, lower:=second, upper:=first)
        End If
    End Function

    Private Shared Function InterpolateWavelength(ByVal wavelength As Double, ByVal lower As KeyValuePair(Of Double, RgbLight), ByVal upper As KeyValuePair(Of Double, RgbLight)) As RgbLight
        Return lower.Value + (upper.Value - lower.Value) * (wavelength - lower.Key) / _WavelengthStep
    End Function

End Class