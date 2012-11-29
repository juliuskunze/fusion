Public Class RadianceSpectrumToColorConverterTests
    Const _ExampleRadiancePerWhite = 25

    Private ReadOnly _Converter As New RadianceSpectrumToRgbColorConverter(testedWavelengthsCount:=150, spectralRadiancePerWhite:=_ExampleRadiancePerWhite, bitmapFilePath:="C:\Fusion Repository\Fusion.RayTracer\Data\2000pixel spectrum sRGB (380nm to 710nm).bmp")

    <Test()>
    Public Sub TestWhite()
        Dim whiteSpectrum = New RadianceSpectrum([function]:=Function(wavelength) _ExampleRadiancePerWhite)
        Dim white = _Converter.Run(whiteSpectrum)

        Assert.That(Abs(1 - white.R / 255), [Is].LessThan(0.01))
        Assert.That(Abs(1 - white.G / 255), [Is].LessThan(0.01))
        Assert.That(Abs(1 - white.B / 255), [Is].LessThan(0.01))

    End Sub

    <Test()>
    Public Sub TestBlue()
        Dim blueSpectrum = New RadianceSpectrum([function]:=Function(wavelength)
                                                                If wavelength > 400 * 10 ^ -9 AndAlso wavelength < 430 * 10 ^ -9 Then
                                                                    Return 2
                                                                Else
                                                                    Return 0
                                                                End If
                                                            End Function)

        Dim blue = _Converter.Run(blueSpectrum)

        Assert.That(blue.B, [Is].GreaterThan(10 * blue.G))
        Assert.That(blue.B, [Is].GreaterThan(2 * blue.R))
    End Sub
End Class
