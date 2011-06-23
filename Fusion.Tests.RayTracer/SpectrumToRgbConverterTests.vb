Public Class SpectrumToRgbConverterTests

    Public Class SpectrumToRgbConverterFactoryTests

        <Test()>
        Public Sub TestPrimary()
            Assert.That(SpectrumToRgbConverter.Convert(wavelength:=SpectrumToRgbConverter.BlueWavelength, intensity:=17),
                        [Is].EqualTo(New RgbLight(red:=0, green:=0, blue:=17)))
        End Sub

    End Class

End Class
