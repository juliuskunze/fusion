Public Class UndirectionalLightSource(Of TLight As {ILight(Of TLight), New})
    Implements ILightSource(Of TLight)

    Private ReadOnly _Color As TLight

    Public Sub New(light As TLight)
        _Color = light
    End Sub

    Public Function GetLight(surfacePoint As SurfacePoint) As TLight Implements ILightSource(Of TLight).GetLight
        Return _Color
    End Function

End Class