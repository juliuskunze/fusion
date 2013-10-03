Public Class ScatteringRayTracer(Of TLight As {ILight(Of TLight), New})
    Implements IRayTracer(Of TLight)

    Public Sub New(surface As ISurface(Of TLight), Optional rayCountPerPixel As Integer = 1, Optional maxIntersectionCount As Integer = 10)
        _Surface = surface
        _RayCount = rayCountPerPixel
        _MaxIntersectionCount = maxIntersectionCount
    End Sub

    Private ReadOnly _Surface As ISurface(Of TLight)

    Private Function TraceColor(sightRay As SightRay, intersectionCount As Integer) As TLight
        Dim firstIntersection = _Surface.FirstMaterialIntersection(sightRay)

        If firstIntersection Is Nothing Then Return _BackColor

        Dim finalColor = firstIntersection.Material.SourceLight

        If intersectionCount >= _MaxIntersectionCount Then
            Return finalColor
        Else
            Dim rayChanger = New RayChanger(Of TLight)(sightRay, firstIntersection)

            If firstIntersection.Material.Scatters Then
                Dim scatteredRay = rayChanger.ScatteredRay
                Dim scatteredColor = TraceColor(sightRay:=scatteredRay, intersectionCount:=intersectionCount + 1)
                finalColor = finalColor.Add(firstIntersection.Material.ScatteringRemission.GetRemission(scatteredColor))
            End If

            If firstIntersection.Material.Reflects Then
                Dim reflectedRay = rayChanger.ReflectedRay()
                Dim reflectionColor = TraceColor(sightRay:=reflectedRay, intersectionCount:=intersectionCount + 1)
                finalColor = finalColor.Add(firstIntersection.Material.ReflectionRemission.GetRemission(reflectionColor))
            End If

            If firstIntersection.Material.IsTranslucent Then
                Dim passedRay As SightRay
                If firstIntersection.Material.Refracts Then
                    passedRay = rayChanger.RefractedRay()
                Else
                    passedRay = rayChanger.PassedRay()
                End If
                Dim passedColor = TraceColor(sightRay:=passedRay, intersectionCount:=intersectionCount + 1)
                finalColor = finalColor.Add(firstIntersection.Material.TransparencyRemission.GetRemission(passedColor))
            End If

            Return finalColor
        End If
    End Function

    Private ReadOnly _BackColor As New TLight
    Private ReadOnly _MaxIntersectionCount As Integer
    Private ReadOnly _RayCount As Integer

    Public Function GetLight(sightRay As SightRay) As TLight Implements IRayTracer(Of TLight).GetLight
        Dim colorSum = New TLight
        For i = 1 To _RayCount
            colorSum = colorSum.Add(TraceColor(sightRay, intersectionCount:=0))
        Next
        Return colorSum.DivideBrightness(_RayCount)
    End Function
End Class