Public Class RecursiveMultiReferenceFrameRayTracer
    Implements IRayTracer(Of RadianceSpectrum)

    Private ReadOnly _ObserverTime As Double
    Private ReadOnly _ReferenceFrames As IEnumerable(Of RecursiveRayTracerReferenceFrame(Of RadianceSpectrum))
    Private ReadOnly _TransformationOptions As RadianceSpectrumLorentzTransformationOptions

    Public Sub New(observerTime As Double, referenceFrames As IEnumerable(Of RecursiveRayTracerReferenceFrame(Of RadianceSpectrum)), transformationOptions As RadianceSpectrumLorentzTransformationOptions)
        _ObserverTime = observerTime
        _ReferenceFrames = referenceFrames
        _TransformationOptions = transformationOptions
    End Sub

    Public ReadOnly Property TransformationOptions() As RadianceSpectrumLorentzTransformationOptions
        Get
            Return _TransformationOptions
        End Get
    End Property

    Public ReadOnly Property ReferenceFrames() As IEnumerable(Of RecursiveRayTracerReferenceFrame(Of RadianceSpectrum))
        Get
            Return _ReferenceFrames
        End Get
    End Property

    Public ReadOnly Property ObserverTime() As Double
        Get
            Return _ObserverTime
        End Get
    End Property


    Private Function TraceLight(sightRay As SightRay) As RadianceSpectrum
        Dim hits = From frame In _ReferenceFrames
                             Let surface = frame.RecursiveRayTracer.Surface
                             Let transformation = frame.Transformation
                             Let transformedSightRay = transformation.TransformSightRay(sightRay)
                             Let transformedSurfacePoint = surface.FirstMaterialIntersection(transformedSightRay.Ray)
                             Where transformedSurfacePoint IsNot Nothing
                             Let transformedDistanceFromOrigin = (transformedSurfacePoint.Location - transformedSightRay.OriginLocation).Length
                             Let transformedHitEvent = transformedSightRay.GetEvent(transformedDistanceFromOrigin)
                             Let hitEvent = transformation.Inverse.TransformEvent(transformedHitEvent)
                             Select transformation, transformedSurfacePoint, transformedSightRay, hitEvent

        If Not hits.Any Then Return New RadianceSpectrum

        Dim realHit = hits.MaxItem(Function(hit) hit.hitEvent.Time)
        Dim hitMaterial = realHit.transformedSurfacePoint.Material
        Dim lorentzBackTransformationAtSightRay = realHit.transformation.Inverse.AtSightRayDirection(realHit.transformedSightRay.Ray.NormalizedDirection)
        Dim finalLight = lorentzBackTransformationAtSightRay.TransformRadianceSpectrum(hitMaterial.SourceLight)

        'If hitMaterial.Scatters Then
        '    Dim lightColor = LightSource.GetLight(firstIntersection).Add(_ShadedPointLightSources.GetLight(firstIntersection))
        '    finalLight = finalLight.Add(hitMaterial.ScatteringRemission.GetRemission(lightColor))
        'End If

        If hitMaterial.Reflects OrElse hitMaterial.IsTranslucent Then
            Throw New NotImplementedException("Reflection and translucence are not implemented.")
        End If

        Return finalLight
    End Function

    Public Function GetLight(sightRay As SightRay) As RadianceSpectrum
        Return TraceLight(sightRay)
    End Function

    Public Function GetLight(sightRay As Ray) As RadianceSpectrum Implements IRayTracer(Of RadianceSpectrum).GetLight
        Return GetLight(New SightRay(sightRay, _ObserverTime))
    End Function
End Class
