Public Class PartlyLorentzTransformationAtSightRayDirection
    Inherits LorentzTransformationAtSightRayDirection

    Private ReadOnly _Options As RadianceSpectrumLorentzTransformationOptions

    Public Sub New(relativeVelocity As Vector3D, sightRayDirection As Vector3D, options As RadianceSpectrumLorentzTransformationOptions)
        MyBase.New(relativeVelocity:=relativeVelocity, sightRayDirection:=sightRayDirection)
        _Options = options
    End Sub

    Public Sub New(transformation As LorentzTransformationAtSightRayDirection, options As RadianceSpectrumLorentzTransformationOptions)
        MyBase.New(transformation.RelativeVelocity, sightRayDirection:=transformation.NormalizedSightRayDirection)
        _Options = options
    End Sub

    Public ReadOnly Property Options As RadianceSpectrumLorentzTransformationOptions
        Get
            Return _Options
        End Get
    End Property

    Protected Overrides Function InverseTransformWavelength(wavelength As Double) As Double
        Return If(_Options.IgnoreDopplerEffect, wavelength, MyBase.InverseTransformWavelength(wavelength))
    End Function

    Public Overrides Function TransformSpectralRadiance(spectralRadiance As Double) As Double
        Return If(_Options.IgnoreSearchlightEffect, spectralRadiance, MyBase.TransformSpectralRadiance(spectralRadiance))
    End Function

    ''' <param name="radianceSpectrum">A radiance spectrum in S.</param>
    ''' <returns>The corresponding radiance spectrum in T.</returns>
    Public Overrides Function TransformRadianceSpectrum(radianceSpectrum As RadianceSpectrum) As RadianceSpectrum
        Return New RadianceSpectrum(TransformSpectralRadianceFunction(radianceSpectrum.Function))
    End Function

End Class
