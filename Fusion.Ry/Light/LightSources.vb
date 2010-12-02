Public Class LightSources
    Inherits List(Of ILightSource)
    Implements ILightSource

    Public Sub New()
        MyBase.New()
    End Sub

    Public Function LightColor(ByVal surfacePoint As SurfacePoint) As ExactColor Implements ILightSource.LightColor
        Dim colorSum = ExactColor.Black

        For Each lightSource In Me
            colorSum += lightSource.LightColor(surfacePoint)
        Next

        Return colorSum
    End Function
End Class
