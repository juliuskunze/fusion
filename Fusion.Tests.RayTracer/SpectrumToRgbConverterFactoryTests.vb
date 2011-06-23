Public Class SpectrumToRgbConverterFactoryTests

    <Test()>
    Public Sub TestPrimary()
        Dim spectrumToRgbConverter = SpectrumToRgbConverterFactory.Create

        Assert.That(spectrumToRgbConverter.Convert(wavelength:=spectrumToRgbConverter.BlueWavelength, intensity:=17),
                    [Is].EqualTo(New RgbLight(red:=0, green:=0, blue:=17)))
    End Sub

End Class
