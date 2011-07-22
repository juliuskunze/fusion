Public Class RelativisticRayTracerWithoutDopplerEffect(Of TLight As {ILight(Of TLight), New})
    Inherits RelativisticRayTracerBase(Of TLight)

    Public Sub New(ByVal classicRayTracer As IRayTracer(Of TLight),
                   ByVal cameraVelocity As Vector3D)
        MyBase.New(classicRayTracer:=classicRayTracer, cameraVelocity:=cameraVelocity)
    End Sub

    Public Overrides Function GetLight(ByVal viewRay As Math.Ray) As TLight
        Dim transformedRay = _RayTransformation.GetViewRayInS(viewRayInT:=viewRay)

        Dim transformedIntensity = _RayTransformation.GetSpectralRadianceInT(viewRayInS:=viewRay, spectralRadianceInS:=1)

        Return _ClassicRayTracer.GetLight(transformedRay).MultiplyBrightness(transformedIntensity)
    End Function

End Class
