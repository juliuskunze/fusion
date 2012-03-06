Public Class SingleObjectFrameRelativisticRayTracer
    Inherits SingleReferenceFrameRelativisticRayTracerBase(Of RadianceSpectrum)

    Private ReadOnly _Options As RadianceSpectrumLorentzTransformationOptions

    Public Sub New(classicRayTracer As IRayTracer(Of RadianceSpectrum),
                   observerVelocity As Vector3D,
                   options As RadianceSpectrumLorentzTransformationOptions)
        MyBase.New(classicRayTracer:=classicRayTracer, observerVelocity:=observerVelocity)
        _Options = options
    End Sub

    Public Overrides Function GetLight(observerSightRayWithObjectOrigin As Ray) As RadianceSpectrum
        Dim objectSightRay = If(_Options.IgnoreGeometryEffect,
                                observerSightRayWithObjectOrigin,
                                ObjectToObserver.Inverse.SemiTransformSightRay(observerSightRayWithObjectOrigin))
        Dim radianceSpectrum = ClassicRayTracer.GetLight(objectSightRay)

        Return ObjectToObserver.
            AtSightRayDirection(objectSightRay.NormalizedDirection).
            Partly(_Options).
            TransformRadianceSpectrum(radianceSpectrum)
    End Function

End Class
