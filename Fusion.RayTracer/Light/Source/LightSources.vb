Public Class LightSources(Of TLight As {ILight(Of TLight), New})
    Inherits List(Of ILightSource(Of TLight))
    Implements ILightSource(Of TLight)

    Public Sub New()
        MyBase.New()
    End Sub

    Public Function GetLight(ByVal surfacePoint As SurfacePoint) As TLight Implements ILightSource(Of TLight).GetLight
        Dim colorSum = New TLight

        For Each lightSource In Me
            colorSum = colorSum.Add(lightSource.GetLight(surfacePoint))
        Next

        Return colorSum
    End Function
End Class
