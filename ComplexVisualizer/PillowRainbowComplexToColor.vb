Public Class PillowRainbowComplexToColor
    Implements IComplexToColor

    Private ReadOnly _PillowStrength As Double
    Private ReadOnly _LengthPartFactor As Double
    Private ReadOnly _ArgumentParts As Double

    Public Sub New(pillowStrength As Double, lengthPartFactor As Double, argumentParts As Double)
        If pillowStrength < 0 OrElse pillowStrength > 1 Then Throw New ArgumentOutOfRangeException("pillowStrength")

        _PillowStrength = pillowStrength
        _LengthPartFactor = lengthPartFactor
        _ArgumentParts = argumentParts
    End Sub

    Public Function GetColor(complex As Complex) As Color Implements IComplexToColor.GetColor
        Return New HsbColor(hue:=complex.Argument Mod 2 * PI,
                            saturation:=1,
                            brightness:=
                                1 - _PillowStrength +
                                _PillowStrength / 2 * PositiveNormalizedMod(Log(complex.Length, _ArgumentParts), 1) +
                                _PillowStrength / 2 * PositiveNormalizedMod(complex.Argument, 2 * PI / _LengthPartFactor)).ToRgbColor
    End Function
End Class