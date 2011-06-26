Public Class SpectrumToRgbConverterTests

    <Test()>
    Public Sub TestWhite()
        Dim _Converter = New SpectrumToRgbConverter(testStepCount:=150)

        Dim whiteSpectrum = New FunctionLightSpectrum(IntensityFunction:=Function(wavelength) 1)
        Dim white = _Converter.Convert(whiteSpectrum)

        Assert.That(Abs(1 - white.Red), [Is].LessThan(0.01))
        Assert.That(Abs(1 - white.Green), [Is].LessThan(0.01))
        Assert.That(Abs(1 - white.Blue), [Is].LessThan(0.01))

    End Sub

    <Test()>
    Public Sub TestBlue()
        Dim _Converter = New SpectrumToRgbConverter(testStepCount:=25)

        Dim blueSpectrum = New FunctionLightSpectrum(IntensityFunction:=Function(wavelength)
                                                                            If wavelength > 400 * 10 ^ -9 AndAlso wavelength < 430 * 10 ^ -9 Then
                                                                                Return 2
                                                                            Else
                                                                                Return 0
                                                                            End If
                                                                        End Function)

        Dim blue = _Converter.Convert(blueSpectrum)

        Assert.That(blue.Blue, [Is].GreaterThan(10 * blue.Green))
        Assert.That(blue.Blue, [Is].GreaterThan(2 * blue.Red))

    End Sub

End Class
