Public Class ColorWindow

    Private _RgbLightToColorConverter As New RgbLightToColorConverter
    Private _RadianceSpectrumToColorConverter As RadianceSpectrumToColorConverter

    Public Sub New()
        Me.InitializeComponent()
    End Sub

    Private Sub IntensitySlider_ValueChanged(ByVal sender As System.Object, ByVal e As System.Windows.RoutedPropertyChangedEventArgs(Of System.Double)) Handles _IntensitySlider.ValueChanged
        _RadianceSpectrumToColorConverter = New RadianceSpectrumToColorConverter(testStepCount:=150, spectralRadiancePerWhite:=_IntensitySlider.Value)
        Me.DrawBlackbodySpectrum()
    End Sub

    Private Sub DrawMonochromaticSpectrum()
        Dim width = 1000
        Dim height = 400

        Dim bitmap = New SimpleBitmap(width:=width, height:=height)

        Dim wavelengthStep = (RadianceSpectrumToColorConverter.UpperVisibleWavelengthBound - RadianceSpectrumToColorConverter.LowerVisibleWavelengthBound) / width
        For x = 0 To width - 1
            Dim color = _RadianceSpectrumToColorConverter.GetSpectralRadiance(wavelength:=RadianceSpectrumToColorConverter.LowerVisibleWavelengthBound + x * wavelengthStep) * _IntensitySlider.Value

            For y = 0 To height - 1
                bitmap.SetPixel(x:=x, y:=y, color:=_RgbLightToColorConverter.Convert(color))
            Next

        Next

        Dim white = _RadianceSpectrumToColorConverter.Convert(New FunctionRadianceSpectrum(spectralRadianceFunction:=Function(wavelength) 1))

        ' bitmap.Clear(white.ToColor)

        _MonochromaticColorsImage.Source = bitmap.ToBitmapSource
    End Sub

    Private Sub DrawBlackbodySpectrum()
        Dim width = 1000
        Dim height = 200

        Dim bitmap = New SimpleBitmap(width:=width, height:=height)

        Dim lowerTemperatureBound = 0
        Dim upperTemperatureBound = 16000

        Dim temperatureStep = (upperTemperatureBound - lowerTemperatureBound) / width

        For x = 0 To width - 1
            Dim color = _RadianceSpectrumToColorConverter.Convert(New FunctionRadianceSpectrum(New BlackBodyRadianceSpectrum(lowerTemperatureBound + x * temperatureStep)))

            For y = 0 To height - 1
                bitmap.SetPixel(x:=x, y:=y, color:=color)
            Next

        Next

        _BlackBodyImage.Source = bitmap.ToBitmapSource
    End Sub

End Class
