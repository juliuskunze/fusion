''' <summary>
''' Transforms (on a sightray with a specified direction) wavelength dependent values of a (stationary) reference frame S into a reference frame T which relativly moves with a constant velocity.
''' The events (0, 0, 0, 0) of both reference frames are the same.
''' </summary>
Public Class LorentzTransformationAtSightRay
    Inherits LorentzTransformation

    Private ReadOnly _SightRay As SightRay

    Public ReadOnly Property SightRay As SightRay
        Get
            Return _SightRay
        End Get
    End Property

    Private ReadOnly _GammaTheta As Double

    Public ReadOnly Property GammaTheta As Double
        Get
            Return _GammaTheta
        End Get
    End Property

    Public Sub New(relativeVelocity As Vector3D, sightRay As Ray)
        Me.New(relativeVelocity, New SightRay(sightRay, 0))
    End Sub

    Public Sub New(relativeVelocity As Vector3D, sightRay As SightRay)
        MyBase.New(relativeVelocity:=relativeVelocity)
        _SightRay = sightRay

        _GammaTheta = If(RelativeVelocityIsNull, 1, 1 / (Gamma * (1 + Beta * _SightRay.Ray.NormalizedDirection * NormalizedRelativeVelocity)))
    End Sub

    ''' <param name="wavelength">A wavelength in S.</param>
    ''' <returns>The corresponding wavelength in T.</returns>
    Public Function TransformWavelength(wavelength As Double) As Double
        Return wavelength * _GammaTheta
    End Function

    ''' <param name="spectralRadiance">A spectral radiance in S.</param>
    ''' <returns>The corresponding spectral radiance in T.</returns>
    Public Overridable Function TransformSpectralRadiance(spectralRadiance As Double) As Double
        Return spectralRadiance / _GammaTheta ^ 5
    End Function

    ''' <param name="spectralRadianceFunction">A spectral radiance function in S.</param>
    ''' <returns>The corresponding spectral radiance function in T.</returns>
    Public Overridable Function TransformSpectralRadianceFunction(spectralRadianceFunction As SpectralRadianceFunction) As SpectralRadianceFunction
        Return Function(wavelength) TransformSpectralRadiance(spectralRadianceFunction(InverseTransformWavelength(wavelength)))
    End Function

    Protected Overridable Function InverseTransformWavelength(wavelength As Double) As Double
        Return InverseAtSightRay.TransformWavelength(wavelength)
    End Function

    ''' <param name="radianceSpectrum">A spectral radiance spectrum in S.</param>
    ''' <returns>The corresponding spectral radiance spectrum in T.</returns>
    Public Function TransformRadianceSpectrum(radianceSpectrum As RadianceSpectrum) As RadianceSpectrum
        Return New RadianceSpectrum(TransformSpectralRadianceFunction(radianceSpectrum.Function))
    End Function

    Public Function InverseAtSightRay() As LorentzTransformationAtSightRay
        Static lazy As New Lazy(Of LorentzTransformationAtSightRay)(Function() MyBase.Inverse.AtSightRay(TransformSightRay))
        Return lazy.Value
    End Function

    Public Overrides Function Inverse() As LorentzTransformation
        Return InverseAtSightRay()
    End Function

    Public Function Partly(options As LorentzTransformationAtSightRayOptions) As PartlyLorentzTransformationAtSightRay
        Return New PartlyLorentzTransformationAtSightRay(Me, options)
    End Function

    ''' <summary>
    ''' Transforms the direction, but keeps the origin event of the sight ray.
    ''' </summary>
    Public Function SemiTransformSightRay() As SightRay
        Return New SightRay(originEvent:=_SightRay.OriginEvent,
                            direction:=TransformSightRayDirection)
    End Function

    Public Overridable Function TransformSightRayDirection() As Vector3D
        Return -TransformVelocity(-_SightRay.Ray.NormalizedDirection.ScaledToLength(SpeedOfLight))
    End Function

    Public Overridable Shadows Function TransformSightRay() As SightRay
        Return MyBase.TransformSightRay(_SightRay)
    End Function
End Class
