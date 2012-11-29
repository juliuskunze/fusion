Public Class Materials2D(Of TLight As {ILight(Of TLight), New})

    Public Shared Function Black() As Material2D(Of TLight)
        Return Materials2D(Of TLight).LightSource(sourceLight:=New TLight)
    End Function

    Public Shared Function LightSource(sourceLight As TLight) As Material2D(Of TLight)
        Return New Material2D(Of TLight)(sourceLight:=sourceLight,
                              scatteringRemission:=New BlackRemission(Of TLight),
                              reflectionRemission:=New BlackRemission(Of TLight),
                              transparencyRemission:=New BlackRemission(Of TLight))
    End Function
End Class
