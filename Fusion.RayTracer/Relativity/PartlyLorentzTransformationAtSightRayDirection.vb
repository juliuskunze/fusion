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

    Public Function InversePartly() As PartlyLorentzTransformationAtSightRayDirection
        Return InverseAtSightRayDirection.Partly(_Options)
    End Function

    Public Overrides Function Inverse() As LorentzTransformation
        Return InversePartly()
    End Function

End Class
