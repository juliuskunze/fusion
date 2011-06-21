Public Class PointLightSource(Of TLight As {ILight(Of TLight), New})
    Implements IPointLightSource(Of TLight)

    Public Property Location As Vector3D Implements IPointLightSource(Of TLight).Location
    Public Property BaseLight As TLight

    Public Sub New(ByVal location As Vector3D, ByVal baseLight As TLight)
        Me.Location = location
        Me.BaseLight = baseLight
    End Sub

    Public Function LightColor(ByVal surfacePoint As SurfacePoint) As TLight Implements IPointLightSource(Of TLight).GetLight
        Dim relativeVector = surfacePoint.Location - Me.Location
        Dim normalizedRelativeVector = relativeVector.Normalized
        Dim distanceSquared = relativeVector.LengthSquared
        Dim valueFactorByDistance = 1 / distanceSquared
        Dim valueFactorByNormal = -surfacePoint.NormalizedNormal.DotProduct(normalizedRelativeVector)
        If valueFactorByNormal <= 0 Then
            Return New TLight
        End If

        Dim valueFactor = valueFactorByDistance * valueFactorByNormal
        Return Me.BaseLight.MultiplyBrighness(valueFactor)
    End Function

End Class
