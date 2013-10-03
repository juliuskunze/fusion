Public Class LightSources(Of TLight As {ILight(Of TLight), New})
    Inherits List(Of ILightSource(Of TLight))
    Implements ILightSource(Of TLight)
    
    Public Sub New()
    End Sub

    Public Sub New(lightSources As IEnumerable(Of ILightSource(Of TLight)))
        MyBase.New(lightSources)
    End Sub

    Public Function GetLight(surfacePoint As SurfacePoint(Of TLight)) As TLight Implements ILightSource(Of TLight).GetLight
        Dim colorSum = New TLight

        For Each lightSource In Me
            colorSum = colorSum.Add(lightSource.GetLight(surfacePoint))
        Next

        Return colorSum
    End Function
End Class
