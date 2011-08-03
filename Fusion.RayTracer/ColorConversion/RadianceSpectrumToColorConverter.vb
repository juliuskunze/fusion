Imports System.IO

Public Class RadianceSpectrumToColorConverter
    Implements ILightToColorConverter(Of RadianceSpectrum)

    Private _RgbLightToColorConverter As New RgbLightToColorConverter

    Public Const LowerVisibleWavelengthBound = 380 * 10 ^ -9
    Public Const UpperVisibleWavelengthBound = 710 * 10 ^ -9

    Private _WavelengthStep As Double
    Private ReadOnly _TestStepCount As Integer

    Private _ColorArray As RgbLight()
    Private ReadOnly _SpectralRadiancePerWhite As Double

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="testStepCount"></param>
    ''' <param name="spectralRadiancePerWhite">Wenn das ganze Spektrum die übergebene spektrale Strahldichte besitzt, wird die RGB-Farbe Weiß (255, 255, 255) zurückgegeben.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal testStepCount As Integer, ByVal spectralRadiancePerWhite As Double)
        If spectralRadiancePerWhite <= 0 Then Throw New ArgumentOutOfRangeException("spectralRadiancePerWhite")

        _TestStepCount = testStepCount
        _SpectralRadiancePerWhite = spectralRadiancePerWhite
        Me.ReadWavelengthRgbDictionary()
    End Sub

    Private Sub ReadWavelengthRgbDictionary()
        Dim filename = Directory.GetCurrentDirectory & "\Data\2000pixel spectrum sRGB (380nm to 710nm).bmp"
        Dim image = Drawing.Bitmap.FromFile(filename:=filename)
        Dim bitmap = CType(image, Bitmap)

        ReDim _ColorArray(bitmap.Width - 1)
        _WavelengthStep = (UpperVisibleWavelengthBound - LowerVisibleWavelengthBound) / (_ColorArray.Count - 1)

        For index = 0 To bitmap.Width - 1
            _ColorArray(index) = New RgbLight(bitmap.GetPixel(index, 0))
        Next

        Me.NormalizeToWhite()
        Me.NormalizeWhiteToSpecificRadiance()
    End Sub

    Private Sub NormalizeWhiteToSpecificRadiance()
        Me.NormalizeColorData(divisor:=_SpectralRadiancePerWhite)
    End Sub

    Private Sub NormalizeToWhite()
        Dim white = Me.GetRgbLight(New RadianceSpectrum(Function(wavelength) 1), testStepCount:=_ColorArray.Count)

        Me.NormalizeColorData(divisorRed:=white.Red, divisorGreen:=white.Green, divisorBlue:=white.Blue)
    End Sub

    Private Sub NormalizeColorData(ByVal divisor As Double)
        Me.NormalizeColorData(divisorRed:=divisor, divisorGreen:=divisor, divisorBlue:=divisor)
    End Sub

    Private Sub NormalizeColorData(ByVal divisorRed As Double, ByVal divisorGreen As Double, ByVal divisorBlue As Double)
        For index = 0 To _ColorArray.Count - 1
            _ColorArray(index) = New RgbLight(red:=_ColorArray(index).Red / divisorRed,
                                              green:=_ColorArray(index).Green / divisorGreen,
                                              blue:=_ColorArray(index).Blue / divisorBlue)
        Next
    End Sub

    Public Function GetColor(ByVal wavelength As Double) As RgbLight
        If wavelength < LowerVisibleWavelengthBound OrElse wavelength > UpperVisibleWavelengthBound Then Return RgbLight.Black

        Dim index = CInt((wavelength - LowerVisibleWavelengthBound) / _WavelengthStep)

        Return _ColorArray(index)
    End Function

    Private Function GetRgbLight(ByVal radianceSpectrum As IRadianceSpectrum, ByVal testStepCount As Integer) As RgbLight
        Dim rgbLight = New RgbLight

        Dim interval = (UpperVisibleWavelengthBound - LowerVisibleWavelengthBound) / (testStepCount - 1)

        For index = 0 To testStepCount - 1
            Dim wavelength = LowerVisibleWavelengthBound + index * interval
            Dim intensity = radianceSpectrum.GetSpectralRadiance(wavelength)
            rgbLight += Me.GetColor(wavelength:=wavelength) * intensity
        Next

        Return rgbLight / testStepCount
    End Function

    Public Function Convert(ByVal light As RadianceSpectrum) As System.Drawing.Color Implements ILightToColorConverter(Of RadianceSpectrum).Convert
        Return _RgbLightToColorConverter.Convert(Me.GetRgbLight(radianceSpectrum:=light, testStepCount:=_TestStepCount))
    End Function
End Class