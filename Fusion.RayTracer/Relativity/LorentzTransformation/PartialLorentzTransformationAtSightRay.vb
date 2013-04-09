Public Class PartialLorentzTransformationAtSightRay
    Inherits LorentzTransformationAtSightRay

    Private ReadOnly _Options As LorentzTransformationAtSightRayOptions

    Public Sub New(transformation As LorentzTransformationAtSightRay, options As LorentzTransformationAtSightRayOptions)
        Me.New(RelativeVelocity:=transformation.RelativeVelocity, SightRay:=transformation.SightRay, options:=options)
        _Options = options
    End Sub

    Public Sub New(relativeVelocity As Vector3D, sightRay As SightRay, options As LorentzTransformationAtSightRayOptions)
        MyBase.New(relativeVelocity, sightRay)
        _Options = options
    End Sub

    Protected Overrides Function InverseTransformWavelength(wavelength As Double) As Double
        Return If(_Options.IgnoreDopplerEffect, wavelength, MyBase.InverseTransformWavelength(wavelength))
    End Function

    Public Overrides Function TransformSpectralRadiance(spectralRadiance As Double) As Double
        Return If(_Options.IgnoreSearchlightEffect, spectralRadiance, MyBase.TransformSpectralRadiance(spectralRadiance))
    End Function

    Public Overrides Function TransformSightRayDirection() As Vector3D
        Return If(_Options.IgnoreGeometryEffect, SightRay.Ray.NormalizedDirection, MyBase.TransformSightRayDirection)
    End Function

    Public Overrides Function TransformSightRay() As SightRay
        Return If(_Options.IgnoreGeometryEffect, SightRay, MyBase.TransformSightRay)
    End Function

    Public Function PartialInverse() As PartialLorentzTransformationAtSightRay
        Return InverseAtSightRay.[Partial](_Options)
    End Function

    Public Overrides Function Inverse() As LorentzTransformation
        Return PartialInverse()
    End Function
End Class
