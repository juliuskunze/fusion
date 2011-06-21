Public Class UndirectionalLightSource(Of TLight As {ILight(Of TLight), New})
    Implements ILightSource(Of TLight)

    Public Sub New(ByVal light As TLight)
        Me.Color = light
    End Sub

    Public Property Color As TLight

    Public Function LightColor(ByVal surfacePoint As SurfacePoint) As TLight Implements ILightSource(Of TLight).GetLight
        Return Me.Color
    End Function

End Class