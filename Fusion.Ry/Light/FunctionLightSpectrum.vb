Public Class FunctionLightSpectrum
    Implements ILightSpectrum(Of FunctionLightSpectrum)

    Private ReadOnly _IntensityFunction As IntensityFunction
    Public ReadOnly Property IntensityFunction As IntensityFunction
        Get
            Return _IntensityFunction
        End Get
    End Property

    Private Shared ReadOnly _NullIntensityFunction As IntensityFunction = Function(wavelength)
                                                                              Return 0
                                                                          End Function

    Public Sub New()
        _IntensityFunction = _NullIntensityFunction
    End Sub

    Public Sub New(ByVal intensityFunction As IntensityFunction)
        _IntensityFunction = intensityFunction
    End Sub

    Public Function Add(ByVal other As FunctionLightSpectrum) As FunctionLightSpectrum Implements ILight(Of FunctionLightSpectrum).Add
        Return New FunctionLightSpectrum(IntensityFunction:=Function(waveLength) Me.GetIntensity(waveLength) + other.GetIntensity(waveLength))
    End Function

    Public Function DivideBrightness(ByVal divisor As Double) As FunctionLightSpectrum Implements ILight(Of FunctionLightSpectrum).DivideBrightness
        Return New FunctionLightSpectrum(IntensityFunction:=Function(waveLength) Me.GetIntensity(waveLength) / divisor)
    End Function

    Public Function MultiplyBrightness(ByVal factor As Double) As FunctionLightSpectrum Implements ILight(Of FunctionLightSpectrum).MultiplyBrightness
        Return New FunctionLightSpectrum(IntensityFunction:=Function(waveLength) Me.GetIntensity(waveLength) * factor)
    End Function

    Public Function ToColor() As System.Drawing.Color Implements ILight(Of FunctionLightSpectrum).ToColor
        Throw New NotImplementedException
    End Function

    Public Function GetIntensity(ByVal wavelength As Double) As Double Implements ILightSpectrum(Of FunctionLightSpectrum).GetIntensity
        Return _IntensityFunction.Invoke(wavelength)
    End Function

End Class

Public Delegate Function IntensityFunction(ByVal wavelength As Double) As Double