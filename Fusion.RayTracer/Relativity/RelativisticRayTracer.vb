Public Class RelativisticRayTracer
    Inherits RelativisticRayTracerBase(Of RadianceSpectrum)

    Private ReadOnly _Transformation As RadianceSpectrumLorentzTransformation

    Public Sub New(classicRayTracer As IRayTracer(Of RadianceSpectrum),
                   observerVelocity As Vector3D,
                   options As RadianceSpectrumLorentzTransformationOptions)
        MyBase.New(classicRayTracer:=classicRayTracer, observerVelocity:=observerVelocity)
        _Transformation = New RadianceSpectrumLorentzTransformation(MyBase.LorentzTransformation, options:=options)
    End Sub

    Public Overrides Function GetLight(sightRay As Ray) As RadianceSpectrum
        Dim sightRayInS = InverseSemiTransformSightRay(sightRayInT:=sightRay)
        Dim spectralRadianceFunctionInS = ClassicRayTracer.GetLight(sightRayInS).Function

        Return New RadianceSpectrum(_Transformation.TransformSpectralRadianceFunction(normalizedSightRayDirectionInS:=sightRayInS.NormalizedDirection, spectralRadianceFunction:=spectralRadianceFunctionInS))
    End Function

    Private Function InverseSemiTransformSightRay(sightRayInT As Ray) As Ray
        If _Transformation.Options.IgnoreGeometryEffect Then Return sightRayInT

        Return LorentzTransformation.InverseSemiTransformSightRay(sightRayInTWithOriginInS:=sightRayInT)
    End Function

End Class
