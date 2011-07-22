Public Class RadianceSpectrum
    Implements ILight(Of RadianceSpectrum), IRadianceSpectrum

    Private ReadOnly _Function As SpectralRadianceFunction
    Public ReadOnly Property [Function] As SpectralRadianceFunction
        Get
            Return _Function
        End Get
    End Property

    Private Shared ReadOnly _NullFunction As SpectralRadianceFunction = Function(wavelength)
                                                                            Return 0
                                                                        End Function

    Public Sub New()
        _Function = _NullFunction
    End Sub

    Public Sub New(ByVal [function] As SpectralRadianceFunction)
        _Function = [function]
    End Sub

    Public Sub New(ByVal radianceSpectrum As IRadianceSpectrum)
        _Function = AddressOf radianceSpectrum.GetSpectralRadiance
    End Sub

    Public Function Add(ByVal other As RadianceSpectrum) As RadianceSpectrum Implements ILight(Of RadianceSpectrum).Add
        Return New RadianceSpectrum(AddressOf New RadianceSpectrum(Function(wavelength) Me.GetSpectralRadiance(waveLength) + other.GetSpectralRadiance(waveLength)).GetSpectralRadiance)
    End Function

    Public Function DivideBrightness(ByVal divisor As Double) As RadianceSpectrum Implements ILight(Of RadianceSpectrum).DivideBrightness
        Return New RadianceSpectrum(Function(wavelength) Me.GetSpectralRadiance(wavelength) / divisor)
    End Function

    Public Function MultiplyBrightness(ByVal factor As Double) As RadianceSpectrum Implements ILight(Of RadianceSpectrum).MultiplyBrightness
        Return New RadianceSpectrum([Function]:=Function(wavelength) Me.GetSpectralRadiance(wavelength) * factor)
    End Function

    Public Function GetSpectralRadiance(ByVal wavelength As Double) As Double Implements IRadianceSpectrum.GetSpectralRadiance
        Return _Function.Invoke(wavelength)
    End Function

End Class

Public Delegate Function SpectralRadianceFunction(ByVal wavelength As Double) As Double