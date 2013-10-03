Public Class RadianceSpectrum
    Implements ILight(Of RadianceSpectrum), IRadianceSpectrum

    Private ReadOnly _Function As SpectralRadianceFunction
    Public ReadOnly Property [Function] As SpectralRadianceFunction
        Get
            Return _Function
        End Get
    End Property

    Private Shared ReadOnly _NullFunction As SpectralRadianceFunction = Function(wavelength) 0

    Public Sub New()
        _Function = _NullFunction
    End Sub

    Public Sub New([function] As SpectralRadianceFunction)
        _Function = [function]
    End Sub

    Public Sub New(radianceSpectrum As IRadianceSpectrum)
        _Function = AddressOf radianceSpectrum.GetSpectralRadiance
    End Sub

    Public Function Add(other As RadianceSpectrum) As RadianceSpectrum Implements ILight(Of RadianceSpectrum).Add
        Return New RadianceSpectrum(AddressOf New RadianceSpectrum(Function(wavelength) Me.GetSpectralRadiance(waveLength) + other.GetSpectralRadiance(waveLength)).GetSpectralRadiance)
    End Function

    Public Function DivideBrightness(divisor As Double) As RadianceSpectrum Implements ILight(Of RadianceSpectrum).DivideBrightness
        Return New RadianceSpectrum(Function(wavelength) Me.GetSpectralRadiance(wavelength) / divisor)
    End Function

    Public Function MultiplyBrightness(factor As Double) As RadianceSpectrum Implements ILight(Of RadianceSpectrum).MultiplyBrightness
        Return New RadianceSpectrum([Function]:=Function(wavelength) Me.GetSpectralRadiance(wavelength) * factor)
    End Function

    Public Function GetSpectralRadiance(wavelength As Double) As Double Implements IRadianceSpectrum.GetSpectralRadiance
        Return _Function.Invoke(wavelength)
    End Function
End Class

Public Delegate Function SpectralRadianceFunction(wavelength As Double) As Double
