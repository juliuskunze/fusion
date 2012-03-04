Public Class SaturationStripedRainbowComplexToColor
    Implements IComplexToColor

    Private ReadOnly _PillowStrength As Double
    Private ReadOnly _LengthPartFactor As Double

    Public Sub New(pillowStrength As Double, lengthPartFactor As Double)
        If pillowStrength < 0 OrElse pillowStrength > 1 Then Throw New ArgumentOutOfRangeException("pillowStrength")

        _PillowStrength = pillowStrength
        _LengthPartFactor = lengthPartFactor
    End Sub

    Public Function GetColor(complex As Complex) As Color Implements IComplexToColor.GetColor
        Return New HsbColor(hue:=complex.Argument Mod 2 * PI,
                            saturation:=1 - _PillowStrength + _PillowStrength * PositiveNormalizedMod(Log(complex.Length, _LengthPartFactor), 1),
                            brightness:=1 - _PillowStrength + _PillowStrength * PositiveNormalizedMod(Log(complex.Length, _LengthPartFactor), 1)).ToRgbColor()
    End Function
End Class