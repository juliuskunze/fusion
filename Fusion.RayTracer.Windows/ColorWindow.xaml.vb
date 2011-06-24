Public Class ColorWindow

    Private ReadOnly _SpectrumToRgbConverter As New SpectrumToRgbConverter(testStepCount:=25)

    Public Sub New()
        Me.InitializeComponent()

        ' Me.DrawSpectrum()
    End Sub

    Private Sub IntensitySlider_ValueChanged(ByVal sender As System.Object, ByVal e As System.Windows.RoutedPropertyChangedEventArgs(Of System.Double)) Handles _IntensitySlider.ValueChanged
        Me.DrawSpectrum()
    End Sub

    Private Sub DrawSpectrum()
        Dim width = 1400
        Dim height = 1000

        Dim wavelengthStep = (SpectrumToRgbConverter.UpperWavelengthBound - SpectrumToRgbConverter.LowerWavelengthBound) / width

        Dim bitmap = New SimpleBitmap(width:=width, height:=height)

        'For x = 0 To width - 1
        '    Dim unchangedColor = _SpectrumToRgbConverter.GetIntensityPerWavelength(wavelength:=SpectrumToRgbConverter.LowerWavelengthBound + x * wavelengthStep,
        '                                                         intensity:=_IntensitySlider.Value)

        '    Dim color = (New HsbColor(hue:=HsbColor.FromRgbColor(unchangedColor.ToColor).Hue,
        '                              saturation:=HsbColor.FromRgbColor(unchangedColor.ToColor).Saturation,
        '                              brightness:=1)).ToRgbColor

        '    For y = 0 To height - 1
        '        bitmap.SetPixel(x:=x, y:=y, color:=unchangedColor.ToColor)
        '    Next

        'Next

        Dim white = _SpectrumToRgbConverter.Convert(New FunctionLightSpectrum(IntensityFunction:=Function(wavelength) 1))

        bitmap.Clear(white.ToColor)

        _Image.Source = bitmap.ToBitmapSource
    End Sub
End Class
