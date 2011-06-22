Public Class HueWavelengthConverter
    'source: http://www.mediengestalter.info/forum/22/wellenlaengen-in-rgb-werte-umrechnen-48642-2.html
    Private Const _RedWavelength = 700 * 10 ^ -9
    Private Const _GreenWaveLength = 546.1 * 10 ^ -9
    Private Const _BlueWaveLength = 435.8 * 10 ^ -9

    Public Shared Function WavelengthFromHue(ByVal hue As Double) As Double
        If Not (0 <= hue AndAlso hue < 2 * PI) Then Throw New ArgumentException("Hue has to be an angle in [0, 2 * Pi).")

        Const green = 2 * PI / 3
        Const magenta = 5 * PI / 3 'split point

        If magenta <= hue Then hue -= 2 * PI

        If hue < green Then
            ' 0 is red and 2*Pi/3 is green
            Return InterpolateWavelengths(_RedWavelength, _GreenWaveLength, hue / (PI / 3))
        ElseIf green <= hue Then
            ' 2*Pi/3 is green and 4*Pi/3 is blue
            Return InterpolateWavelengths(_GreenWaveLength, _BlueWaveLength, (hue - PI / 3) / (PI / 3))
        Else
            Return 0
        End If
    End Function

    Public Shared Function HueFromWavelength(ByVal wavelength As Double) As Double
        Throw New NotImplementedException
    End Function

    Private Shared Function InterpolateWavelengths(ByVal wavelength1 As Double, ByVal wavelength2 As Double, ByVal position As Double) As Double
        Return wavelength1 + position * (wavelength2 - wavelength1)
    End Function

End Class
