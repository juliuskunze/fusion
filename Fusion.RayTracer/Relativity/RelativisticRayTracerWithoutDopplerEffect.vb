Public Class RelativisticRayTracerWithoutDopplerEffect(Of TLight As {ILight(Of TLight), New})
    Inherits RelativisticRayTracerBase(Of TLight)

    Public Sub New(classicRayTracer As IRayTracer(Of TLight),
                    cameraVelocity As Vector3D)
        MyBase.New(classicRayTracer:=classicRayTracer, observerVelocity:=cameraVelocity)
    End Sub

    Public Overrides Function GetLight(sightRay As Ray) As TLight
        Dim sightRayInS = LorentzTransformation.InverseSemiTransformSightRay(sightRayInTWithOriginInS:=sightRay)
        Dim lightInS = ClassicRayTracer.GetLight(sightRayInS)

        Dim searchlightFactor = LorentzTransformation.TransformSpectralRadiance(normalizedSightRayDirectionInS:=sightRay.NormalizedDirection, spectralRadiance:=1)

        Return lightInS.MultiplyBrightness(searchlightFactor)
    End Function

End Class
