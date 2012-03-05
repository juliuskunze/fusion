''' <summary>
''' Transforms events, velocities and view rays of a (stationary) reference frame S into a reference frame T which relativly moves with a constant velocity.
''' The events (0, 0, 0, 0) of both reference frames are the same.
''' </summary>
Public Class LorentzTransformation
    Private ReadOnly _RelativeVelocity As Vector3D
    Private ReadOnly _NormalizedRelativeVelocity As Vector3D
    Private ReadOnly _Beta As Double
    Private ReadOnly _Gamma As Double

    ''' <param name="relativeVelocity">The relative velocity of T in S.</param>
    Public Sub New(relativeVelocity As Vector3D)
        If relativeVelocity.Length >= SpeedOfLight Then Throw New ArgumentException("A velocity must be smaller than light velocity.")

        _RelativeVelocity = relativeVelocity
        _NormalizedRelativeVelocity = _RelativeVelocity.Normalized
        _Beta = _RelativeVelocity.Length / SpeedOfLight
        _Gamma = 1 / Sqrt(1 - _Beta ^ 2)
        _RelativeVelocityIsNull = (_RelativeVelocity.LengthSquared = 0)
    End Sub

    Public ReadOnly Property RelativeVelocity As Vector3D
        Get
            Return _RelativeVelocity
        End Get
    End Property

    Public ReadOnly Property NormalizedRelativeVelocity As Vector3D
        Get
            Return _NormalizedRelativeVelocity
        End Get
    End Property

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

    Private Function GetGammaTheta(normalizedSightRayDirectionInS As Vector3D) As Double
        If _RelativeVelocityIsNull Then Return 1

        Return GetGammaTheta(cosinusTheta:=-normalizedSightRayDirectionInS * _NormalizedRelativeVelocity)
    End Function

    Private Function GetGammaTheta(cosinusTheta As Double) As Double
        Return 1 / (_Gamma * (1 + _Beta * cosinusTheta))
    End Function

    Public Function InverseSemiTransformSightRay(sightRayInTWithOriginInS As Ray) As Ray
        If _RelativeVelocityIsNull Then Return sightRayInTWithOriginInS

        Return New Ray(origin:=sightRayInTWithOriginInS.Origin,
                       direction:=Inverse.TransformSightRayDirection(sightRayInTWithOriginInS.NormalizedDirection))
    End Function

    Private Function TransformSightRayDirection(sightRayDirection As Vector3D) As Vector3D
        Return -TransformVelocity(-sightRayDirection.Normalized.ScaledToLength(SpeedOfLight))
    End Function

    ''' <param name="wavelength">A wavelength in S.</param>
    ''' <returns>The corresponding wavelength in T.</returns>
    Public Function TransformWavelength(normalizedSightRayDirectionInS As Vector3D, wavelength As Double) As Double
        'lambda' = lambda / gamma_theta
        Return wavelength / GetGammaTheta(normalizedSightRayDirectionInS:=normalizedSightRayDirectionInS)
    End Function

    ''' <param name="spectralRadiance">A spectral radiance in S.</param>
    ''' <returns>The corresponding spectral radiance in T.</returns>
    Public Function TransformSpectralRadiance(normalizedSightRayDirectionInS As Vector3D, spectralRadiance As Double) As Double
        'L'(...') = L(...) * gamma_theta ^ 5
        Return spectralRadiance * GetGammaTheta(normalizedSightRayDirectionInS:=normalizedSightRayDirectionInS) ^ 5
    End Function

    ''' <param name="spectralRadianceFunction">A spectral radiance function in S.</param>
    ''' <returns>The corresponding spectral radiance function in T.</returns>
    Public Function TransformSpectralRadianceFunction(normalizedSightRayDirectionInS As Vector3D, spectralRadianceFunction As SpectralRadianceFunction) As SpectralRadianceFunction
        Return Function(wavelengthInT) TransformSpectralRadiance(normalizedSightRayDirectionInS:=normalizedSightRayDirectionInS, spectralRadiance:=spectralRadianceFunction(Inverse.TransformWavelength(normalizedSightRayDirectionInS:=normalizedSightRayDirectionInS, wavelength:=wavelengthInT)))
    End Function

    ''' <param name="radianceSpectrum">A spectral radiance spectrum in S.</param>
    ''' <returns>The corresponding spectral radiance spectrum in T.</returns>
    Public Function TransformRadianceSpectrum(normalizedSightRayDirectionInS As Vector3D, radianceSpectrum As RadianceSpectrum) As RadianceSpectrum
        Return New RadianceSpectrum(TransformSpectralRadianceFunction(normalizedSightRayDirectionInS:=normalizedSightRayDirectionInS, spectralRadianceFunction:=radianceSpectrum.Function))
    End Function

    ''' <param name="event">An event in S.</param>
    ''' <returns>The corresponding event in T.</returns>
    Public Function TransformEvent([event] As SpaceTimeEvent) As SpaceTimeEvent
        If _RelativeVelocityIsNull Then Return [event]

        Return New SpaceTimeEvent(time:=_Gamma * ([event].Time - (_RelativeVelocity.Length * (_NormalizedRelativeVelocity * [event].Location)) / SpeedOfLight ^ 2),
                                  location:=[event].Location + (_Gamma - 1) * (_NormalizedRelativeVelocity * [event].Location) * _NormalizedRelativeVelocity - _Gamma * [event].Time * _RelativeVelocity)
    End Function

    Public Function Inverse() As LorentzTransformation
        Static state As LorentzTransformation

        If state Is Nothing Then state = New LorentzTransformation(-_RelativeVelocity)

        Return state
    End Function

    ''' <param name="velocity">A velocity in S.</param>
    ''' <returns>The corresponding velocity in T.</returns>
    Public Function TransformVelocity(velocity As Vector3D) As Vector3D
        If _RelativeVelocityIsNull Then Return velocity

        Dim ux = velocity.OrthogonalProjectionOn(_NormalizedRelativeVelocity)

        Return 1 / (1 - _RelativeVelocity * velocity / SpeedOfLight ^ 2) *
            ((velocity - ux) / _Gamma + _NormalizedRelativeVelocity * (ux * _NormalizedRelativeVelocity - _RelativeVelocity.Length))
    End Function

    Public Function Before(second As LorentzTransformation) As LorentzTransformation
        Return New LorentzTransformation(RelativeVelocity:=Inverse.TransformVelocity(second.RelativeVelocity))
    End Function

    Public Function TransformSightRay(sightRay As SightRay) As SightRay
        Return New SightRay(originEvent:=TransformEvent(sightRay.OriginEvent), direction:=TransformSightRayDirection(sightRay.Ray.NormalizedDirection))
    End Function
End Class
