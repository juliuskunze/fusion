Public Class RecursiveMultiReferenceFrameRayTracer
    Implements IRayTracer(Of RadianceSpectrum)

    Private ReadOnly _ObserverTime As Double
    Private ReadOnly _ReferenceFrames As IEnumerable(Of RecursiveRayTracerReferenceFrame(Of RadianceSpectrum))
    Private ReadOnly _Options As RadianceSpectrumLorentzTransformationOptions

    Public Sub New(observerTime As Double, referenceFrames As IEnumerable(Of RecursiveRayTracerReferenceFrame(Of RadianceSpectrum)), options As RadianceSpectrumLorentzTransformationOptions)
        _ObserverTime = observerTime
        _ReferenceFrames = referenceFrames
        _Options = options
    End Sub

    Public ReadOnly Property Options() As RadianceSpectrumLorentzTransformationOptions
        Get
            Return _Options
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


    Private Function TraceLight(observerSightRay As SightRay) As RadianceSpectrum
        Dim hits =
            From frame In _ReferenceFrames
            Let surface = frame.RecursiveRayTracer.Surface
            Let observerToObject = frame.ObserverToObject
            Let objectSightRay = observerToObject.TransformSightRay(observerSightRay)
            Let objectSurfacePoint = surface.FirstMaterialIntersection(objectSightRay.Ray)
            Where objectSurfacePoint IsNot Nothing
            Let objectDistanceFromOrigin = (objectSurfacePoint.Location - objectSightRay.OriginLocation).Length
            Let objectEvent = objectSightRay.GetEvent(objectDistanceFromOrigin)
            Let [event] = observerToObject.Inverse.TransformEvent(objectEvent)
            Select frame, objectSurfacePoint, objectSightRay, [event]

        If Not hits.Any Then Return New RadianceSpectrum

        Dim actualHit = hits.MaxItem(Function(hit) hit.event.Time)
        Dim hitMaterial = actualHit.objectSurfacePoint.Material
        Dim objectToObserverAtSightRay = actualHit.frame.ObserverToObject.Inverse.AtSightRay(actualHit.objectSightRay).Partly(_Options)
        Dim finalLight = objectToObserverAtSightRay.TransformRadianceSpectrum(hitMaterial.SourceLight)

        If hitMaterial.Scatters Then
            Dim lightColor = actualHit.frame.RecursiveRayTracer.LightSource.GetLight(actualHit.objectSurfacePoint)

            Dim a = From frame In _ReferenceFrames
                    Let observerToPointLight = frame.ObserverToObject
                    Let pointLightHitEvent = frame.ObserverToObject.TransformEvent(actualHit.event)
                        From pointLightSource In frame.RecursiveRayTracer.ShadedPointLightSources
                        Let pointLightSightRay = New SightRay(pointLightHitEvent, direction:=pointLightSource.Location - pointLightHitEvent.Location)
                            From surfaceFrame In _ReferenceFrames
                            Let pointLightToSurface = observerToPointLight.Inverse.Before(surfaceFrame.ObserverToObject)
                            Let surfaceFrameSightRay = pointLightToSurface.TransformSightRay(pointLightSightRay)
                            Let barrier = surfaceFrame.RecursiveRayTracer.Surface.FirstMaterialIntersection(surfaceFrameSightRay.Ray)
                            Where barrier.Location = actualHit.objectSurfacePoint.Location

            finalLight = finalLight.Add(hitMaterial.ScatteringRemission.GetRemission(lightColor))
        End If

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
