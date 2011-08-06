Public Class PointLightSource(Of TLight As {ILight(Of TLight), New})
    Implements IPointLightSource(Of TLight)

    Public Property Location As Vector3D Implements IPointLightSource(Of TLight).Location
    Public Property BaseLight As TLight

    Public Sub New(location As Vector3D, baseLight As TLight)
        Me.Location = location
        Me.BaseLight = baseLight
    End Sub

    Public Function GetLight(surfacePoint As SurfacePoint) As TLight Implements ILightSource(Of TLight).GetLight
        Dim relativeVector = surfacePoint.Location - Me.Location
        Dim normalizedRelativeVector = relativeVector.Normalized
        Dim distanceSquared = relativeVector.LengthSquared
        Dim valueFactorByDistance = 1 / distanceSquared
        Dim valueFactorByNormal = -surfacePoint.NormalizedNormal.DotProduct(normalizedRelativeVector)
        If valueFactorByNormal <= 0 Then
            Return New TLight
        End If

        Dim valueFactor = valueFactorByDistance * valueFactorByNormal
        Return Me.BaseLight.MultiplyBrightness(valueFactor)
    End Function

End Class
