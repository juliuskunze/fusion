Public Class ExactColorTests

    Private ReadOnly _ColorConverter As New RgbLightToRgbColorConverter

    <Test()>
    Public Sub Black()
        Dim c = Color.FromArgb(0, 0, 0)
        Dim exact = New RgbLight(c)
        Dim c2 = _ColorConverter.Convert(exact)

        Assert.AreEqual(c, c2)
    End Sub

    <Test()>
    Public Sub UpperBound()
        Dim c = Color.FromArgb(255, 255, 0)
        Dim exact = New RgbLight(1, 1, 0)
        Dim c2 = _ColorConverter.Convert(exact)

        Assert.AreEqual(c, c2)
    End Sub

    <Test()>
    Public Sub Truncate()
        Dim c = Color.FromArgb(0, 0, 255)
        Dim exact = New RgbLight(0, 0, 2)
        Dim c2 = _ColorConverter.Convert(exact)

        Assert.AreEqual(c, c2)
    End Sub

End Class
