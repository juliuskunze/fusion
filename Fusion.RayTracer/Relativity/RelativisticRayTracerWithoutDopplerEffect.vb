Public Class RelativisticRayTracerWithoutDopplerEffect(Of TLight As {ILight(Of TLight), New})
    Inherits RelativisticRayTracerBase(Of TLight)

    Public Sub New(classicRayTracer As IRayTracer(Of TLight),
                    cameraVelocity As Vector3D)
        MyBase.New(classicRayTracer:=classicRayTracer, observerVelocity:=cameraVelocity)
    End Sub

    Public Overrides Function GetLight(viewRay As Ray) As TLight
        Dim viewRayInS = LorentzTransformation.InverseSemiTransformViewRay(viewRayInTWithOriginInS:=viewRay)
        Dim lightInS = ClassicRayTracer.GetLight(viewRayInS)

        Dim searchlightFactor = LorentzTransformation.TransformSpectralRadiance(sightRayInS:=viewRay, spectralRadianceInS:=1)

        Return lightInS.MultiplyBrightness(searchlightFactor)
    End Function

End Class
