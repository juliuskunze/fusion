Public Class SingleReferenceFrameRelativisticRayTracerWithoutDopplerEffect(Of TLight As {ILight(Of TLight), New})
    Inherits SingleReferenceFrameRelativisticRayTracerBase(Of TLight)

    Public Sub New(classicRayTracer As IRayTracer(Of TLight),
                    cameraVelocity As Vector3D)
        MyBase.New(classicRayTracer:=classicRayTracer, observerVelocity:=cameraVelocity)
    End Sub

    Public Overrides Function GetLight(observerSightRayWithObjectOrigin As Ray) As TLight
        Dim objectSightRay = ObjectToObserver.Inverse.SemiTransformSightRay(observerSightRayWithObjectOrigin)
        Dim light = ClassicRayTracer.GetLight(objectSightRay)

        Dim searchlightFactor = ObjectToObserver.AtSightRayDirection(observerSightRayWithObjectOrigin.NormalizedDirection).TransformSpectralRadiance(spectralRadiance:=1)

        Return light.MultiplyBrightness(searchlightFactor)
    End Function

End Class
