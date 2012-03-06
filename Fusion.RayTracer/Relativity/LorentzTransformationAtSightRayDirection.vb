﻿''' <summary>
''' Transforms (on a sightray with a specified direction) wavelength dependent values of a (stationary) reference frame S into a reference frame T which relativly moves with a constant velocity.
''' The events (0, 0, 0, 0) of both reference frames are the same.
''' </summary>
Public Class LorentzTransformationAtSightRayDirection
    Inherits LorentzTransformation

    Private ReadOnly _NormalizedSightRayDirectionInS As Vector3D

    Public ReadOnly Property NormalizedSightRayDirectionInS() As Vector3D
        Get
            Return _NormalizedSightRayDirectionInS
        End Get
    End Property

    Private ReadOnly _GammaTheta As Double

    Public Sub New(relativeVelocity As Vector3D, sightRayDirectionInS As Vector3D)
        MyBase.New(relativeVelocity:=relativeVelocity)
        _NormalizedSightRayDirectionInS = sightRayDirectionInS.Normalized

        _GammaTheta = If(RelativeVelocityIsNull, 1, 1 / (Gamma * (1 - Beta * _NormalizedSightRayDirectionInS * NormalizedRelativeVelocity)))
    End Sub

    ''' <param name="wavelength">A wavelength in S.</param>
    ''' <returns>The corresponding wavelength in T.</returns>
    Public Function TransformWavelength(wavelength As Double) As Double
        Return wavelength / _GammaTheta
    End Function

    ''' <param name="spectralRadiance">A spectral radiance in S.</param>
    ''' <returns>The corresponding spectral radiance in T.</returns>
    Public Function TransformSpectralRadiance(spectralRadiance As Double) As Double
        Return spectralRadiance * _GammaTheta ^ 5
    End Function

    ''' <param name="spectralRadianceFunction">A spectral radiance function in S.</param>
    ''' <returns>The corresponding spectral radiance function in T.</returns>
    Public Overridable Function TransformSpectralRadianceFunction(spectralRadianceFunction As SpectralRadianceFunction) As SpectralRadianceFunction
        Return Function(wavelengthInT) TransformSpectralRadiance(spectralRadiance:=spectralRadianceFunction(Inverse.TransformWavelength(wavelength:=wavelengthInT)))
    End Function

    ''' <param name="radianceSpectrum">A spectral radiance spectrum in S.</param>
    ''' <returns>The corresponding spectral radiance spectrum in T.</returns>
    Public Overridable Function TransformRadianceSpectrum(radianceSpectrum As RadianceSpectrum) As RadianceSpectrum
        Return New RadianceSpectrum(TransformSpectralRadianceFunction(spectralRadianceFunction:=radianceSpectrum.Function))
    End Function

    Public Shadows Function Inverse() As LorentzTransformationAtSightRayDirection
        Return MyBase.Inverse.AtSightRayDirection(_NormalizedSightRayDirectionInS)
    End Function

    Public Function Partly(options As RadianceSpectrumLorentzTransformationOptions) As PartlyLorentzTransformationAtSightRayDirection
        Return New PartlyLorentzTransformationAtSightRayDirection(Me, options)
    End Function

End Class
