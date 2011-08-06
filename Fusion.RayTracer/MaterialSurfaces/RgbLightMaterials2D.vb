Public NotInheritable Class RgbLightMaterials2D
    Inherits Materials2D(Of RgbLight)

    Private Sub New()
    End Sub

    Public Shared Function Scattering(color As RgbLight) As Material2D(Of RgbLight)
        Return New Material2D(Of RgbLight)(sourceLight:=RgbLight.Black,
                              scatteringRemission:=New RgbLightRemission(color),
                              reflectionRemission:=New BlackRemission(Of RgbLight),
                              transparencyRemission:=New BlackRemission(Of RgbLight))
    End Function

    Public Shared Function Transparent(refractionIndexQuotient As Double, reflectionAlbedo As Double) As Material2D(Of RgbLight)
        Return RgbLightMaterials2D.Transparent(scatteringRemission:=New BlackRemission(Of RgbLight),
                                            reflectionRemission:=New ScaledRemission(Of RgbLight)(reflectionAlbedo),
                                            refractionIndexQuotient:=refractionIndexQuotient)
    End Function

    Public Shared Function Transparent(scatteringRemission As IRemission(Of RgbLight),
                                        reflectionRemission As IRemission(Of RgbLight),
                                        refractionIndexQuotient As Double) As Material2D(Of RgbLight)

        Return New Material2D(Of RgbLight)(sourceLight:=RgbLight.Black,
                              scatteringRemission:=scatteringRemission,
                              reflectionRemission:=reflectionRemission,
                              transparencyRemission:=New FullRemission(Of RgbLight),
                              refractionIndexQuotient:=refractionIndexQuotient)
    End Function

    Public Shared Function TransparentInner(refractionIndexQuotient As Double) As Material2D(Of RgbLight)
        Return RgbLightMaterials2D.Transparent(scatteringRemission:=New BlackRemission(Of RgbLight),
                                            reflectionRemission:=New BlackRemission(Of RgbLight),
                                            refractionIndexQuotient:=1 / refractionIndexQuotient)
    End Function

    Public Shared Function Reflecting(Optional  albedo As Double = 1) As Material2D(Of RgbLight)
        Return New Material2D(Of RgbLight)(sourceLight:=RgbLight.Black,
                              scatteringRemission:=New BlackRemission(Of RgbLight),
                              reflectionRemission:=New ScaledRemission(Of RgbLight)(albedo:=albedo),
                              transparencyRemission:=New BlackRemission(Of RgbLight))
    End Function

    Public Shared Function Scattering(Optional  albedo As Double = 1) As Material2D(Of RgbLight)
        Return New Material2D(Of RgbLight)(sourceLight:=RgbLight.Black,
                              scatteringRemission:=New ScaledRemission(Of RgbLight)(albedo:=albedo),
                              reflectionRemission:=New BlackRemission(Of RgbLight),
                              transparencyRemission:=New BlackRemission(Of RgbLight))
    End Function

End Class
