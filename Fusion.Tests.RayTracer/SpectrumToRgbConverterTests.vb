Public Class SpectrumToRgbConverterTests

    Public Class SpectrumToRgbConverterFactoryTests

        Private _Converter As SpectrumToRgbConverter = New SpectrumToRgbConverter(testStepCount:=25)

        <Test()>
        Public Sub TestPrimary()
            Dim blue = _Converter.GetIntensityPerWavelength(wavelength:=SpectrumToRgbConverter.BlueWavelength, intensity:=0.5)

            Assert.That(blue.Blue, [Is].GreaterThan(0))
            Assert.That(Abs(blue.Green), [Is].LessThan(0.001))
            Assert.That(Abs(blue.Red), [Is].LessThan(0.001))

            Dim green = _Converter.GetIntensityPerWavelength(wavelength:=SpectrumToRgbConverter.GreenWavelength, intensity:=0.5)

            Assert.That(green.Green, [Is].GreaterThan(0))
            Assert.That(Abs(green.Blue), [Is].LessThan(0.001))
            Assert.That(Abs(green.Red), [Is].LessThan(0.001))

            Dim red = _Converter.GetIntensityPerWavelength(wavelength:=SpectrumToRgbConverter.RedWavelength, intensity:=0.5)

            Assert.That(red.Red, [Is].GreaterThan(0))
            Assert.That(Abs(red.Green), [Is].LessThan(0.001))
            Assert.That(Abs(red.Blue), [Is].LessThan(0.001))

        End Sub

        <Test()>
        Public Sub TestWhite()
            Dim whiteSpectrum = New FunctionLightSpectrum(IntensityFunction:=Function(wavelength) 1)

            Dim white = _Converter.Convert(whiteSpectrum)

            Assert.That(Abs(1 - white.Red), [Is].LessThan(0.05))
            Assert.That(Abs(1 - white.Green), [Is].LessThan(0.05))
            Assert.That(Abs(1 - white.Blue), [Is].LessThan(0.05))

        End Sub

        <Test()>
        Public Sub TestBlue()
            Dim blueSpectrum = New FunctionLightSpectrum(IntensityFunction:=Function(wavelength)
                                                                                If wavelength > 380 * 10 ^ -9 AndAlso wavelength < 530 * 10 ^ -9 Then
                                                                                    Return 1
                                                                                Else
                                                                                    Return 0
                                                                                End If
                                                                            End Function)

            Dim blue = _Converter.Convert(blueSpectrum)

            Assert.That(blue.Red, [Is].LessThan(0.1))
            Assert.That(blue.Green, [Is].LessThan(0.5))
            Assert.That(blue.Blue, [Is].GreaterThan(0.9))

        End Sub

    End Class

End Class
