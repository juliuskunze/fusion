Public Class PointLightSource(Of TLight As {ILight(Of TLight), New})
    Implements IPointLightSource(Of TLight)
    
    Private ReadOnly _Location As Vector3D
    Private ReadOnly _BrightnessFactorByDistance As Func(Of Double, Double)

    Public ReadOnly Property Location As Vector3D Implements IPointLightSource(Of TLight).Location
        Get
            Return _Location
        End Get
    End Property
    Private ReadOnly _BaseLightByTime As Func(Of Double, TLight)

    Protected Sub New(location As Vector3D, baseLightByTime As Func(Of Double, TLight), brightnessFactorByDistance As Func(Of Double, Double))
        _Location = location
        _BrightnessFactorByDistance = brightnessFactorByDistance
        _BaseLightByTime = baseLightByTime
    End Sub

    Public Overloads Function GetLight(surfacePoint As SurfacePoint(Of TLight)) As TLight Implements IPointLightSource(Of TLight).GetLight
        Dim normalizedPointToLight = (Location - surfacePoint.Location).Normalized
        Dim brightnessFactorByNormal = surfacePoint.NormalizedNormal.DotProduct(normalizedPointToLight)
        If brightnessFactorByNormal <= 0 Then Return New TLight

        Return GetMaximumLight(surfacePoint.SpaceTimeEvent).MultiplyBrightness(brightnessFactorByNormal)
    End Function

    Public Overloads Function GetMaximumLight(spaceTimeEvent As SpaceTimeEvent) As TLight Implements IPointLightSource(Of TLight).GetMaximumLight
        Dim distance = (spaceTimeEvent.Location - Location).Length
        Dim elapsedTimeSpan = distance * SpeedOfLight
        Dim brightnessFactorByDistance = _BrightnessFactorByDistance(distance)
        Dim baseLight = _BaseLightByTime(spaceTimeEvent.Time - elapsedTimeSpan)

        Return baseLight.MultiplyBrightness(brightnessFactorByDistance)
    End Function
End Class