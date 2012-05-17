''' <summary>
''' A PointLightSource where brightness does not depend on the distance. 
''' </summary>
Public Class ConstantPointLightSource(Of TLight As {ILight(Of TLight), New})
    Implements IPointLightSource(Of TLight)
    
    Private ReadOnly _Location As Vector3D

    Public ReadOnly Property Location As Vector3D Implements IPointLightSource(Of TLight).Location
        Get
            Return _Location
        End Get
    End Property
    Public Property Light As TLight

    Public Sub New(location As Vector3D, light As TLight)
        _Location = location
        Me.Light = light
    End Sub

    Public Function GetLight(surfacePoint As SurfacePoint) As TLight Implements ILightSource(Of TLight).GetLight
        Dim pointToLight = Location - surfacePoint.Location
        Dim normalizedPointToLight = pointToLight.Normalized
        Dim brightnessFactorByNormal = surfacePoint.NormalizedNormal.DotProduct(normalizedPointToLight)
        If brightnessFactorByNormal <= 0 Then Return New TLight

        Return Light.MultiplyBrightness(brightnessFactorByNormal)
    End Function

    Public Function GetLight(point As Vector3D) As TLight Implements IPointLightSource(Of TLight).GetLight
        Return Light
    End Function
End Class