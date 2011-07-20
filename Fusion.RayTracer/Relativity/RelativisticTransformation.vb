''' <summary>
''' Transforms a (light) ray of a (stationary) reference frame into one which relativly moves
''' with a constant velocity in x-direction.
''' When the origins of the reference frames are equal, their times are 0.
''' </summary>
''' <remarks></remarks>
Public Class RelativisticRadianceTransformation

    Public Sub New(ByVal relativeVelocity As Vector3D)
        If relativeVelocity.Length >= Physics.Constants.SpeedOfLight Then Throw New ArgumentException("A velocity must be smaller than light velocity.")

        _RelativeVelocity = relativeVelocity
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

    Private Function GetGammaTheta(ByVal ray As Ray) As Double
        If _RelativeVelocityIsNull Then Return 1

        Return Me.GetGammaTheta(cosinusTheta:=ray.NormalizedDirection * _NormalizedRelativeVelocityDirection)
    End Function

    Private Function GetGammaTheta(ByVal cosinusTheta As Double) As Double
        Return 1 / (_Gamma * (1 - _Beta * cosinusTheta))
    End Function

    ''' <summary>
    ''' Returns the transformed ray in the moved reference frame.
    ''' </summary>
    ''' <param name="ray">The ray in the stationary reference frame.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetTransformedRay(ByVal ray As Ray) As Ray
        If _RelativeVelocityIsNull Then Return ray

        Dim oldDirection = ray.NormalizedDirection
        Dim oldCosinus = oldDirection * _NormalizedRelativeVelocityDirection
        Dim oldCosinusVector = oldCosinus * _NormalizedRelativeVelocityDirection
        Dim oldSinusVector = oldDirection - oldCosinusVector

        'cos theta'= (cos theta - beta) / (1-beta * cos theta)
        Dim newCosinus = (oldCosinus - _Beta) / (1 - _Beta * oldCosinus)
        Dim newCosinusVector = newCosinus * _NormalizedRelativeVelocityDirection

        'cos phi'= cos phi
        Dim newSinusVector = oldSinusVector

        Dim newDirection = newSinusVector + newCosinusVector
        Return New Ray(origin:=ray.Origin, direction:=newDirection)
    End Function

    Public Function GetTransformedWavelength(ByVal ray As Ray, ByVal wavelength As Double) As Double
        Return wavelength * Me.GetGammaTheta(ray)
    End Function

    Public Function GetTransformedSpectralRadianceFunction(ByVal ray As Ray, ByVal spectralRadianceFunction As SpectralRadianceFunction) As SpectralRadianceFunction
        Return Function(wavelength) Me.GetTransformedSpectralRadiance(ray:=ray,
                                                                      intensity:=spectralRadianceFunction.Invoke(wavelength:=Me.GetTransformedWavelength(ray:=ray,
                                                                                                                                                         wavelength:=wavelength)))
    End Function

    Private Function GetTransformedRadiance(ByVal ray As Ray, ByVal intensity As Double) As Double
        Return intensity * Me.GetGammaTheta(ray) ^ 4
    End Function

    Public Function GetTransformedSpectralRadiance(ByVal ray As Ray, ByVal intensity As Double) As Double
        Return intensity * Me.GetGammaTheta(ray) ^ 5
    End Function

End Class
