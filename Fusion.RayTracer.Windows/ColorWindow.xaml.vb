Public Class ColorWindow

    Private _RgbLightToColorConverter As New RgbLightToColorConverter
    Private _RadianceSpectrumToColorConverter As RadianceSpectrumToColorConverter

    Public Sub New()
        Me.InitializeComponent()
    End Sub

    Private Sub IntensitySlider_ValueChanged(sender As System.Object, e As System.Windows.RoutedPropertyChangedEventArgs(Of System.Double)) Handles _IntensitySlider.ValueChanged
        _RadianceSpectrumToColorConverter = New RadianceSpectrumToColorConverter(testStepCount:=150, spectralRadiancePerWhite:=_IntensitySlider.Value)
        Me.DrawRelativisticColors()
    End Sub

    Private Sub DrawMonochromaticSpectrum()
        Dim width = 1000
        Dim height = 400

        Dim bitmap = New SimpleBitmap(width:=width, height:=height)

        Dim wavelengthStep = (RadianceSpectrumToColorConverter.UpperVisibleWavelengthBound - RadianceSpectrumToColorConverter.LowerVisibleWavelengthBound) / width

        For x = 0 To width - 1
            Dim color = _RgbLightToColorConverter.Convert(_RadianceSpectrumToColorConverter.GetColor(wavelength:=RadianceSpectrumToColorConverter.LowerVisibleWavelengthBound + x * wavelengthStep))

            For y = 0 To height - 1
                bitmap.SetPixel(x:=x, y:=y, color:=color)
            Next

        Next

        Dim white = _RadianceSpectrumToColorConverter.Convert(New RadianceSpectrum(Function(wavelength) 1))

        'bitmap.Clear(white)

        _ColorsImage.Source = bitmap.ToBitmapSource
    End Sub

    Private Sub DrawBlackbodySpectrum()
        Dim width = 1000
        Dim height = 200

        Dim bitmap = New SimpleBitmap(width:=width, height:=height)

        Dim lowerTemperatureBound = 0
        Dim upperTemperatureBound = 16000

        Dim temperatureStep = (upperTemperatureBound - lowerTemperatureBound) / width

        For x = 0 To width - 1
            Dim color = _RadianceSpectrumToColorConverter.Convert(New RadianceSpectrum(New BlackBodyRadianceSpectrum(lowerTemperatureBound + x * temperatureStep)))

            For y = 0 To height - 1
                bitmap.SetPixel(x:=x, y:=y, color:=color)
            Next

        Next

        _ColorsImage.Source = bitmap.ToBitmapSource
    End Sub

    Private Sub DrawRelativisticColors()
        Const width = 1000
        Const height = 200

        Dim bitmap = New SimpleBitmap(width:=width, height:=height)

        Dim temperature = 2800

        Const minBeta = -0.9999999
        Const maxBeta = 0.9999999

        Const betaStep = (maxBeta - minBeta) / width


        For x = 0 To width - 1
            Dim beta = minBeta + x * betaStep
            Dim transformation = New RelativisticRadianceTransformation(New Vector3D(beta * SpeedOfLight, 0, 0))

            Const minTemperature = 0
            Const maxTemperature = 16000

            Const temperatureStep = (maxTemperature - minTemperature) / height

            For y = 0 To height - 1
                Dim color = _RadianceSpectrumToColorConverter.Convert(transformation.GetRadianceSpectrumInT(viewRayInS:=New Ray(New Vector3D, New Vector3D(1, 0, 0)), radianceSpectrumInS:=New RadianceSpectrum(New BlackBodyRadianceSpectrum(minTemperature + y * temperatureStep))))
                bitmap.SetPixel(x:=x, y:=y, color:=Color)
            Next

        Next

        _ColorsImage.Source = bitmap.ToBitmapSource
    End Sub

End Class
