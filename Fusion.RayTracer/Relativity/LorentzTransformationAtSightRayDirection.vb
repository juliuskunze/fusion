''' <summary>
''' Transforms (on a sightray with a specified direction) wavelength dependent values of a (stationary) reference frame S into a reference frame T which relativly moves with a constant velocity.
''' The events (0, 0, 0, 0) of both reference frames are the same.
''' </summary>
Public Class LorentzTransformationAtSightRayDirection
    Inherits LorentzTransformation

    Private ReadOnly _NormalizedSightRayDirection As Vector3D

    Public ReadOnly Property NormalizedSightRayDirection() As Vector3D
        Get
            Return _NormalizedSightRayDirection
        End Get
    End Property

    Private ReadOnly _GammaTheta As Double

    Public Sub New(relativeVelocity As Vector3D, sightRayDirection As Vector3D)
        MyBase.New(relativeVelocity:=relativeVelocity)
        _NormalizedSightRayDirection = sightRayDirection.Normalized

        _GammaTheta = If(RelativeVelocityIsNull, 1, 1 / (Gamma * (1 - Beta * _NormalizedSightRayDirection * NormalizedRelativeVelocity)))
    End Sub

    ''' <param name="wavelength">A wavelength in S.</param>
    ''' <returns>The corresponding wavelength in T.</returns>
    Public Function TransformWavelength(wavelength As Double) As Double
        Return wavelength / _GammaTheta
    End Function

    ''' <param name="spectralRadiance">A spectral radiance in S.</param>
    ''' <returns>The corresponding spectral radiance in T.</returns>
    Public Overridable Function TransformSpectralRadiance(spectralRadiance As Double) As Double
        Return spectralRadiance * _GammaTheta ^ 5
    End Function

    ''' <param name="spectralRadianceFunction">A spectral radiance function in S.</param>
    ''' <returns>The corresponding spectral radiance function in T.</returns>
    Public Overridable Function TransformSpectralRadianceFunction(spectralRadianceFunction As SpectralRadianceFunction) As SpectralRadianceFunction
        Return Function(wavelength) TransformSpectralRadiance(spectralRadianceFunction(InverseTransformWavelength(wavelength)))
    End Function

    Protected Overridable Function InverseTransformWavelength(wavelength As Double) As Double
        Return Inverse.TransformWavelength(wavelength)
    End Function

    ''' <param name="radianceSpectrum">A spectral radiance spectrum in S.</param>
    ''' <returns>The corresponding spectral radiance spectrum in T.</returns>
    Public Overridable Function TransformRadianceSpectrum(radianceSpectrum As RadianceSpectrum) As RadianceSpectrum
        Return New RadianceSpectrum(TransformSpectralRadianceFunction(radianceSpectrum.Function))
    End Function

    Public Shadows Function Inverse() As LorentzTransformationAtSightRayDirection
        Dim inverseLorentz = MyBase.Inverse

        Return inverseLorentz.AtSightRayDirection(inverseLorentz.TransformSightRayDirection(_NormalizedSightRayDirection))
    End Function

    Public Function Partly(options As RadianceSpectrumLorentzTransformationOptions) As PartlyLorentzTransformationAtSightRayDirection
        Return New PartlyLorentzTransformationAtSightRayDirection(Me, options)
    End Function

End Class
