Public Class RecursiveRayTracer(Of TLight As {ILight(Of TLight), New})
    Implements IRayTracer(Of TLight)

    Public Sub New(surface As ISurface(Of Material2D(Of TLight)),
                   unshadedLightSource As ILightSource(Of TLight),
                   shadedPointLightSources As IEnumerable(Of IPointLightSource(Of TLight)),
                   Optional maxIntersectionCount As Integer = 10)
        _Surface = surface
        _LightSource = unshadedLightSource
        _ShadedPointLightSources = New ShadedLightSources(Of TLight)(shadowingSurface:=_Surface, pointLightSources:=shadedPointLightSources)
        _MaxIntersectionCount = maxIntersectionCount
    End Sub

    Private ReadOnly _Surface As ISurface(Of Material2D(Of TLight))
    Public ReadOnly Property Surface As ISurface(Of Material2D(Of TLight))
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
    Public ReadOnly Property ShadedPointLightSources As IEnumerable(Of IPointLightSource(Of TLight))
        Get
            Return _ShadedPointLightSources
        End Get
    End Property

    Protected Function TraceLight(ray As Ray, intersectionCount As Integer) As TLight
        Dim firstIntersection = Surface.FirstMaterialIntersection(ray)

        If firstIntersection Is Nothing Then Return New TLight

        Dim hitMaterial = firstIntersection.Material

        Dim finalLight = hitMaterial.SourceLight
        If hitMaterial.Scatters Then
            Dim lightColor = LightSource.GetLight(firstIntersection).Add(_ShadedPointLightSources.GetLight(firstIntersection))
            finalLight = finalLight.Add(hitMaterial.ScatteringRemission.GetRemission(lightColor))
        End If

        If intersectionCount >= MaxIntersectionCount Then Return finalLight

        Dim rayChanger = New RayChanger(ray)
        If hitMaterial.Reflects Then
            Dim reflectedRay = rayChanger.ReflectedRay(firstIntersection)
            Dim reflectionColor = TraceLight(ray:=reflectedRay, intersectionCount:=intersectionCount + 1)
            finalLight = finalLight.Add(hitMaterial.ReflectionRemission.GetRemission(reflectionColor))
        End If
        If hitMaterial.IsTranslucent Then
            Dim passedRay As Ray
            If hitMaterial.Refracts Then
                passedRay = rayChanger.RefractedRay(firstIntersection)
            Else
                passedRay = rayChanger.PassedRay(firstIntersection)
            End If
            Dim passedColor = TraceLight(ray:=passedRay, intersectionCount:=intersectionCount + 1)
            finalLight = finalLight.Add(hitMaterial.TransparencyRemission.GetRemission(passedColor))
        End If

        Return finalLight
    End Function

    Private ReadOnly _MaxIntersectionCount As Integer
    Public ReadOnly Property MaxIntersectionCount As Integer
        Get
            Return _MaxIntersectionCount
        End Get
    End Property

    Public Overridable Function GetLight(sightRay As Ray) As TLight Implements IRayTracer(Of TLight).GetLight
        Return TraceLight(sightRay, intersectionCount:=0)
    End Function

End Class
