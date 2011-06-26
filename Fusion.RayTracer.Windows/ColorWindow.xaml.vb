Public Class ColorWindow

    Private _SpectrumToRgbConverter As New SpectrumToRgbConverter(testStepCount:=150)

    Public Sub New()
        Me.InitializeComponent()

        ' Me.DrawSpectrum()
    End Sub

    Private Sub IntensitySlider_ValueChanged(ByVal sender As System.Object, ByVal e As System.Windows.RoutedPropertyChangedEventArgs(Of System.Double)) Handles _IntensitySlider.ValueChanged
        Me.DrawSpectrum()
    End Sub

    Private Sub DrawSpectrum()
        Dim width = 1000
        Dim height = 400

        Dim bitmap = New SimpleBitmap(width:=width, height:=height)

        Dim wavelengthStep = (SpectrumToRgbConverter.UpperWavelengthBound - SpectrumToRgbConverter.LowerWavelengthBound) / width
        For x = 0 To width - 1
            Dim color = _SpectrumToRgbConverter.GetIntensityPerWavelength(wavelength:=SpectrumToRgbConverter.LowerWavelengthBound + x * wavelengthStep) * _IntensitySlider.Value

            For y = 0 To height - 1
                bitmap.SetPixel(x:=x, y:=y, color:=color.ToColor)
            Next

        Next

        Dim white = _SpectrumToRgbConverter.Convert(New FunctionLightSpectrum(IntensityFunction:=Function(wavelength) 1))

        ' bitmap.Clear(white.ToColor)

        _Image.Source = bitmap.ToBitmapSource
    End Sub
End Class
