Public Class ScatteringRayTracer(Of TLight As {ILight(Of TLight), New})
    Implements IRayTracer(Of TLight)

    Public Sub New(surface As ISurface(Of Material2D(Of TLight)), Optional rayCountPerPixel As Integer = 1, Optional maxIntersectionCount As Integer = 10)
        Me.Surface = surface
        Me.RayCount = rayCountPerPixel
        Me.MaxIntersectionCount = maxIntersectionCount
    End Sub

    Public Property Surface As ISurface(Of Material2D(Of TLight))

    Private Function TraceColor(ray As Ray, intersectionCount As Integer) As TLight
        Dim firstIntersection = Me.Surface.FirstMaterialIntersection(ray)

        If firstIntersection Is Nothing Then Return Me.BackColor

        Dim finalColor = firstIntersection.Material.SourceLight

        If intersectionCount >= Me.MaxIntersectionCount Then
            Return finalColor
        Else
            Dim rayChanger = New RayChanger(ray)

            If firstIntersection.Material.Scatters Then
                Dim scatteredRay = New RayChanger(ray).ScatteredRay(firstIntersection)
                Dim scatteredColor = TraceColor(ray:=scatteredRay, intersectionCount:=intersectionCount + 1)
                finalColor = finalColor.Add(firstIntersection.Material.ScatteringRemission.GetRemission(scatteredColor))
            End If

            If firstIntersection.Material.Reflects Then
                Dim reflectedRay = rayChanger.ReflectedRay(firstIntersection)
                Dim reflectionColor = TraceColor(ray:=reflectedRay, intersectionCount:=intersectionCount + 1)
                finalColor = finalColor.Add(firstIntersection.Material.ReflectionRemission.GetRemission(reflectionColor))
            End If

            If firstIntersection.Material.IsTranslucent Then
                Dim passedRay As Ray
                If firstIntersection.Material.Refracts Then
                    passedRay = rayChanger.RefractedRay(firstIntersection)
                Else
                    passedRay = rayChanger.PassedRay(firstIntersection)
                End If
                Dim passedColor = TraceColor(ray:=passedRay, intersectionCount:=intersectionCount + 1)
                finalColor = finalColor.Add(firstIntersection.Material.TransparencyRemission.GetRemission(passedColor))
            End If

            Return finalColor
        End If
    End Function

    Public Property BackColor As New TLight
    Public Property MaxIntersectionCount As Integer
    Public Property RayCount As Integer

    Public Function GetLight(sightRay As Ray) As TLight Implements IRayTracer(Of TLight).GetLight
        Dim colorSum = New TLight
        For i = 1 To Me.RayCount
            colorSum = colorSum.Add(Me.TraceColor(sightRay, intersectionCount:=0))
        Next
        Return colorSum.DivideBrightness(Me.RayCount)
    End Function

End Class