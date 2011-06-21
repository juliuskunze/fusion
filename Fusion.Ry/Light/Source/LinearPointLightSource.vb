Public Class LinearPointLightSource(Of TLight As {ILight(Of TLight), New})
    Implements IPointLightSource(Of TLight)

    Public Property Location As Vector3D Implements IPointLightSource(Of TLight).Location
    Public Property BaseLight As TLight

    Public Sub New(ByVal location As Vector3D, ByVal baseLight As TLight)
        Me.Location = location
        Me.BaseLight = baseLight
    End Sub

    Public Function GetLight(ByVal surfacePoint As SurfacePoint) As TLight Implements ILightSource(Of TLight).GetLight
        Dim relativeVector = surfacePoint.Location - Me.Location
        Dim normalizedRelativeVector = relativeVector.Normalized
        Dim distance = relativeVector.Length
        Dim brightnessFactorByDistance = 1 / distance
        Dim brightnessFactorByNormal = -surfacePoint.NormalizedNormal.DotProduct(normalizedRelativeVector)
        If brightnessFactorByNormal <= 0 Then
            Return New TLight
        End If

        Dim valueFactor = brightnessFactorByDistance * brightnessFactorByNormal
        Return Me.BaseLight.MultiplyBrightness(valueFactor)
    End Function
End Class
