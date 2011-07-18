Public Class ColorWindow

    Private _SpectrumToRgbConverter As New SpectrumToRgbConverter(testStepCount:=150)

    Public Sub New()
        Me.InitializeComponent()
    End Sub

    Private Sub IntensitySlider_ValueChanged(ByVal sender As System.Object, ByVal e As System.Windows.RoutedPropertyChangedEventArgs(Of System.Double)) Handles _IntensitySlider.ValueChanged
        Me.DrawBlackbodySpectrum()
    End Sub

    Private Sub DrawMonochromaticSpectrum()
        Dim width = 1000
        Dim height = 400

        Dim bitmap = New SimpleBitmap(width:=width, height:=height)

        Dim wavelengthStep = (SpectrumToRgbConverter.UpperVisibleWavelengthBound - SpectrumToRgbConverter.LowerVisibleWavelengthBound) / width
        For x = 0 To width - 1
            Dim color = _SpectrumToRgbConverter.GetSpectralRadiance(wavelength:=SpectrumToRgbConverter.LowerVisibleWavelengthBound + x * wavelengthStep) * _IntensitySlider.Value

            For y = 0 To height - 1
                bitmap.SetPixel(x:=x, y:=y, color:=color.ToColor)
            Next

        Next

        Dim white = _SpectrumToRgbConverter.Convert(New RadianceSpectrum(spectralRadianceFunction:=Function(wavelength) 1))

        ' bitmap.Clear(white.ToColor)

        _MonochromaticColorsImage.Source = bitmap.ToBitmapSource
    End Sub

    Private Sub DrawBlackbodySpectrum()
        Dim width = 1000
        Dim height = 200

        Dim bitmap = New SimpleBitmap(width:=width, height:=height)

        Dim lowerTemperatureBound = 1000
        Dim upperTemperatureBound = 16000

        Dim temperatureStep = (upperTemperatureBound - lowerTemperatureBound) / width

        For x = 0 To width - 1
            Dim color = New RadianceSpectrum(New BlackBodyRadianceSpectrum(lowerTemperatureBound + x * temperatureStep)).MultiplyBrightness(_IntensitySlider.Value)

            For y = 0 To height - 1
                bitmap.SetPixel(x:=x, y:=y, color:=color.ToColor)
            Next

        Next

        _BlackBodyImage.Source = bitmap.ToBitmapSource
    End Sub

End Class
