Public Class RecursiveRayTracer(Of TLight As {ILight(Of TLight), New})
    Implements IRayTracer(Of TLight)

    Public Sub New(surface As ISurface(Of Material2D(Of TLight)),
                   unshadedLightSource As ILightSource(Of TLight),
                   shadedPointLightSources As List(Of IPointLightSource(Of TLight)),
                   Optional maxIntersectionCount As Integer = 10)
        Me.Surface = surface
        Me.LightSource = unshadedLightSource
        Me.ShadedPointLightSources = shadedPointLightSources
        Me.MaxIntersectionCount = maxIntersectionCount
    End Sub

    Public Property Surface As ISurface(Of Material2D(Of TLight))
    Public Property LightSource As ILightSource(Of TLight)

    Public Property ShadedPointLightSources As List(Of IPointLightSource(Of TLight))
        Get
            Return _ShadedLightSources
        End Get
        Set(value As List(Of IPointLightSource(Of TLight)))
            _ShadedLightSources = New ShadedLightSources(Of TLight)(pointLightSources:=value, shadowingSurface:=Me.Surface)
        End Set
    End Property

    Private _ShadedLightSources As ShadedLightSources(Of TLight)

    Protected Function TraceColor(ray As Ray, intersectionCount As Integer) As TLight
        Dim firstIntersection = Me.Surface.FirstMaterialIntersection(ray)

        If firstIntersection Is Nothing Then Return Me.BackgroundLight

        Dim hitMaterial = firstIntersection.Material

        Dim finalLight = hitMaterial.SourceLight
        If hitMaterial.Scatters Then
            Dim lightColor = Me.LightSource.GetLight(firstIntersection).Add(_ShadedLightSources.GetLight(firstIntersection))
            finalLight = finalLight.Add(hitMaterial.ScatteringRemission.GetRemission(lightColor))
        End If

        If intersectionCount >= Me.MaxIntersectionCount Then Return finalLight

        Dim rayChanger = New RayChanger(ray)
        If hitMaterial.Reflects Then
            Dim reflectedRay = rayChanger.ReflectedRay(firstIntersection)
            Dim reflectionColor = Me.TraceColor(ray:=reflectedRay, intersectionCount:=intersectionCount + 1)
            finalLight = finalLight.Add(hitMaterial.ReflectionRemission.GetRemission(reflectionColor))
        End If
        If hitMaterial.IsTranslucent Then
            Dim passedRay As Ray
            If hitMaterial.Refracts Then
                passedRay = rayChanger.RefractedRay(firstIntersection)
            Else
                passedRay = rayChanger.PassedRay(firstIntersection)
            End If
            Dim passedColor = Me.TraceColor(ray:=passedRay, intersectionCount:=intersectionCount + 1)
            finalLight = finalLight.Add(hitMaterial.TransparencyRemission.GetRemission(passedColor))
        End If

        Return finalLight
    End Function

    Public Property BackgroundLight As New TLight
    Public Property MaxIntersectionCount As Integer

    Public Overridable Function GetLight(viewRay As Ray) As TLight Implements IRayTracer(Of TLight).GetLight
        Return Me.TraceColor(viewRay, intersectionCount:=0)
    End Function

End Class
