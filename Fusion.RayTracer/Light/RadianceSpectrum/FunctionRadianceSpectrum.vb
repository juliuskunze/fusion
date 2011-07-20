Public Class FunctionRadianceSpectrum
    Implements ILight(Of FunctionRadianceSpectrum), IRadianceSpectrum

    Private ReadOnly _SpectralRadianceFunction As SpectralRadianceFunction
    Public ReadOnly Property IntensityFunction As SpectralRadianceFunction
        Get
            Return _SpectralRadianceFunction
        End Get
    End Property

    Private Shared ReadOnly _NullIntensityFunction As SpectralRadianceFunction = Function(wavelength)
                                                                                     Return 0
                                                                                 End Function

    Public Sub New()
        _SpectralRadianceFunction = _NullIntensityFunction
    End Sub

    Public Sub New(ByVal spectralRadianceFunction As SpectralRadianceFunction)
        _SpectralRadianceFunction = spectralRadianceFunction
    End Sub

    Public Sub New(ByVal radianceSpectrum As IRadianceSpectrum)
        _SpectralRadianceFunction = AddressOf radianceSpectrum.GetSpectralRadiance
    End Sub

    Public Function Add(ByVal other As FunctionRadianceSpectrum) As FunctionRadianceSpectrum Implements ILight(Of FunctionRadianceSpectrum).Add
        Return New FunctionRadianceSpectrum(AddressOf New FunctionRadianceSpectrum(Function(waveLength) Me.GetSpectralRadiance(waveLength) + other.GetSpectralRadiance(waveLength)).GetSpectralRadiance)
    End Function

    Public Function DivideBrightness(ByVal divisor As Double) As FunctionRadianceSpectrum Implements ILight(Of FunctionRadianceSpectrum).DivideBrightness
        Return New FunctionRadianceSpectrum(spectralRadianceFunction:=Function(waveLength) Me.GetSpectralRadiance(waveLength) / divisor)
    End Function

    Public Function MultiplyBrightness(ByVal factor As Double) As FunctionRadianceSpectrum Implements ILight(Of FunctionRadianceSpectrum).MultiplyBrightness
        Return New FunctionRadianceSpectrum(spectralRadianceFunction:=Function(waveLength) Me.GetSpectralRadiance(waveLength) * factor)
    End Function

    Public Function GetSpectralRadiance(ByVal wavelength As Double) As Double Implements IRadianceSpectrum.GetSpectralRadiance
        Return _SpectralRadianceFunction.Invoke(wavelength)
    End Function

End Class

Public Delegate Function SpectralRadianceFunction(ByVal wavelength As Double) As Double