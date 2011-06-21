Public Class ShadedLightSources
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

    Public Function LightColor(ByVal surfacePoint As SurfacePoint) As ExactColor Implements ILightSource(Of ExactColor).GetLight
        Dim returnColor = ExactColor.Black
        For Each pointLightSource In Me
            Dim lightRay = New Ray(origin:=pointLightSource.Location,
                                   direction:=surfacePoint.Location - pointLightSource.Location)

            Dim firstIntersection = Me.ShadowingSurface.FirstIntersection(lightRay)
            If firstIntersection IsNot Nothing Then
                Dim notShadowing = firstIntersection IsNot Nothing AndAlso
                    (firstIntersection.Location - surfacePoint.Location).LengthSquared < SafetyDistanceSquared

                If notShadowing Then
                    returnColor += pointLightSource.GetLight(surfacePoint)
                End If
            End If
        Next

        Return returnColor
    End Function
End Class
