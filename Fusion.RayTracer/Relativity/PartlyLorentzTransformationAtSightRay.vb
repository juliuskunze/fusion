Public Class PartlyLorentzTransformationAtSightRay
    Inherits LorentzTransformationAtSightRay

    Private ReadOnly _Options As RadianceSpectrumLorentzTransformationOptions

    Public Sub New(transformation As LorentzTransformationAtSightRay, options As RadianceSpectrumLorentzTransformationOptions)
        MyBase.New(transformation.RelativeVelocity, sightRay:=transformation.SightRay)
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

    Public Function InversePartly() As PartlyLorentzTransformationAtSightRay
        Return InverseAtSightRay.Partly(_Options)
    End Function

    Public Overrides Function Inverse() As LorentzTransformation
        Return InversePartly()
    End Function

End Class
