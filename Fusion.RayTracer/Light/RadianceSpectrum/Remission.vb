Public Class Remission
    Implements IRemission(Of RadianceSpectrum)

    Private ReadOnly _AlbedoSpectrum As Func(Of Double, Double)

    Public Sub New(albedoSpectrum As Func(Of Double, Double))
        _AlbedoSpectrum = albedoSpectrum
    End Sub

    Public Function GetRemission(light As RadianceSpectrum) As RadianceSpectrum Implements IRemission(Of RadianceSpectrum).GetRemission
        Return New RadianceSpectrum(Function(wavelength) _AlbedoSpectrum(wavelength) * light.Function(wavelength))
    End Function

    Public ReadOnly Property IsBlack As Boolean Implements IRemission(Of RadianceSpectrum).IsBlack
        Get
            Return False
        End Get
    End Property
End Class
