Public Class RelativisticRayTracerWithoutDopplerEffect(Of TLight As {ILight(Of TLight), New})
    Inherits RelativisticRayTracerBase(Of TLight)

    Public Sub New(ByVal classicRayTracer As IRayTracer(Of TLight),
                   ByVal cameraVelocity As Vector3D)
        MyBase.New(classicRayTracer:=classicRayTracer, observerVelocity:=cameraVelocity)
    End Sub

    Public Overrides Function GetLight(ByVal viewRay As Math.Ray) As TLight
        Dim viewRayInS = _RayTransformation.GetViewRayInS(viewRayInT:=viewRay)
        Dim lightInS = _ClassicRayTracer.GetLight(viewRayInS)

        Dim searchlightFactor = _RayTransformation.GetSpectralRadianceInT(viewRayInS:=viewRay, spectralRadianceInS:=1)

        Return lightInS.MultiplyBrightness(searchlightFactor)
    End Function

End Class
