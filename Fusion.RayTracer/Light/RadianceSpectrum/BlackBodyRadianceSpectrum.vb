Public Class BlackBodyRadianceSpectrum
    Implements IRadianceSpectrum

    Private ReadOnly _ExponentFactor As Double

    Public Sub New(temperature As Double)
        If temperature <= 0 Then Throw New ArgumentOutOfRangeException(paramName:="temperature", message:="The temperature of a black body must be positive.")

        _ExponentFactor = Constants.PlanckConstant * Constants.SpeedOfLight / (Constants.BoltzmannConstant * temperature)
    End Sub

    Public Function GetSpectralRadiance(wavelength As Double) As Double Implements IRadianceSpectrum.GetSpectralRadiance
        'source: http://de.wikipedia.org/wiki/Plancksches_Strahlungsgesetz
        '(without factor pi, because this function returns spectral radiance not a radiance) 
        '2 * h * c^2 / lambda ^ 5 / (e ^ (h * c / (lambda * k * T)) - 1)
        Return 2 * Constants.PlanckConstant * Constants.SpeedOfLight ^ 2 / (wavelength ^ 5 * (System.Math.Exp(_ExponentFactor / wavelength) - 1))
    End Function

    Public Overrides Function ToString() As String
        Return 2 * Constants.PlanckConstant * Constants.SpeedOfLight ^ 2 & " / lambda ^ 5 / (e ^ (" & _ExponentFactor & " / lambda)) - 1)"
    End Function

End Class
