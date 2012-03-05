Public Class RadianceSpectrumLorentzTransformation
    Private ReadOnly _Transformation As LorentzTransformation
    Private ReadOnly _Options As RadianceSpectrumLorentzTransformationOptions

    Public Sub New(transformation As LorentzTransformation, options As RadianceSpectrumLorentzTransformationOptions)
        _Transformation = transformation
        _Options = options
    End Sub

    Public ReadOnly Property Options As RadianceSpectrumLorentzTransformationOptions
        Get
            Return _Options
        End Get
    End Property

    ''' <param name="spectralRadianceFunction">A spectral radiance function in S.</param>
    ''' <returns>The corresponding spectral radiance function in T.</returns>
    Public Function TransformSpectralRadianceFunction(normalizedSightRayDirectionInS As Vector3D, spectralRadianceFunction As SpectralRadianceFunction) As SpectralRadianceFunction
        If _Options.IgnoreDopplerEffect AndAlso _Options.IgnoreSearchlightEffect Then Return spectralRadianceFunction
        If _Options.IgnoreSearchlightEffect Then Return Function(wavelengthInT) spectralRadianceFunction(_Transformation.Inverse.AtDirection(normalizedSightRayDirectionInS).TransformWavelength(wavelength:=wavelengthInT))
        If _Options.IgnoreDopplerEffect Then Return Function(wavelengthInT) _Transformation.AtDirection(normalizedSightRayDirectionInS).TransformSpectralRadiance(spectralRadiance:=spectralRadianceFunction(wavelengthInT))

        Return _Transformation.AtDirection(normalizedSightRayDirectionInS).TransformSpectralRadianceFunction(spectralRadianceFunction:=spectralRadianceFunction)
    End Function
End Class
