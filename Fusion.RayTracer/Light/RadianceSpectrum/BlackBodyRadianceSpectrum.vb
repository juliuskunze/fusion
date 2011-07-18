Public Class BlackBodyRadianceSpectrum
    Implements IRadianceSpectrum

    Private ReadOnly _ExponentFactor As Double

    Public Sub New(ByVal temperature As Double)
        _ExponentFactor = Constants.PlanckConstant * Constants.SpeedOfLight / (Constants.BoltzmannConstant * temperature)
    End Sub

    Public Function GetSpectralRadiance(ByVal wavelength As Double) As Double Implements IRadianceSpectrum.GetSpectralRadiance
        'source: http://de.wikipedia.org/wiki/Plancksches_Strahlungsgesetz
        '(without factor 2*pi) h * c^2 / lambda ^ 5 / (e ^ (h * c / (lambda * k * T)) - 1)
        Return 2 * Constants.PlanckConstant * Constants.SpeedOfLight ^ 2 / (wavelength ^ 5 * (System.Math.Exp(_ExponentFactor / wavelength) - 1))
    End Function

End Class
