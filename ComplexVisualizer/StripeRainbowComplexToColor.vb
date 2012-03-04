Public Class StripeRainbowComplexToColor
    Implements IComplexToColor

    Public Function GetColor(complex As Complex) As Color Implements IComplexToColor.GetColor
        Return New HsbColor(complex.Argument Mod 2 * PI,
                            1,
                            1 / 2 + 1 / 2 * PositiveMod(Log(complex.Length, 1.5), 1)).ToRgbColor
    End Function
End Class