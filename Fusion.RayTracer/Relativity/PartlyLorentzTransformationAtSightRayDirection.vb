Public Class PartlyLorentzTransformationAtSightRayDirection
    Inherits LorentzTransformationAtSightRayDirection

    Private ReadOnly _Options As RadianceSpectrumLorentzTransformationOptions

    Public Sub New(relativeVelocity As Vector3D, sightRayDirectionInS As Vector3D, options As RadianceSpectrumLorentzTransformationOptions)
        MyBase.New(relativeVelocity:=relativeVelocity, sightRayDirectionInS:=sightRayDirectionInS)
        _Options = options
    End Sub

    Public Sub New(transformation As LorentzTransformationAtSightRayDirection, options As RadianceSpectrumLorentzTransformationOptions)
        MyBase.New(transformation.RelativeVelocity, sightRayDirectionInS:=transformation.NormalizedSightRayDirectionInS)
        _Options = options
    End Sub

    Public ReadOnly Property Options As RadianceSpectrumLorentzTransformationOptions
        Get
            Return _Options
        End Get
    End Property

    ''' <param name="spectralRadianceFunction">A spectral radiance function in S.</param>
    ''' <returns>The corresponding spectral radiance function in T.</returns>
    Public Overrides Function TransformSpectralRadianceFunction(spectralRadianceFunction As SpectralRadianceFunction) As SpectralRadianceFunction
        If _Options.IgnoreDopplerEffect AndAlso _Options.IgnoreSearchlightEffect Then Return spectralRadianceFunction
        If _Options.IgnoreSearchlightEffect Then Return Function(wavelengthInT) spectralRadianceFunction(Inverse.AtSightRayDirection(NormalizedSightRayDirectionInS).TransformWavelength(wavelength:=wavelengthInT))
        If _Options.IgnoreDopplerEffect Then Return Function(wavelengthInT) AtSightRayDirection(NormalizedSightRayDirectionInS).TransformSpectralRadiance(spectralRadianceFunction(wavelengthInT))

        Return AtSightRayDirection(NormalizedSightRayDirectionInS).TransformSpectralRadianceFunction(spectralRadianceFunction)
    End Function

    ''' <param name="radianceSpectrum">A radiance spectrum in S.</param>
    ''' <returns>The corresponding radiance spectrum in T.</returns>
    Public Overrides Function TransformRadianceSpectrum(radianceSpectrum As RadianceSpectrum) As RadianceSpectrum
        Return New RadianceSpectrum(TransformSpectralRadianceFunction(radianceSpectrum.Function))
    End Function

End Class
