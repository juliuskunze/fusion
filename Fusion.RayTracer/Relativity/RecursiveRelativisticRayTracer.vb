Public Class RecursiveRelativisticRayTracer
    Implements IRayTracer(Of RadianceSpectrum)

    Private ReadOnly _ObserverTime As Double
    Private ReadOnly _ReferenceFrames As IEnumerable(Of RecursiveRayTracerReferenceFrame)
    Private ReadOnly _Options As LorentzTransformationAtSightRayOptions

    Private Shared ReadOnly _LocationComparer As New Vector3DRoughComparer(maxDeviation:=3.2 * 10 ^ -9)

    Public Sub New(observerTime As Double, referenceFrames As IEnumerable(Of RecursiveRayTracerReferenceFrame), options As LorentzTransformationAtSightRayOptions)
        If options.IgnoreGeometryEffect Then Throw New ArgumentOutOfRangeException("options", "Geometry effect cannot be ignored in a recursive relativistic raytracer.")

        _ObserverTime = observerTime
        _ReferenceFrames = referenceFrames
        _Options = options
    End Sub

    Public ReadOnly Property Options As LorentzTransformationAtSightRayOptions
        Get
            Return _Options
        End Get
    End Property

    Public ReadOnly Property ReferenceFrames As IEnumerable(Of RecursiveRayTracerReferenceFrame)
        Get
            Return _ReferenceFrames
        End Get
    End Property

    Public ReadOnly Property ObserverTime As Double
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
        Dim observerToSurface = actualHit.frame.ObserverToObject
        Dim surfaceToObserverAtSightRay = observerToSurface.Inverse.AtSightRay(actualHit.objectSightRay).Partly(_Options)
        Dim finalLight = surfaceToObserverAtSightRay.TransformRadianceSpectrum(hitMaterial.SourceLight)

        If hitMaterial.Scatters Then
            Dim scatteringLight = actualHit.frame.RecursiveRayTracer.LightSource.GetLight(actualHit.objectSurfacePoint)

            Dim scatteringPointLights = From lightFrame In _ReferenceFrames
                    Let observerToLight = lightFrame.ObserverToObject
                    Let lightHitEvent = lightFrame.ObserverToObject.TransformEvent(actualHit.event)
                        From lightSource In lightFrame.RecursiveRayTracer.ShadedPointLightSources.Cast(Of PointLightSource(Of RadianceSpectrum))()
                        Let lightSightRay = New SightRay(lightHitEvent, direction:=lightSource.Location - lightHitEvent.Location)
                        Where Not (
                            From surfaceFrame In _ReferenceFrames
                            Let lightToSurface = observerToLight.Inverse.Before(surfaceFrame.ObserverToObject)
                            Let surfaceSightRay = lightToSurface.TransformSightRay(lightSightRay)
                            Let surfaceIntersection = surfaceFrame.RecursiveRayTracer.Surface.FirstMaterialIntersection(surfaceSightRay.Ray)
                            Where Not _LocationComparer.Equals(surfaceIntersection.Location, actualHit.objectSurfacePoint.Location)).
                            Any()
                        Let light = lightSource.GetLightAtPoint(lightHitEvent.Location)
                        Let lightToSurface = observerToLight.Inverse.Before(observerToSurface)
                        Let lightToSurfaceAtLightSightRay = lightToSurface.AtSightRay(lightSightRay)
                        Let surfaceLightWithoutGeometry = lightToSurfaceAtLightSightRay.Partly(_Options).TransformRadianceSpectrum(light)
                        Let brightnessFactorByNormalUncut = actualHit.objectSurfacePoint.NormalizedNormal.DotProduct(lightSightRay.Ray.NormalizedDirection)
                        Let brightnessFactorByNormal = If(brightnessFactorByNormalUncut > 0, brightnessFactorByNormalUncut, 0)
                        Select surfaceLight = surfaceLightWithoutGeometry.MultiplyBrightness(brightnessFactorByNormal)

            Dim scatteringPointLight = scatteringPointLights.Aggregate(New RadianceSpectrum, Function(sum, current) sum.Add(current))

            Dim lightFromSurface = hitMaterial.ScatteringRemission.GetRemission(scatteringLight.Add(scatteringPointLight))

            finalLight = finalLight.Add(surfaceToObserverAtSightRay.TransformRadianceSpectrum(lightFromSurface))
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
