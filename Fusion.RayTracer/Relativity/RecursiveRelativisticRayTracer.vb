﻿Public Class RecursiveRelativisticRayTracer
    Implements IRayTracer(Of RadianceSpectrum)

    Private ReadOnly _ReferenceFrames As IEnumerable(Of RecursiveRayTracerReferenceFrame)
    Private ReadOnly _Options As LorentzTransformationAtSightRayOptions

    Private Shared ReadOnly _LocationComparer As New Vector3DRoughComparer(maxDeviation:=3.2 * 10 ^ -9)

    Public Sub New(referenceFrames As IEnumerable(Of RecursiveRayTracerReferenceFrame), options As LorentzTransformationAtSightRayOptions)
        If options.IgnoreGeometryEffect Then Throw New ArgumentOutOfRangeException("options", "Geometry effect cannot be ignored in a recursive relativistic raytracer.")

        _ReferenceFrames = referenceFrames
        _Options = options
    End Sub

    Public Function GetLight(sightRay As SightRay) As RadianceSpectrum Implements IRayTracer(Of RadianceSpectrum).GetLight
        Dim observerSightRay = sightRay

        Dim hits =
            From frame In _ReferenceFrames
            Let surface = frame.RecursiveRayTracer.Surface
            Let observerToObject = frame.ObserverToObject
            Let objectSightRay = observerToObject.TransformSightRay(observerSightRay)
            Let objectSurfacePoint = surface.FirstMaterialIntersection(objectSightRay)
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

            Dim scatteringPointLights =
                From lightFrame In _ReferenceFrames
                Let observerToLight = lightFrame.ObserverToObject
                Let lightHitEvent = lightFrame.ObserverToObject.TransformEvent(actualHit.event)
                    From lightSource In lightFrame.RecursiveRayTracer.ShadedPointLightSources.Cast(Of IPointLightSource(Of RadianceSpectrum))()
                    Let lightSightRay = New SightRay(lightHitEvent, direction:=lightSource.Location - lightHitEvent.Location)
                    Where Not (
                            From surfaceFrame In _ReferenceFrames
                            Let lightToSurface = observerToLight.Inverse.Before(surfaceFrame.ObserverToObject)
                            Let surfaceSightRay = lightToSurface.TransformSightRay(lightSightRay)
                            Let surfaceIntersection = surfaceFrame.RecursiveRayTracer.Surface.FirstMaterialIntersection(surfaceSightRay)
                            Where surfaceIntersection IsNot Nothing
                            Where Not _LocationComparer.Equals(surfaceIntersection.Location, actualHit.objectSurfacePoint.Location)).
                            Any()
                    Let light = lightSource.GetLight(lightHitEvent.Location)
                    Let lightToSurface = observerToLight.Inverse.Before(observerToSurface)
                    Let lightToSurfaceAtLightSightRay = lightToSurface.AtSightRay(lightSightRay).Partly(_Options)
                    Let surfaceLightWithoutGeometry = lightToSurfaceAtLightSightRay.TransformRadianceSpectrum(light)
                    Let brightnessFactorByNormalUncut = actualHit.objectSurfacePoint.NormalizedNormal.DotProduct(lightToSurfaceAtLightSightRay.TransformSightRay.Ray.NormalizedDirection)
                    Let brightnessFactorByNormal = If(brightnessFactorByNormalUncut > 0, brightnessFactorByNormalUncut, 0)
                    Select surfaceLight = surfaceLightWithoutGeometry.MultiplyBrightness(brightnessFactorByNormal)

            Dim scatteringPointLight = scatteringPointLights.Aggregate(New RadianceSpectrum, Function(sum, current) sum.Add(current))

            Dim lightFromSurface = hitMaterial.ScatteringRemission.GetRemission(scatteringLight.Add(scatteringPointLight))

            finalLight = finalLight.Add(surfaceToObserverAtSightRay.TransformRadianceSpectrum(lightFromSurface))
        End If

        If hitMaterial.Reflects OrElse hitMaterial.IsTranslucent Then
            Throw New NotImplementedException("Reflection and translucence at the same time is not implemented.")
        End If

        Return finalLight
    End Function
End Class