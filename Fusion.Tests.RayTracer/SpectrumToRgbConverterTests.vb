Public Class SpectrumToRgbConverterTests

    Public Class SpectrumToRgbConverterFactoryTests

        <Test()>
        Public Sub TestPrimary()
            Dim converter = New SpectrumToRgbConverter

            Dim blue = converter.Convert(wavelength:=SpectrumToRgbConverter.BlueWavelength, intensity:=0.5)

            Assert.That(blue.Blue, [Is].GreaterThan(0))
            Assert.That(Abs(blue.Green), [Is].LessThan(0.001))
            Assert.That(Abs(blue.Red), [Is].LessThan(0.001))

            Dim green = converter.Convert(wavelength:=SpectrumToRgbConverter.GreenWavelength, intensity:=0.5)

            Assert.That(green.Green, [Is].GreaterThan(0))
            Assert.That(Abs(green.Blue), [Is].LessThan(0.001))
            Assert.That(Abs(green.Red), [Is].LessThan(0.001))

            Dim red = converter.Convert(wavelength:=SpectrumToRgbConverter.RedWavelength, intensity:=0.5)

            Assert.That(red.Red, [Is].GreaterThan(0))
            Assert.That(Abs(red.Green), [Is].LessThan(0.001))
            Assert.That(Abs(red.Blue), [Is].LessThan(0.001))

        End Sub

    End Class

End Class
