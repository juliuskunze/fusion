Public Class ShadedLightSources(Of TLight As {ILight(Of TLight), New})
    Inherits List(Of IPointLightSource(Of TLight))
    Implements ILightSource(Of TLight)

    Public Property ShadowingSurface As ISurface

    Public Property SafetyDistanceSquared As Double = 1.0E-19

    Public Sub New(shadowingSurface As ISurface)
        Me.ShadowingSurface = shadowingSurface
    End Sub

    Public Sub New(pointLightSources As IEnumerable(Of IPointLightSource(Of TLight)), shadowingSurface As ISurface)
        MyBase.New(pointLightSources)

        Me.ShadowingSurface = shadowingSurface
    End Sub

    Public Function GetLight(surfacePoint As SurfacePoint) As TLight Implements ILightSource(Of TLight).GetLight
        Dim returnColor = New TLight
        For Each pointLightSource In Me
            Dim lightRay = New Ray(origin:=pointLightSource.Location,
                                   direction:=surfacePoint.Location - pointLightSource.Location)

            Dim firstIntersection = ShadowingSurface.FirstIntersection(lightRay)

            If firstIntersection Is Nothing Then Continue For
            If (firstIntersection.Location - surfacePoint.Location).LengthSquared >= SafetyDistanceSquared Then Continue For

            returnColor = returnColor.Add(pointLightSource.GetLight(surfacePoint))
        Next

        Return returnColor
    End Function
End Class
