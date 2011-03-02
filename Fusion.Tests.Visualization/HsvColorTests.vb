Public Class HsvColorTests

    <Test()>
    Public Sub Test()
        Dim red = Color.Red
        Dim convertedRed = HsvColor.FromRgbColor(red).ToRgbColor
        Assert.That(ColorsEqual(red, convertedRed))

        Dim green = Color.Green
        Dim convertedGreen = HsvColor.FromRgbColor(green).ToRgbColor
        Assert.That(ColorsEqual(green, convertedGreen))

        Dim blue = Color.Blue
        Dim convertedBlue = HsvColor.FromRgbColor(blue).ToRgbColor
        Assert.That(ColorsEqual(blue, convertedBlue))

        Dim gray = Color.Gray
        Dim convertedGray = HsvColor.FromRgbColor(gray).ToRgbColor
        Assert.That(ColorsEqual(gray, convertedGray))

        Dim random = Color.FromArgb(18, 145, 245)
        Dim convertedRandom = HsvColor.FromRgbColor(random).ToRgbColor
        Assert.That(ColorsEqual(random, convertedRandom))
    End Sub
    
    Private Function ColorsEqual(ByVal color1 As Color, ByVal color2 As Color) As Boolean
        Return color1.R = color2.R AndAlso
               color1.G = color2.G AndAlso
               color1.B = color2.B
    End Function

End Class
