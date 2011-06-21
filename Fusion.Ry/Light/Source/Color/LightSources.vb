Public Class ColorLightSources
    Inherits List(Of ILightSource(Of ExactColor))
    Implements ILightSource(Of ExactColor)

    Public Sub New()
        MyBase.New()
    End Sub

    Public Function LightColor(ByVal surfacePoint As SurfacePoint) As ExactColor Implements ILightSource(Of ExactColor).GetLight
        Dim colorSum = ExactColor.Black

        For Each lightSource In Me
            colorSum += lightSource.GetLight(surfacePoint)
        Next

        Return colorSum
    End Function
End Class
