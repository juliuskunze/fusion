Public Class RelativisticRayTracer
    Inherits RelativisticRayTracerBase(Of RadianceSpectrum)

    Private ReadOnly _IgnoreGeometryEffect As Boolean
    Private ReadOnly _IgnoreDopplerEffect As Boolean
    Private ReadOnly _IgnoreSearchlightEffect As Boolean

    Public Sub New(ByVal classicRayTracer As IRayTracer(Of RadianceSpectrum),
                   ByVal cameraVelocity As Vector3D,
                   Optional ByVal ignoreGeometryEffect As Boolean = False,
                   Optional ByVal ignoreDopplerEffect As Boolean = False,
                   Optional ByVal ignoreSearchlightEffect As Boolean = False)
        MyBase.New(classicRayTracer:=classicRayTracer, cameraVelocity:=cameraVelocity)
        _IgnoreGeometryEffect = ignoreGeometryEffect
        _IgnoreDopplerEffect = ignoreDopplerEffect
        _IgnoreSearchlightEffect = ignoreSearchlightEffect
    End Sub

    Public Overrides Function GetLight(ByVal viewRay As Ray) As RadianceSpectrum
        Dim viewRayInS = _RayTransformation.GetViewRayInS(viewRayInT:=viewRay)

        Return New RadianceSpectrum(_RayTransformation.GetSpectralRadianceFunctionInT(viewRayInS:=viewRayInS,
                                                                                      spectralRadianceFunctionInS:=_ClassicRayTracer.GetLight(viewRayInS).Function))
    End Function



End Class
