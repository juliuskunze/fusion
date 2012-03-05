Public Class RelativisticRayTracer
    Inherits RelativisticRayTracerBase(Of RadianceSpectrum)

    Private ReadOnly _Transformation As RadianceSpectrumLorentzTransformation

    Public Sub New(classicRayTracer As IRayTracer(Of RadianceSpectrum),
                   observerVelocity As Vector3D,
                   options As RadianceSpectrumLorentzTransformationOptions)
        MyBase.New(classicRayTracer:=classicRayTracer, observerVelocity:=observerVelocity)
        _Transformation = New RadianceSpectrumLorentzTransformation(MyBase.LorentzTransformation, options:=options)
    End Sub

    Public Overrides Function GetLight(viewRay As Ray) As RadianceSpectrum
        Dim viewRayInS = InverseSemiTransformViewRay(viewRayInT:=viewRay)
        Dim spectralRadianceFunctionInS = ClassicRayTracer.GetLight(viewRayInS).Function

        Return New RadianceSpectrum(_Transformation.GetSpectralRadianceFunctionInT(viewRayInS:=viewRayInS, spectralRadianceFunctionInS:=spectralRadianceFunctionInS))
    End Function

    Private Function InverseSemiTransformViewRay(viewRayInT As Ray) As Ray
        If _Transformation.Options.IgnoreGeometryEffect Then Return viewRayInT

        Return LorentzTransformation.InverseSemiTransformViewRay(viewRayInTWithOriginInS:=viewRayInT)
    End Function

End Class
