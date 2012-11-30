Public Class ShadedLightSources(Of TLight As {ILight(Of TLight), New})
    Inherits List(Of IPointLightSource(Of TLight))
    Implements ILightSource(Of TLight)

    Private ReadOnly _ShadowingSurface As ISurface

    Public ReadOnly Property ShadowingSurface As ISurface
        Get
            Return _ShadowingSurface
        End Get
    End Property

    Private Shared ReadOnly _LocationComparer As New Vector3DRoughComparer(maxDeviation:=3.2 * 10 ^ -10)

    Public Sub New(shadowingSurface As ISurface)
        _ShadowingSurface = shadowingSurface
    End Sub

    Public Sub New(pointLightSources As IEnumerable(Of IPointLightSource(Of TLight)), shadowingSurface As ISurface)
        MyBase.New(pointLightSources)

        _ShadowingSurface = shadowingSurface
    End Sub

    Public Function GetLight(surfacePoint As SurfacePoint(Of TLight)) As TLight Implements ILightSource(Of TLight).GetLight
        Return (From pointLightSource In Me
                Let lightRay = New Ray(origin:=pointLightSource.Location, direction:=surfacePoint.Location - pointLightSource.Location)
                Let firstIntersection = ShadowingSurface.FirstIntersection(lightRay)
                Where firstIntersection IsNot Nothing
                Where _LocationComparer.Equals(firstIntersection.Location, surfacePoint.Location)
                Select pointLightSource).Aggregate(New TLight, Function(current, pointLightSource) current.Add(pointLightSource.GetLight(surfacePoint)))
    End Function
End Class
