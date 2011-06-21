Public NotInheritable Class ColorMaterials2D
    Private Sub New()
    End Sub

    Public Shared Function Black() As Material2D(Of ExactColor)
        Return ColorMaterials2D.LightSource(sourceLight:=ExactColor.Black)
    End Function

    Public Shared Function LightSource(ByVal sourceLight As ExactColor) As Material2D(Of ExactColor)
        Return New Material2D(Of ExactColor)(sourceLight:=sourceLight,
                              scatteringRemission:=New BlackRemission(Of ExactColor),
                              reflectionRemission:=New BlackRemission(Of ExactColor),
                              transparencyRemission:=New BlackRemission(Of ExactColor))
    End Function

    Public Shared Function Scattering(ByVal color As ExactColor) As Material2D(Of ExactColor)
        Return New Material2D(Of ExactColor)(sourceLight:=ExactColor.Black,
                              scatteringRemission:=New ComponentScaledColorRemission(color),
                              reflectionRemission:=New BlackRemission(Of ExactColor),
                              transparencyRemission:=New BlackRemission(Of ExactColor))
    End Function

    Public Shared Function Transparent(ByVal refractionIndexQuotient As Double, ByVal reflectionAlbedo As Double) As Material2D(Of ExactColor)
        Return ColorMaterials2D.Transparent(scatteringRemission:=New BlackRemission(Of ExactColor),
                                            reflectionRemission:=New ScaledRemission(Of ExactColor)(reflectionAlbedo),
                                            refractionIndexQuotient:=refractionIndexQuotient)
    End Function

    Public Shared Function Transparent(ByVal scatteringRemission As IRemission(Of ExactColor),
                                       ByVal reflectionRemission As IRemission(Of ExactColor),
                                       ByVal refractionIndexQuotient As Double) As Material2D(Of ExactColor)

        Return New Material2D(Of ExactColor)(sourceLight:=ExactColor.Black,
                              scatteringRemission:=scatteringRemission,
                              reflectionRemission:=reflectionRemission,
                              transparencyRemission:=New FullRemission(Of ExactColor),
                              refractionIndexQuotient:=refractionIndexQuotient)
    End Function

    Public Shared Function TransparentInner(ByVal refractionIndexQuotient As Double) As Material2D(Of ExactColor)
        Return ColorMaterials2D.Transparent(scatteringRemission:=New BlackRemission(Of ExactColor),
                                            reflectionRemission:=New BlackRemission(Of ExactColor),
                                            refractionIndexQuotient:=1 / refractionIndexQuotient)
    End Function

    Public Shared Function Reflecting(Optional ByVal albedo As Double = 1) As Material2D(Of ExactColor)
        Return New Material2D(Of ExactColor)(sourceLight:=ExactColor.Black,
                              scatteringRemission:=New BlackRemission(Of ExactColor),
                              reflectionRemission:=New ScaledRemission(Of ExactColor)(albedo:=albedo),
                              transparencyRemission:=New BlackRemission(Of ExactColor))
    End Function

    Public Shared Function Scattering(Optional ByVal albedo As Double = 1) As Material2D(Of ExactColor)
        Return New Material2D(Of ExactColor)(sourceLight:=ExactColor.Black,
                              scatteringRemission:=New ScaledRemission(Of ExactColor)(albedo:=albedo),
                              reflectionRemission:=New BlackRemission(Of ExactColor),
                              transparencyRemission:=New BlackRemission(Of ExactColor))
    End Function

End Class
