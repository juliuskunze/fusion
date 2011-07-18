Public Class BlackBodyRadianceSpectrumTests

    <Test()>
    Public Sub Test()
        Dim b = New BlackBodyRadianceSpectrum(temperature:=1000)
        'source: http://upload.wikimedia.org/wikipedia/commons/5/5b/BlackbodySpectrum_lin_150dpi_de.png
        'Assert.AreEqual(2*Constants.PlanckConstant * SpeedOfLight ^ 2 / ((3 * 10 ^ -6) ^ 5) / (System.Math.E ^ (Constants.SpeedOfLight * Constants.PlanckConstant / (1000 * Constants.BoltzmannConstant * 3 * 10 ^ -6)) - 1), b.GetSpectralRadiance(3 * 10 ^ -6))
        Dim ratio = 4000 * 10 ^ 6 / b.GetSpectralRadiance(3 * 10 ^ -6)
        Assert.That(0.9 < ratio)
        Assert.That(ratio < 1.1)
    End Sub

End Class
