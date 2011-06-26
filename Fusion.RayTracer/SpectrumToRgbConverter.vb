Imports System.IO

Public Class SpectrumToRgbConverter

    Public Const LowerWavelengthBound = 380 * 10 ^ -9
    Public Const UpperWavelengthBound = 710 * 10 ^ -9

    Private _WavelengthStep As Double
    Private ReadOnly _TestStepCount As Integer

    Private _ColorArray As RgbLight()

    Public Sub New(ByVal testStepCount As Integer)
        _TestStepCount = testStepCount
        Me.ReadWavelengthRgbDictionary()
    End Sub

    Private Sub ReadWavelengthRgbDictionary()
        Dim filename = Directory.GetCurrentDirectory & "\Data\2000pixel spectrum sRGB (380nm to 710nm).bmp"
        Dim image = Drawing.Bitmap.FromFile(filename:=filename)
        Dim bitmap = CType(image, Bitmap)

        ReDim _ColorArray(bitmap.Width - 1)
        _WavelengthStep = (UpperWavelengthBound - LowerWavelengthBound) / (_ColorArray.Count - 1)

        For index = 0 To bitmap.Width - 1
            _ColorArray(index) = New RgbLight(bitmap.GetPixel(index, 0))
        Next

        Dim white = Me.GetIntegral(New FunctionLightSpectrum(IntensityFunction:=Function(wavelength) 1), testStepCount:=_ColorArray.Count)

        For index = 0 To _ColorArray.Count - 1
            _ColorArray(index) = New RgbLight(red:=_ColorArray(index).Red / white.Red,
                                              blue:=_ColorArray(index).Blue / white.Blue,
                                              green:=_ColorArray(index).Green / white.Green)
        Next
    End Sub

    Public Function GetIntensityPerWavelength(ByVal wavelength As Double) As RgbLight
        If wavelength < LowerWavelengthBound OrElse wavelength > UpperWavelengthBound Then Return RgbLight.Black

        Dim index = CInt((wavelength - LowerWavelengthBound) / _WavelengthStep)

        Return _ColorArray(index)
    End Function

    Public Function Convert(ByVal spectrum As ILightSpectrum) As RgbLight
        Return Me.GetIntegral(spectrum, testStepCount:=_TestStepCount)
    End Function

    Private Function GetIntegral(ByVal spectrum As ILightSpectrum, ByVal testStepCount As Integer) As RgbLight
        Dim rgbLight = New RgbLight

        Dim interval = (UpperWavelengthBound - LowerWavelengthBound) / (testStepCount - 1)

        For index = 0 To testStepCount - 1
            Dim wavelength = LowerWavelengthBound + index * interval
            Dim intensity = spectrum.GetIntensityPerWavelength(wavelength)
            rgbLight += Me.GetIntensityPerWavelength(wavelength:=wavelength) * intensity
        Next

        Return rgbLight / testStepCount
    End Function

End Class