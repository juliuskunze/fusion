Public Class ScatteringRayTracer
    Implements IRayTracer

    Public Sub New(ByVal surface As IMaterialSurface(Of Material2D), Optional ByVal rayCount As Integer = 1, Optional ByVal maxIntersectionCount As Integer = 10)
        Me.Surface = surface
        Me.RayCount = rayCount
        Me.MaxIntersectionCount = maxIntersectionCount
    End Sub

    Public Property Surface As IMaterialSurface(Of Material2D)

    Private Function TraceColor(ByVal ray As Ray, ByVal intersectionCount As Integer) As ExactColor
        Dim firstIntersection = Me.Surface.FirstMaterialIntersection(ray)

        If firstIntersection Is Nothing Then Return Me.BackColor

        Dim finalColor = firstIntersection.Material.LightSourceColor

        If intersectionCount >= Me.MaxIntersectionCount Then
            Return finalColor
        Else
            Dim rayChanger = New RayChanger(ray)

            If firstIntersection.Material.Scatters Then
                Dim scatteredRay = New RayChanger(ray).ScatteredRay(firstIntersection)
                Dim scatteredColor = TraceColor(ray:=scatteredRay, intersectionCount:=intersectionCount + 1)
                finalColor += firstIntersection.Material.ScatteringRemission.Color(scatteredColor)
            End If

            If firstIntersection.Material.Reflects Then
                Dim reflectedRay = rayChanger.ReflectedRay(firstIntersection)
                Dim reflectionColor = TraceColor(ray:=reflectedRay, intersectionCount:=intersectionCount + 1)
                finalColor += firstIntersection.Material.ReflectionRemission.Color(reflectionColor)
            End If

            If firstIntersection.Material.IsTranslucent Then
                Dim passedRay As Ray
                If firstIntersection.Material.Refracts Then
                    passedRay = rayChanger.RefractedRay(firstIntersection)
                Else
                    passedRay = rayChanger.PassedRay(firstIntersection)
                End If
                Dim passedColor = TraceColor(ray:=passedRay, intersectionCount:=intersectionCount + 1)
                finalColor += firstIntersection.Material.TransparencyRemission.Color(passedColor)
            End If

            Return finalColor
        End If
    End Function

    Public Property BackColor As ExactColor = ExactColor.Black
    Public Property MaxIntersectionCount As Integer
    Public Property RayCount As Integer

    Public Function GetColor(ByVal startRay As Ray) As ExactColor Implements IRayTracer.GetColor
        Dim colorSum = ExactColor.Black
        For i = 1 To Me.RayCount
            colorSum += Me.TraceColor(startRay, intersectionCount:=0)
        Next
        Return colorSum / Me.RayCount
    End Function

End Class