''' <summary>
''' Transforms a view ray of a (stationary) reference frame S into a reference frame T which relativly moves with a constant velocity in x-direction.
''' When the origins of the reference frames are equal, their times are 0.
''' </summary>
''' <remarks></remarks>
Public Class RelativisticRadianceTransformation

    Public Sub New(ByVal relativeVelocityOfTInS As Vector3D)
        If relativeVelocityOfTInS.Length >= Physics.Constants.SpeedOfLight Then Throw New ArgumentException("A velocity must be smaller than light velocity.")

        _RelativeVelocity = relativeVelocityOfTInS
        _NormalizedRelativeVelocityDirection = _RelativeVelocity.Normalized
        _Beta = _RelativeVelocity.Length / Physics.Constants.SpeedOfLight
        _Gamma = 1 / (1 - _Beta ^ 2)
        _RelativeVelocityIsNull = (_RelativeVelocity.LengthSquared = 0)
    End Sub

    Private ReadOnly _RelativeVelocity As Vector3D
    Public ReadOnly Property RelativeVelocity As Vector3D
        Get
            Return _RelativeVelocity
        End Get
    End Property

    Private ReadOnly _NormalizedRelativeVelocityDirection As Vector3D
    Public ReadOnly Property NormalizedRelativeVelocityDirection As Vector3D
        Get
            Return _NormalizedRelativeVelocityDirection
        End Get
    End Property

    Private ReadOnly _Beta As Double
    Private ReadOnly Property Beta As Double
        Get
            Return _Beta
        End Get
    End Property

    Private ReadOnly _Gamma As Double
    Private ReadOnly Property Gamma As Double
        Get
            Return _Gamma
        End Get
    End Property

    Private ReadOnly _RelativeVelocityIsNull As Boolean

    Private Function GetGammaTheta(ByVal viewRayInS As Ray) As Double
        If _RelativeVelocityIsNull Then Return 1

        Return Me.GetGammaTheta(cosinusTheta:=-viewRayInS.NormalizedDirection * _NormalizedRelativeVelocityDirection)
    End Function

    Private Function GetGammaTheta(ByVal cosinusTheta As Double) As Double
        Return 1 / (_Gamma * (1 + _Beta * cosinusTheta))
    End Function

    Public Function GetViewRayInS(ByVal viewRayInT As Ray) As Ray
        If _RelativeVelocityIsNull Then Return viewRayInT

        Dim oldDirection = viewRayInT.NormalizedDirection
        Dim oldCosinus = oldDirection * _NormalizedRelativeVelocityDirection
        Dim oldCosinusVector = oldCosinus * _NormalizedRelativeVelocityDirection
        Dim oldSinusVector = oldDirection - oldCosinusVector

        'cos theta= (cos theta' - beta) / (1 - beta * cos theta')
        Dim newCosinus = (oldCosinus - _Beta) / (1 - _Beta * oldCosinus)
        Dim newCosinusVector = newCosinus * _NormalizedRelativeVelocityDirection

        'cos phi= cos phi'
        Dim newSinusVector = oldSinusVector

        Dim newDirection = newSinusVector + newCosinusVector
        Return New Ray(origin:=viewRayInT.Origin, direction:=newDirection)
    End Function

    Public Function GetWavelengthInS(ByVal viewRayInS As Ray, ByVal wavelengthInT As Double) As Double
        'lambda = lambda' * gamma_phi
        Return wavelengthInT * Me.GetGammaTheta(viewRayInS:=viewRayInS)
    End Function

    Public Function GetSpectralRadianceInT(ByVal viewRayInS As Ray, ByVal spectralRadianceInS As Double) As Double
        'L'(...') = L(...) * gamma_phi ^ 5
        Return spectralRadianceInS * Me.GetGammaTheta(viewRayInS:=viewRayInS) ^ 5
    End Function

    Public Function GetSpectralRadianceFunctionInT(ByVal viewRayInS As Ray, ByVal spectralRadianceFunctionInS As SpectralRadianceFunction) As SpectralRadianceFunction
        Return Function(wavelengthInT) Me.GetSpectralRadianceInT(viewRayInS:=viewRayInS, spectralRadianceInS:=spectralRadianceFunctionInS(Me.GetWavelengthInS(viewRayInS:=viewRayInS, wavelengthInT:=wavelengthInT)))
        'Me.GetTransformedSpectralRadiance(ray, spectralRadianceFunction(Me.GetTransformedWavelength(ray, wavelength)))
    End Function

    Public Function GetRadianceSpectrumInT(ByVal viewRayInS As Ray, ByVal radianceSpectrumInS As RadianceSpectrum) As RadianceSpectrum
        Return New RadianceSpectrum(Me.GetSpectralRadianceFunctionInT(viewRayInS:=viewRayInS, spectralRadianceFunctionInS:=radianceSpectrumInS.Function))
    End Function

End Class
