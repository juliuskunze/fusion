Public Class ShadedColorLightSources
    Inherits List(Of IPointLightSource(Of ExactColor))
    Implements ILightSource(Of ExactColor)

    Public Property ShadowingSurface As ISurface

    Public Property SafetyDistanceSquared As Double = 1.0E-19

    Public Sub New(ByVal shadowingSurface As ISurface)
        Me.ShadowingSurface = shadowingSurface
    End Sub

    Public Sub New(ByVal pointLightSources As IEnumerable(Of IPointLightSource(Of ExactColor)), ByVal shadowingSurface As ISurface)
        MyBase.New(pointLightSources)

        Me.ShadowingSurface = shadowingSurface
    End Sub

    Public Function GetLightColor(ByVal surfacePoint As SurfacePoint) As ExactColor Implements ILightSource(Of ExactColor).GetLight
        Dim returnColor = ExactColor.Black
        For Each pointLightSource In Me
            Dim lightRay = New Ray(origin:=pointLightSource.Location,
                                   direction:=surfacePoint.Location - pointLightSource.Location)

            Dim firstIntersection = Me.ShadowingSurface.FirstIntersection(lightRay)

            If firstIntersection Is Nothing Then Continue For
            If (firstIntersection.Location - surfacePoint.Location).LengthSquared >= SafetyDistanceSquared Then Continue For

            returnColor += pointLightSource.GetLight(surfacePoint)
        Next

        Return returnColor
    End Function
End Class
