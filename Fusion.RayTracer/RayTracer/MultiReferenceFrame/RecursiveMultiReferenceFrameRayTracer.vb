Public Class RecursiveMultiReferenceFrameRayTracer(Of TLight As {ILight(Of TLight), New})
    Implements IRayTracer(Of TLight)

    Private ReadOnly _ObserverTime As Double
    Private ReadOnly _ReferenceFrames As IEnumerable(Of RecursiveRayTracerReferenceFrame(Of TLight))

    Public Sub New(observerTime As Double, referenceFrames As IEnumerable(Of RecursiveRayTracerReferenceFrame(Of TLight)))
        _ObserverTime = observerTime
        _ReferenceFrames = referenceFrames
    End Sub

    Public ReadOnly Property ReferenceFrames() As IEnumerable(Of RecursiveRayTracerReferenceFrame(Of TLight))
        Get
            Return _ReferenceFrames
        End Get
    End Property

    Private Function TraceLight(sightRay As SightRay, intersectionCount As Integer) As TLight
        Dim hits = From frame In _ReferenceFrames
                             Let surface = frame.RecursiveRayTracer.Surface
                             Let transformation = frame.Transformation
                             Let transformedSightRay = transformation.TransformSightRay(sightRay)
                             Let transformedSurfacePoint = surface.FirstMaterialIntersection(transformedSightRay.Ray)
                             Where transformedSurfacePoint IsNot Nothing
                             Let transformedDistanceFromOrigin = (transformedSurfacePoint.Location - transformedSightRay.OriginLocation).Length
                             Let transformedHitEvent = transformedSightRay.GetEvent(transformedDistanceFromOrigin)
                             Let hitEvent = transformation.Inverse.TransformEvent(transformedHitEvent)
                             Select transformedSurfacePoint, transformedSightRay, hitEvent

        If Not hits.Any Then Return New TLight

        Dim realHit = hits.MaxItem(Function(hit) hit.hitEvent.Time)

        Dim hitMaterial = realHit.transformedSurfacePoint.Material

        Dim finalLight = hitMaterial.SourceLight

        If hitMaterial.Scatters Then
            Dim lightColor = LightSource.GetLight(firstIntersection).Add(_ShadedPointLightSources.GetLight(firstIntersection))
            finalLight = finalLight.Add(hitMaterial.ScatteringRemission.GetRemission(lightColor))
        End If

    End Function

    Public Function GetLight(sightRay As SightRay) As TLight
        Return TraceLight(sightRay, intersectionCount:=0)
    End Function

    Public Function GetLight(sightRay As Ray) As TLight Implements IRayTracer(Of TLight).GetLight
        Return GetLight(New SightRay(sightRay, _ObserverTime))
    End Function
End Class
