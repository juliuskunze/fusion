Public Class RecursiveRayTracer(Of TLight As {ILight(Of TLight), New})
    Implements IRayTracer(Of TLight)

    Public Sub New(ByVal surface As ISurface(Of Material2D(Of TLight)),
                   ByVal unshadedLightSource As ILightSource(Of TLight),
                   ByVal shadedPointLightSources As List(Of IPointLightSource(Of TLight)),
                   Optional ByVal maxIntersectionCount As Integer = 10)
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
        Set(ByVal value As List(Of IPointLightSource(Of TLight)))
            _ShadedLightSources = New ShadedLightSources(Of TLight)(pointLightSources:=value, shadowingSurface:=Me.Surface)
        End Set
    End Property

    Private _ShadedLightSources As ShadedLightSources(Of TLight)

    Protected Function TraceColor(ByVal ray As Ray, ByVal intersectionCount As Integer) As TLight
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

    Public Overridable Function GetLight(ByVal startRay As Ray) As TLight Implements IRayTracer(Of TLight).GetColor
        Return Me.TraceColor(startRay, intersectionCount:=0)
    End Function

End Class