Public NotInheritable Class RgbLightMaterials2D
    Inherits Materials2D(Of RgbLight)

    Private Sub New()
    End Sub

    Public Shared Function Scattering(ByVal color As RgbLight) As Material2D(Of RgbLight)
        Return New Material2D(Of RgbLight)(sourceLight:=RgbLight.Black,
                              scatteringRemission:=New RgbLightRemission(color),
                              reflectionRemission:=New BlackRemission(Of RgbLight),
                              transparencyRemission:=New BlackRemission(Of RgbLight))
    End Function

    Public Shared Function Transparent(ByVal refractionIndexQuotient As Double, ByVal reflectionAlbedo As Double) As Material2D(Of RgbLight)
        Return RgbLightMaterials2D.Transparent(scatteringRemission:=New BlackRemission(Of RgbLight),
                                            reflectionRemission:=New ScaledRemission(Of RgbLight)(reflectionAlbedo),
                                            refractionIndexQuotient:=refractionIndexQuotient)
    End Function

    Public Shared Function Transparent(ByVal scatteringRemission As IRemission(Of RgbLight),
                                       ByVal reflectionRemission As IRemission(Of RgbLight),
                                       ByVal refractionIndexQuotient As Double) As Material2D(Of RgbLight)

        Return New Material2D(Of RgbLight)(sourceLight:=RgbLight.Black,
                              scatteringRemission:=scatteringRemission,
                              reflectionRemission:=reflectionRemission,
                              transparencyRemission:=New FullRemission(Of RgbLight),
                              refractionIndexQuotient:=refractionIndexQuotient)
    End Function

    Public Shared Function TransparentInner(ByVal refractionIndexQuotient As Double) As Material2D(Of RgbLight)
        Return RgbLightMaterials2D.Transparent(scatteringRemission:=New BlackRemission(Of RgbLight),
                                            reflectionRemission:=New BlackRemission(Of RgbLight),
                                            refractionIndexQuotient:=1 / refractionIndexQuotient)
    End Function

    Public Shared Function Reflecting(Optional ByVal albedo As Double = 1) As Material2D(Of RgbLight)
        Return New Material2D(Of RgbLight)(sourceLight:=RgbLight.Black,
                              scatteringRemission:=New BlackRemission(Of RgbLight),
                              reflectionRemission:=New ScaledRemission(Of RgbLight)(albedo:=albedo),
                              transparencyRemission:=New BlackRemission(Of RgbLight))
    End Function

    Public Shared Function Scattering(Optional ByVal albedo As Double = 1) As Material2D(Of RgbLight)
        Return New Material2D(Of RgbLight)(sourceLight:=RgbLight.Black,
                              scatteringRemission:=New ScaledRemission(Of RgbLight)(albedo:=albedo),
                              reflectionRemission:=New BlackRemission(Of RgbLight),
                              transparencyRemission:=New BlackRemission(Of RgbLight))
    End Function

End Class
