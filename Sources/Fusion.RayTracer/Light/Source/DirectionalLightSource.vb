Public Class DirectionalLightSource(Of TLight As {ILight(Of TLight), New})
    Implements ILightSource(Of TLight)
    
    Public Sub New(direction As Vector3D, light As TLight)
        _NormalizedDirection = direction.Normalized
        _Light = light
    End Sub

    Private ReadOnly _NormalizedDirection As Vector3D
    Private ReadOnly _Light As TLight

    Public Function GetLight(surfacePoint As SurfacePoint(Of TLight)) As TLight Implements ILightSource(Of TLight).GetLight
        Dim valueFactor = -surfacePoint.NormalizedNormal * _NormalizedDirection

        If valueFactor < 0 Then Return New TLight

        Return _Light.MultiplyBrightness(valueFactor)
    End Function
End Class
