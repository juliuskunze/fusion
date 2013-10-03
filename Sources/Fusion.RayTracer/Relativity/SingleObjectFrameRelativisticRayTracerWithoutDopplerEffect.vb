Public Class SingleReferenceFrameRelativisticRayTracerWithoutDopplerEffect(Of TLight As {ILight(Of TLight), New})
    Inherits SingleReferenceFrameRelativisticRayTracerBase(Of TLight)

    Public Sub New(classicRayTracer As IRayTracer(Of TLight),
                   observerVelocity As Vector3D)
        MyBase.New(classicRayTracer:=classicRayTracer, observerVelocity:=observerVelocity)
    End Sub

    Public Overrides Function GetLight(sightRay As SightRay) As TLight
        Dim observerSightRay = sightRay
        Dim objectSightRay = ObjectToObserver.Inverse.AtSightRay(observerSightRay).SemiTransformSightRay
        Dim light = ClassicRayTracer.GetLight(objectSightRay)

        Dim searchlightFactor = ObjectToObserver.AtSightRay(observerSightRay).TransformSpectralRadiance(spectralRadiance:=1)

        Return light.MultiplyBrightness(searchlightFactor)
    End Function
End Class
