''' <summary>
''' Transforms events, velocities and view rays of a (stationary) reference frame S into a reference frame T which relativly moves with a constant velocity.
''' The events (0, 0, 0, 0) of both reference frames are the same.
''' </summary>
Public Class LorentzTransformation
    ''' <param name="relativeVelocity">The relative velocity of T in S.</param>
    Public Sub New(relativeVelocity As Vector3D)
        If relativeVelocity.Length >= SpeedOfLight Then Throw New ArgumentException("A velocity must be smaller than light velocity.")

        _RelativeVelocity = relativeVelocity
        _NormalizedRelativeVelocity = _RelativeVelocity.Normalized
        _Beta = _RelativeVelocity.Length / SpeedOfLight
        _Gamma = 1 / Sqrt(1 - _Beta ^ 2)
        _RelativeVelocityIsNull = (_RelativeVelocity.LengthSquared = 0)
    End Sub

    Private ReadOnly _RelativeVelocity As Vector3D
    Public ReadOnly Property RelativeVelocity As Vector3D
        Get
            Return _RelativeVelocity
        End Get
    End Property

    Private ReadOnly _NormalizedRelativeVelocity As Vector3D
    Public ReadOnly Property NormalizedRelativeVelocity As Vector3D
        Get
            Return _NormalizedRelativeVelocity
        End Get
    End Property

    Private ReadOnly _Beta As Double
    Private ReadOnly _Gamma As Double

    Public ReadOnly Property Beta() As Double
        Get
            Return _Beta
        End Get
    End Property

    Public ReadOnly Property Gamma() As Double
        Get
            Return _Gamma
        End Get
    End Property

    Private ReadOnly _RelativeVelocityIsNull As Boolean

    Private Function GetGammaTheta(viewRayInS As Ray) As Double
        If _RelativeVelocityIsNull Then Return 1

        Return GetGammaTheta(cosinusTheta:=-viewRayInS.NormalizedDirection * _NormalizedRelativeVelocity)
    End Function

    Private Function GetGammaTheta(cosinusTheta As Double) As Double
        Return 1 / (_Gamma * (1 + _Beta * cosinusTheta))
    End Function

    Public Function InverseSemiTransformViewRay(viewRayInTWithOriginInS As Ray) As Ray
        If _RelativeVelocityIsNull Then Return viewRayInTWithOriginInS

        Return New Ray(origin:=viewRayInTWithOriginInS.Origin, direction:=InverseTransformViewRayDirection(viewRayInTWithOriginInS.NormalizedDirection))
    End Function

    Public Function InverseTransformViewRayDirection(direction As Vector3D) As Vector3D
        Dim oldDirection = direction
        Dim oldCosinus = oldDirection * _NormalizedRelativeVelocity
        Dim oldCosinusVector = oldCosinus * _NormalizedRelativeVelocity
        Dim oldSinusVector = oldDirection - oldCosinusVector

        'cos theta = (cos theta' - beta) / (1 - beta * cos theta')
        Dim newCosinus = (oldCosinus - _Beta) / (1 - _Beta * oldCosinus)
        Dim newCosinusVector = newCosinus * _NormalizedRelativeVelocity

        'cos phi = cos phi'
        Dim newSinusVector = oldSinusVector

        Return newSinusVector + newCosinusVector
    End Function

    Public Function GetWavelengthInS(viewRayInS As Ray, wavelengthInT As Double) As Double
        'lambda = lambda' * gamma_phi
        Return wavelengthInT * GetGammaTheta(viewRayInS:=viewRayInS)
    End Function

    Public Function GetSpectralRadianceInT(viewRayInS As Ray, spectralRadianceInS As Double) As Double
        'L'(...') = L(...) * gamma_phi ^ 5
        Return spectralRadianceInS * GetGammaTheta(viewRayInS:=viewRayInS) ^ 5
    End Function

    Public Function GetSpectralRadianceFunctionInT(viewRayInS As Ray, spectralRadianceFunctionInS As SpectralRadianceFunction) As SpectralRadianceFunction
        Return Function(wavelengthInT) GetSpectralRadianceInT(viewRayInS:=viewRayInS, spectralRadianceInS:=spectralRadianceFunctionInS(GetWavelengthInS(viewRayInS:=viewRayInS, wavelengthInT:=wavelengthInT)))
        'Me.GetTransformedSpectralRadiance(ray, spectralRadianceFunction(Me.GetTransformedWavelength(ray, wavelength)))
    End Function

    Public Function TransformRadianceSpectrum(viewRayInS As Ray, radianceSpectrumInS As RadianceSpectrum) As RadianceSpectrum
        Return New RadianceSpectrum(GetSpectralRadianceFunctionInT(viewRayInS:=viewRayInS, spectralRadianceFunctionInS:=radianceSpectrumInS.Function))
    End Function

    Public Function TransformEvent(eventInS As SpaceTimeEvent) As SpaceTimeEvent
        If _RelativeVelocityIsNull Then Return eventInS

        Return New SpaceTimeEvent(time:=_Gamma * (eventInS.Time - (_RelativeVelocity.Length * (_NormalizedRelativeVelocity * eventInS.Location)) / SpeedOfLight ^ 2),
                                  location:=eventInS.Location + (_Gamma - 1) * (_NormalizedRelativeVelocity * eventInS.Location) * _NormalizedRelativeVelocity - _Gamma * eventInS.Time * _RelativeVelocity)
    End Function

    Public Function Inverse() As LorentzTransformation
        Return New LorentzTransformation(-_RelativeVelocity)
    End Function

    Public Function TransformVelocity(velocity As Vector3D) As Vector3D
        If _RelativeVelocityIsNull Then Return velocity

        Dim ux = velocity.OrthogonalProjectionOn(_NormalizedRelativeVelocity)

        Return 1 / (1 - _RelativeVelocity * velocity / SpeedOfLight ^ 2) *
            ((velocity - ux) / _Gamma + _NormalizedRelativeVelocity * (ux * _NormalizedRelativeVelocity - _RelativeVelocity.Length))
    End Function

    Public Function Before(second As LorentzTransformation) As LorentzTransformation
        Return New LorentzTransformation(RelativeVelocity:=TransformVelocity(second.RelativeVelocity))
    End Function

End Class
