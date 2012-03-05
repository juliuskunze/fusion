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

    Public Function GetSpectralRadianceFunctionInT(viewRayInS As Ray, spectralRadianceFunctionInS As SpectralRadianceFunction) As SpectralRadianceFunction
        If _Options.IgnoreDopplerEffect AndAlso _Options.IgnoreSearchlightEffect Then Return spectralRadianceFunctionInS
        If _Options.IgnoreSearchlightEffect Then Return Function(wavelengthInT) spectralRadianceFunctionInS(_Transformation.InverseTransformWavelength(sightRayInS:=viewRayInS, wavelengthInT:=wavelengthInT))
        If _Options.IgnoreDopplerEffect Then Return Function(wavelengthInT) _Transformation.TransformSpectralRadiance(sightRayInS:=viewRayInS, spectralRadianceInS:=spectralRadianceFunctionInS(wavelengthInT))

        Return _Transformation.TransformSpectralRadianceFunction(sightRayInS:=viewRayInS, spectralRadianceFunctionInS:=spectralRadianceFunctionInS)
    End Function
End Class
