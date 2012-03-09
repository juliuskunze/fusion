Public Class RgbColorGammaCorrectorTests

    Private ReadOnly white As New RgbLight(1, 1, 1)
    Private ReadOnly gray As RgbLight = white / 2
    Private ReadOnly black As RgbLight = white * 0

    <Test()>
    Public Sub Test1()
        Dim gammaCorrector1 = New RgbColorGammaCorrector(1)

        Assert.AreEqual(gammaCorrector1.Run(gray), gray)
    End Sub

    <Test()>
    Public Sub Test2()
        Dim gammaCorrector2 = New RgbColorGammaCorrector(2)

        Assert.AreEqual(gammaCorrector2.Run(gray), white * 0.5 ^ 0.5)
        Assert.AreEqual(gammaCorrector2.Run(white), white)
        Assert.AreEqual(gammaCorrector2.Run(black), black)
    End Sub

End Class