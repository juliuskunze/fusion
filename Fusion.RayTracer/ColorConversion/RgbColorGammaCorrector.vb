Public Class RgbColorGammaCorrector
    Private ReadOnly _MonitorGamma As Double

    Public Sub New(monitorGamma As Double)
        _MonitorGamma = monitorGamma
    End Sub

    Public ReadOnly Property MonitorGamma As Double
        Get
            Return _MonitorGamma
        End Get
    End Property

    Public Function Run(light As RgbLight) As RgbLight
        Dim brightness = (light.Red + light.Green + light.Blue) / 3

        If brightness = 0 Then Return New RgbLight

        Return light.MultiplyBrightness(brightness ^ (1 / MonitorGamma - 1))
    End Function
End Class
