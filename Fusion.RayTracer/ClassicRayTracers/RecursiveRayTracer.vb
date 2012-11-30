Public NotInheritable Class RecursiveRayTracer(Of TLight As {ILight(Of TLight), New})
    Implements IRayTracer(Of TLight)

    Public Sub New(surface As ISurface(Of TLight),
                   unshadedLightSource As ILightSource(Of TLight),
                   shadedPointLightSources As IEnumerable(Of IPointLightSource(Of TLight)),
                   Optional maxIntersectionCount As Integer = 10)
        _Surface = surface
        _LightSource = unshadedLightSource
        _ShadedPointLightSources = New ShadedLightSources(Of TLight)(shadowingSurface:=_Surface, pointLightSources:=shadedPointLightSources)
        _MaxIntersectionCount = maxIntersectionCount
    End Sub

    Private ReadOnly _Surface As ISurface(Of TLight)
    Public ReadOnly Property Surface As ISurface(Of TLight)
        Get
            Return _Surface
        End Get
    End Property

    Private ReadOnly _LightSource As ILightSource(Of TLight)
    Public ReadOnly Property LightSource As ILightSource(Of TLight)
        Get
            Return _LightSource
        End Get
    End Property

    Private ReadOnly _ShadedPointLightSources As ShadedLightSources(Of TLight)
    Public ReadOnly Property ShadedPointLightSources As ShadedLightSources(Of TLight)
        Get
            Return _ShadedPointLightSources
        End Get
    End Property

    Private Function TraceLight(ray As SightRay, intersectionCount As Integer) As TLight
        Dim firstIntersection = Surface.FirstMaterialIntersection(ray)

        If firstIntersection Is Nothing Then Return New TLight

        Dim hitMaterial = firstIntersection.Material

        Dim finalLight = hitMaterial.SourceLight
        If hitMaterial.Scatters Then
            Dim pointLightSourceLight = _ShadedPointLightSources.GetLight(firstIntersection)
            Dim lightColor = LightSource.GetLight(firstIntersection).Add(pointLightSourceLight)
            finalLight = finalLight.Add(hitMaterial.ScatteringRemission.GetRemission(lightColor))
        End If

        If intersectionCount >= _MaxIntersectionCount Then Return finalLight

        Dim rayChanger = New RayChanger(Of TLight)(ray, firstIntersection)
        If hitMaterial.Reflects Then
            Dim reflectedRay = rayChanger.ReflectedRay
            Dim reflectionColor = TraceLight(ray:=reflectedRay, intersectionCount:=intersectionCount + 1)
            finalLight = finalLight.Add(hitMaterial.ReflectionRemission.GetRemission(reflectionColor))
        End If
        If hitMaterial.IsTranslucent Then
            Dim passedRay = If(hitMaterial.Refracts, rayChanger.RefractedRay, rayChanger.PassedRay)
            Dim passedColor = TraceLight(ray:=passedRay, intersectionCount:=intersectionCount + 1)
            finalLight = finalLight.Add(hitMaterial.TransparencyRemission.GetRemission(passedColor))
        End If

        Return finalLight
    End Function

    Private ReadOnly _MaxIntersectionCount As Integer

    Public Function GetLight(sightRay As SightRay) As TLight Implements IRayTracer(Of TLight).GetLight
        Return TraceLight(sightRay, intersectionCount:=0)
    End Function
End Class
