Public Class HsbColorTests

    <Test()>
    Public Sub Test()
        Dim red = Color.Red
        Dim convertedRed = HsbColor.FromRgbColor(red).ToRgbColor
        Assert.That(ColorsEqual(red, convertedRed))

        Dim green = Color.Green
        Dim convertedGreen = HsbColor.FromRgbColor(green).ToRgbColor
        Assert.That(ColorsEqual(green, convertedGreen))

        Dim blue = Color.Blue
        Dim convertedBlue = HsbColor.FromRgbColor(blue).ToRgbColor
        Assert.That(ColorsEqual(blue, convertedBlue))

        Dim gray = Color.Gray
        Dim convertedGray = HsbColor.FromRgbColor(gray).ToRgbColor
        Assert.That(ColorsEqual(gray, convertedGray))

        Dim random = Color.FromArgb(18, 145, 245)
        Dim convertedRandom = HsbColor.FromRgbColor(random).ToRgbColor
        Assert.That(ColorsEqual(random, convertedRandom))
    End Sub

    Private Function ColorsEqual(color1 As Color, color2 As Color) As Boolean
        Return color1.R = color2.R AndAlso
               color1.G = color2.G AndAlso
               color1.B = color2.B
    End Function

End Class
