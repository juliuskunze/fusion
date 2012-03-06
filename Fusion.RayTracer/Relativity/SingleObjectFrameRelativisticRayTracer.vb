Public Class SingleObjectFrameRelativisticRayTracer
    Inherits RelativisticRayTracerBase(Of RadianceSpectrum)

    Private ReadOnly _Options As RadianceSpectrumLorentzTransformationOptions

    Public Sub New(classicRayTracer As IRayTracer(Of RadianceSpectrum),
                   observerVelocity As Vector3D,
                   options As RadianceSpectrumLorentzTransformationOptions)
        MyBase.New(classicRayTracer:=classicRayTracer, observerVelocity:=observerVelocity)
        _Options = options
    End Sub

    Public Overrides Function GetLight(sightRay As Ray) As RadianceSpectrum
        Dim sightRayInS = If(_Options.IgnoreGeometryEffect,
                             sightRay,
                             LorentzTransformation.Inverse.SemiTransformSightRay(sightRayInTWithOriginInS:=sightRay))
        Dim radianceSpectrum = ClassicRayTracer.GetLight(sightRayInS)

        Return LorentzTransformation.
            AtSightRayDirection(sightRayInS.NormalizedDirection).
            Partly(_Options).
            TransformRadianceSpectrum(radianceSpectrum)
    End Function

End Class
