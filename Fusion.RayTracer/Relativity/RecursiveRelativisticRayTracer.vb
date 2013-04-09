Public Class RecursiveRelativisticRayTracer
    Implements IRayTracer(Of RadianceSpectrum)

    Private ReadOnly _ReferenceFrames As IEnumerable(Of RecursiveRayTracerReferenceFrame)
    Private ReadOnly _Options As LorentzTransformationAtSightRayOptions
    Private ReadOnly _ObserverToBase As LorentzTransformation

    Private Shared ReadOnly _LocationComparer As New RoughVector3DComparer(maxDeviation:=3.2 * 10 ^ -9)

    Public Sub New(referenceFrames As IEnumerable(Of RecursiveRayTracerReferenceFrame), options As LorentzTransformationAtSightRayOptions)
        Me.New(referenceFrames, options, observerToBase:=New LorentzTransformation(New Vector3D))
    End Sub

    Public Sub New(referenceFrames As IEnumerable(Of RecursiveRayTracerReferenceFrame), options As LorentzTransformationAtSightRayOptions, observerToBase As LorentzTransformation)
        If options.IgnoreGeometryEffect Then Throw New ArgumentOutOfRangeException("options", "Geometry effect cannot be ignored in a recursive relativistic raytracer.")

        _ReferenceFrames = referenceFrames
        _Options = options
        _ObserverToBase = observerToBase
    End Sub

    Public Function GetLight(sightRay As SightRay) As RadianceSpectrum Implements IRayTracer(Of RadianceSpectrum).GetLight
        Dim observerSightRay = sightRay
        Dim baseSightRay = _ObserverToBase.TransformSightRay(observerSightRay)

        Dim possibleHits =
            From frame In _ReferenceFrames
            Let objectSurface = frame.RecursiveRayTracer.Surface
            Let baseToObject = frame.BaseToObject
            Let objectSightRay = baseToObject.TransformSightRay(baseSightRay)
            Let objectSurfacePoint = objectSurface.FirstMaterialIntersection(objectSightRay)
            Where objectSurfacePoint IsNot Nothing
            Let objectDistanceFromOrigin = (objectSurfacePoint.Location - objectSightRay.OriginLocation).Length
            Let objectEvent = objectSightRay.GetEvent(objectDistanceFromOrigin)
            Let objectToBase = baseToObject.Inverse
            Let baseEvent = objectToBase.TransformEvent(objectEvent)
            Select frame, objectSurfacePoint, objectSightRay, baseEvent

        If Not possibleHits.Any Then Return New RadianceSpectrum

        Dim hit = possibleHits.MaxItem(Function(possibleHit) possibleHit.baseEvent.Time)
        Dim hitMaterial = hit.objectSurfacePoint.Material
        Dim baseToHitObject = hit.frame.BaseToObject
        Dim hitObjectToBaseAtSightRay = baseToHitObject.Inverse.AtSightRay(hit.objectSightRay).[Partial](_Options)
        Dim baseLight = hitObjectToBaseAtSightRay.TransformRadianceSpectrum(hitMaterial.SourceLight)

        If hitMaterial.Scatters Then
            Dim scatteringLight = hit.frame.RecursiveRayTracer.LightSource.GetLight(hit.objectSurfacePoint)

            Dim scatteringPointLights =
                From lightFrame In _ReferenceFrames
                Let baseToLight = lightFrame.BaseToObject
                Let lightToBase = baseToLight.Inverse
                Let lightHitEvent = baseToLight.TransformEvent(hit.baseEvent)
                    From lightSource In lightFrame.RecursiveRayTracer.ShadedPointLightSources.Cast(Of IPointLightSource(Of RadianceSpectrum))()
                    Let lightSightRay = New SightRay(lightHitEvent, direction:=lightSource.Location - lightHitEvent.Location)
                    Where Not (
                            From shadowFrame In _ReferenceFrames
                            Let lightToShadow = lightToBase.Before(shadowFrame.BaseToObject)
                            Let shadowSightRay = lightToShadow.TransformSightRay(lightSightRay)
                            Let shadowIntersection = shadowFrame.RecursiveRayTracer.Surface.FirstMaterialIntersection(shadowSightRay)
                            Where shadowIntersection IsNot Nothing
                            Where Not _LocationComparer.Equals(shadowIntersection.Location, hit.objectSurfacePoint.Location)).
                            Any()
                    Let light = lightSource.GetMaximumLight(lightHitEvent)
                    Let lightToSurface = baseToLight.Inverse.Before(baseToHitObject)
                    Let lightToSurfaceAtLightSightRay = lightToSurface.AtSightRay(lightSightRay).[Partial](_Options)
                    Let surfaceLightWithoutGeometry = lightToSurfaceAtLightSightRay.TransformRadianceSpectrum(light)
                    Let brightnessFactorByNormalUncut = hit.objectSurfacePoint.NormalizedNormal.DotProduct(lightToSurfaceAtLightSightRay.TransformSightRay.Ray.NormalizedDirection)
                    Let brightnessFactorByNormal = If(brightnessFactorByNormalUncut > 0, brightnessFactorByNormalUncut, 0)
                    Select surfaceLight = surfaceLightWithoutGeometry.MultiplyBrightness(brightnessFactorByNormal)

            Dim scatteringPointLight = scatteringPointLights.Aggregate(New RadianceSpectrum, Function(sum, current) sum.Add(current))

            Dim lightFromSurface = hitMaterial.ScatteringRemission.GetRemission(scatteringLight.Add(scatteringPointLight))

            baseLight = baseLight.Add(hitObjectToBaseAtSightRay.TransformRadianceSpectrum(lightFromSurface))
        End If

        If hitMaterial.Reflects OrElse hitMaterial.IsTranslucent Then
            Throw New NotImplementedException("Reflection and translucence are not implemented.")
        End If

        Dim baseToObserver = _ObserverToBase.Inverse
        Dim baseToObserverAtSightRay = baseToObserver.AtSightRay(baseSightRay)
        Dim observerLight = baseToObserverAtSightRay.TransformRadianceSpectrum(baseLight)

        Return observerLight
    End Function
End Class