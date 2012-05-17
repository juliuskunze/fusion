Public Class LinearPointLightSource(Of TLight As {ILight(Of TLight), New})
    Implements IPointLightSource(Of TLight)
    
    Private ReadOnly _Location As Vector3D

    Public ReadOnly Property Location As Vector3D Implements IPointLightSource(Of TLight).Location
        Get
            Return _Location
        End Get
    End Property
    Public Property BaseLight As TLight

    Public Sub New(location As Vector3D, baseLight As TLight)
        _Location = location
        Me.BaseLight = baseLight
    End Sub

    Public Function GetLight(surfacePoint As SurfacePoint) As TLight Implements ILightSource(Of TLight).GetLight
        Dim normalizedRelativeVector = (surfacePoint.Location - Location).Normalized

        Dim brightnessFactorByNormal = -surfacePoint.NormalizedNormal.DotProduct(normalizedRelativeVector)
        If brightnessFactorByNormal <= 0 Then Return New TLight

        Return GetLight(surfacePoint.Location).MultiplyBrightness(brightnessFactorByNormal)
    End Function

    Public Function GetLight(point As Vector3D) As TLight Implements IPointLightSource(Of TLight).GetLight
        Return BaseLight.MultiplyBrightness(1 / (point - Location).Length)
    End Function
End Class
