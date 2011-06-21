Public Class ColorWavelengthConverter
    'source: http://www.mediengestalter.info/forum/22/wellenlaengen-in-rgb-werte-umrechnen-48642-2.html
    Private Const _RedWavelength = 700 * 10 ^ -9
    Private Const _GreenWaveLength = 435.8 * 10 ^ -9
    Private Const _BlueWaveLength = 546.1 * 10 ^ -9

    Public Shared Function BrightnessToWavelength(ByVal hsbColor As HsbColor) As Double
        Dim wavelength As Double
        Dim hue As Double = hsbColor.Hue

        If 5 * PI / 3 <= hue AndAlso hue < 6 * PI / 3 Then
            ' the split 5*Pi/3 is magenta
            hue -= 2 * PI
        End If

        If -PI / 3 <= hue AndAlso hue < 2 * PI / 3 Then
            ' 0 is red and 2*Pi/3 is green
            wavelength = InterpolateWavelengths(_RedWavelength, _GreenWaveLength, hue / (PI / 3))
        ElseIf 2 * PI / 3 <= hue AndAlso hue < 5 * PI / 3 Then
            ' 2*Pi/3 is green and 4*Pi/3 is blue
            wavelength = InterpolateWavelengths(_GreenWaveLength, _BlueWaveLength, (hue - PI / 3) / (PI / 3))
        End If

        Return wavelength
    End Function

    Private Shared Function InterpolateWavelengths(ByVal wavelength1 As Double, ByVal wavelength2 As Double, ByVal position As Double) As Double
        Return wavelength1 + position * (wavelength2 - wavelength1)
    End Function

End Class
