Public NotInheritable Class Materials2D
    Private Sub New()
    End Sub

    Public Shared Function LightSource(ByVal lightColor As ExactColor) As Material2D
        Return New Material2D(lightSourceColor:=lightColor,
                              scatteringRemission:=New BlackColorRemission,
                              reflectionRemission:=New BlackColorRemission,
                              transparencyRemission:=New BlackColorRemission)
    End Function

    Public Shared Function Scattering(ByVal color As ExactColor) As Material2D
        Return New Material2D(lightSourceColor:=ExactColor.Black,
                              scatteringRemission:=New ComponentScaledColorRemission(color),
                              reflectionRemission:=New BlackColorRemission,
                              transparencyRemission:=New BlackColorRemission)
    End Function

    Public Shared Function Glass(ByVal refractionIndexQuotient As Double, ByVal reflectionAlbedo As Double) As Material2D
        Return New Material2D(lightSourceColor:=ExactColor.Black,
                              scatteringRemission:=New BlackColorRemission,
                              reflectionRemission:=New ScaledColorRemission(0.2),
                              transparencyRemission:=New FullColorRemission,
                              refractionIndexQuotient:=refractionIndexQuotient)
    End Function

    Public Shared Function InnerGlass(ByVal refractionIndexQuotient As Double) As Material2D
        Return New Material2D(lightSourceColor:=ExactColor.Black,
                              scatteringRemission:=New BlackColorRemission,
                              reflectionRemission:=New BlackColorRemission,
                              transparencyRemission:=New FullColorRemission,
                              refractionIndexQuotient:=1 / refractionIndexQuotient)
    End Function

    Public Shared Function Mirror() As Material2D
        Return New Material2D(lightSourceColor:=ExactColor.Black,
                              scatteringRemission:=New BlackColorRemission,
                              reflectionRemission:=New FullColorRemission,
                              transparencyRemission:=New BlackColorRemission)
    End Function

End Class
