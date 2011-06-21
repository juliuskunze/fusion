''' <summary>
''' Transforms a (light) ray of a (stationary) reference frame into one which relativly moves
''' with a constant velocity in x-direction.
''' </summary>
''' The origins of the reference frames are equal, their times are equal.
''' <remarks></remarks>
Public Class RelativisticRayTransformation

    Public Sub New(ByVal relativeXVelocityInC As Double)
        If Abs(relativeXVelocityInC) > 1 Then Throw New ArgumentException("A velocity can not be greater than the light velocity.")

        Me.RelativeXVelocityInC = relativeXVelocityInC
        _Gamma = 1 / (1 - Me.Beta ^ 2)
    End Sub

    Public Property RelativeXVelocityInC As Double

    Private ReadOnly Property Beta As Double
        Get
            Return Me.RelativeXVelocityInC
        End Get
    End Property

    Private _Gamma As Double
    Private ReadOnly Property Gamma As Double
        Get
            Return _Gamma
        End Get
    End Property

    ''' <summary>
    ''' The ray in the moved reference frame.
    ''' </summary>
    ''' <param name="ray">The ray in the stationary reference frame.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetTransformedRay(ByVal ray As Ray) As Ray
        Dim directionInStationaryFrame = ray.NormalizedDirection
        Dim directionInMovedFrame = New Vector3D(x:=directionInStationaryFrame.X,
                                                 y:=directionInStationaryFrame.Y,
                                                 z:=Me.Gamma * (directionInStationaryFrame.Z - Me.Beta * directionInStationaryFrame.Length))
        Return New Ray(origin:=Ray.Origin, direction:=directionInMovedFrame)
    End Function

    Public Function GetTransformedWavelength(ByVal ray As Ray, ByVal wavelength As Double) As Double
        Return wavelength * GetTransformedWavelengthFactor(ray)
    End Function

    Private Function GetTransformedWavelengthFactor(ByVal ray As Ray) As Double
        Dim direction = ray.NormalizedDirection
        Return Sqrt((Me.Gamma * (direction.Z - Me.Beta * direction.Length)) ^ 2 +
                                 direction.X ^ 2 +
                                 direction.Y ^ 2) /
                     direction.Length
    End Function

    Public Function GetTransformedIntensityFunction(ByVal ray As Ray, ByVal intensityFunction As IntensityFunction) As IntensityFunction
        Return Function(wavelength) intensityFunction.Invoke(Me.GetTransformedWavelength(ray:=ray, wavelength:=wavelength))
    End Function

    Public Function GetTransformedIntensity(ByVal ray As Ray, ByVal intensity As Double) As Double
        Dim direction = ray.NormalizedDirection
        Return intensity * direction.LengthSquared / (Me.Gamma * (direction.Z - Me.Beta * direction.Length) ^ 2 + (direction.Y ^ 2 + direction.X ^ 2) / Me.Gamma)
    End Function

End Class
