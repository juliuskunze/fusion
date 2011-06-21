Public Class RecursiveRayTracer
    Implements IRayTracer

    Public Sub New(ByVal surface As IMaterialSurface(Of Material2D))
        Me.New(surface, LightSource:=New LightSources, ShadedPointLightSources:=New ShadedLightSources(surface))
    End Sub

    Public Sub New(ByVal surface As IMaterialSurface(Of Material2D),
                   ByVal lightSource As ILightSource(Of ExactColor),
                   ByVal shadedPointLightSources As List(Of IPointLightSource(Of ExactColor)),
                   Optional ByVal maxIntersectionCount As Integer = 10)
        Me.Surface = surface
        Me.LightSource = lightSource
        Me.ShadedPointLightSources = shadedPointLightSources
        Me.MaxIntersectionCount = maxIntersectionCount
    End Sub

    Public Property Surface As IMaterialSurface(Of Material2D)
    Public Property LightSource As ILightSource(Of ExactColor)

    Public Property ShadedPointLightSources As List(Of IPointLightSource(Of ExactColor))
        Get
            Return _ShadedLightSources
        End Get
        Set(ByVal value As List(Of IPointLightSource(Of ExactColor)))
            _ShadedLightSources = New ShadedLightSources(pointLightSources:=value, shadowingSurface:=Me.Surface)
        End Set
    End Property

    Private _ShadedLightSources As ShadedLightSources

    Protected Function TraceColor(ByVal ray As Ray, ByVal intersectionCount As Integer) As ExactColor
        Dim firstIntersection = Me.Surface.FirstMaterialIntersection(ray)

        If firstIntersection Is Nothing Then Return Me.BackColor

        Dim hitMaterial = firstIntersection.Material

        Dim finalColor = hitMaterial.LightSourceColor
        If hitMaterial.Scatters Then
            Dim lightColor = Me.LightSource.GetLight(firstIntersection) + _ShadedLightSources.LightColor(firstIntersection)
            finalColor += hitMaterial.ScatteringRemission.GetRemission(lightColor)
        End If

        If intersectionCount >= Me.MaxIntersectionCount Then
            Return finalColor
        Else
            Dim rayChanger = New RayChanger(ray)
            If hitMaterial.Reflects Then
                Dim reflectedRay = rayChanger.ReflectedRay(firstIntersection)
                Dim reflectionColor = Me.TraceColor(ray:=reflectedRay, intersectionCount:=intersectionCount + 1)
                finalColor += hitMaterial.ReflectionRemission.GetRemission(reflectionColor)
            End If
            If hitMaterial.IsTranslucent Then
                Dim passedRay As Ray
                If hitMaterial.Refracts Then
                    passedRay = rayChanger.RefractedRay(firstIntersection)
                Else
                    passedRay = rayChanger.PassedRay(firstIntersection)
                End If
                Dim passedColor = Me.TraceColor(ray:=passedRay, intersectionCount:=intersectionCount + 1)
                finalColor += hitMaterial.TransparencyRemission.GetRemission(passedColor)
            End If
            Return finalColor
        End If
    End Function

    Public Property BackColor As ExactColor = ExactColor.Black
    Public Property MaxIntersectionCount As Integer

    Public Overridable Function GetColor(ByVal startRay As Ray) As ExactColor Implements IRayTracer.GetColor
        Return Me.TraceColor(startRay, intersectionCount:=0)
    End Function

End Class
