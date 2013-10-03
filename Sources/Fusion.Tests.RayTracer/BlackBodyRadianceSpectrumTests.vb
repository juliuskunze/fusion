Public Class BlackBodyRadianceSpectrumTests
    <Test()>
    Public Sub Test()
        Dim b = New BlackBodyRadianceSpectrum(temperature:=1000)
        'source: http://upload.wikimedia.org/wikipedia/commons/5/5b/BlackbodySpectrum_lin_150dpi_de.png
        Dim ratio = 4000 * 10 ^ 6 / b.GetSpectralRadiance(3 * 10 ^ -6)
        Assert.That(0.9 < ratio)
        Assert.That(ratio < 1.1)
    End Sub
End Class
