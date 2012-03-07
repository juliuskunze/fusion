Public Class PointLightSource(Of TLight As {ILight(Of TLight), New})
    Implements IPointLightSource(Of TLight)

    Public Property Location As Vector3D Implements IPointLightSource(Of TLight).Location
    Public Property BaseLight As TLight

    Public Sub New(location As Vector3D, baseLight As TLight)
        Me.Location = location
        Me.BaseLight = baseLight
    End Sub

    Public Function GetLight(surfacePoint As SurfacePoint) As TLight Implements ILightSource(Of TLight).GetLight
        Dim pointToLight = Location - surfacePoint.Location
        Dim normalizedPointToLight = pointToLight.Normalized
        Dim distanceSquared = pointToLight.LengthSquared
        Dim brightnessFactorByDistance = 1 / distanceSquared
        Dim brightnessFactorByNormal = surfacePoint.NormalizedNormal.DotProduct(normalizedPointToLight)
        If brightnessFactorByNormal <= 0 Then Return New TLight

        Return BaseLight.MultiplyBrightness(brightnessFactorByDistance * brightnessFactorByNormal)
    End Function

    Public Function GetLightAtPoint(point As Vector3D) As TLight
        Return BaseLight.MultiplyBrightness(1 / (Location - point).LengthSquared)
    End Function


End Class
