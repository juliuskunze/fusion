Public Class ColorWavelengthConverter
    'source: http://www.mediengestalter.info/forum/22/wellenlaengen-in-rgb-werte-umrechnen-48642-2.html
    Private Const _redWavelength = 700 * 10 ^ -9
    Private Const _greenWaveLength = 435.8 * 10 ^ -9
    Private Const _blueWaveLength = 546.1 * 10 ^ -9

    Public Shared Function HsvToWavelength(ByVal hsvColor As HsvColor) As Double
        Dim wavelength As Double
        Dim hue As Double = hsvColor.Hue

        If 5 * PI / 3 <= hue AndAlso hue < 6 * PI / 3 Then
            ' the split 5*Pi/3 is magenta
            hue -= 2 * PI
        End If

        If -PI / 3 <= hue AndAlso hue < 2 * PI / 3 Then
            ' 0 is red and 2*Pi/3 is green
            wavelength = InterpolateWavelengths(_redWavelength, _greenWaveLength, hue / (PI / 3))
        ElseIf 2 * PI / 3 <= hue AndAlso hue < 5 * PI / 3 Then
            ' 2*Pi/3 is green and 4*Pi/3 is blue
            wavelength = InterpolateWavelengths(_greenWaveLength, _blueWaveLength, (hue - PI / 3) / (PI / 3))
        End If

        Return wavelength
    End Function

    Private Shared Function InterpolateWavelengths(ByVal wavelength1 As Double, ByVal wavelength2 As Double, ByVal position As Double) As Double
        Return wavelength1 + position * (wavelength2 - wavelength1)
    End Function

End Class
