Public Class RelativisticRayTracer
    Inherits RelativisticRayTracerBase(Of RadianceSpectrum)

    Private ReadOnly _IgnoreGeometryEffect As Boolean
    Private ReadOnly _IgnoreDopplerEffect As Boolean
    Private ReadOnly _IgnoreSearchlightEffect As Boolean

    Public Sub New(classicRayTracer As IRayTracer(Of RadianceSpectrum),
                    observerVelocity As Vector3D,
                   Optional ignoreGeometryEffect As Boolean = False,
                   Optional ignoreDopplerEffect As Boolean = False,
                   Optional ignoreSearchlightEffect As Boolean = False)
        MyBase.New(classicRayTracer:=classicRayTracer, observerVelocity:=observerVelocity)
        _IgnoreGeometryEffect = ignoreGeometryEffect
        _IgnoreDopplerEffect = ignoreDopplerEffect
        _IgnoreSearchlightEffect = ignoreSearchlightEffect
    End Sub

    Public Overrides Function GetLight(viewRay As Ray) As RadianceSpectrum
        Dim viewRayInS = Me.GetViewRayInS(viewRayInT:=viewRay)
        Dim spectralRadianceFunctionInS = _ClassicRayTracer.GetLight(viewRayInS).Function

        Return New RadianceSpectrum(Me.GetSpectralRadianceFunctionInT(viewRayInS:=viewRayInS, spectralRadianceFunctionInS:=spectralRadianceFunctionInS))
    End Function

    Private Function GetViewRayInS(viewRayInT As Ray) As Ray
        If _IgnoreGeometryEffect Then Return viewRayInT

        Return _RayTransformation.InverseSemiTransformViewRay(viewRayInTWithOriginInS:=viewRayInT)
    End Function

    Private Function GetSpectralRadianceFunctionInT(viewRayInS As Ray, spectralRadianceFunctionInS As SpectralRadianceFunction) As SpectralRadianceFunction
        If _IgnoreDopplerEffect AndAlso _IgnoreSearchlightEffect Then Return spectralRadianceFunctionInS
        If _IgnoreSearchlightEffect Then Return Function(wavelengthInT) spectralRadianceFunctionInS(_RayTransformation.GetWavelengthInS(viewRayInS:=viewRayInS, wavelengthInT:=wavelengthInT))
        If _IgnoreDopplerEffect Then Return Function(wavelengthInT) _RayTransformation.GetSpectralRadianceInT(viewRayInS:=viewRayInS, spectralRadianceInS:=spectralRadianceFunctionInS(wavelengthInT))

        Return _RayTransformation.GetSpectralRadianceFunctionInT(viewRayInS:=viewRayInS, spectralRadianceFunctionInS:=spectralRadianceFunctionInS)
    End Function

End Class
