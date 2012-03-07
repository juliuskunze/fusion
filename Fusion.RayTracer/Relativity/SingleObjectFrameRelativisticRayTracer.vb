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
        Dim sightRay = New SightRay(observerSightRayWithObjectOrigin)
        Dim objectSightRay = If(_Options.IgnoreGeometryEffect,
                                sightRay,
                                ObjectToObserver.Inverse.AtSightRay(sightRay).SemiTransformSightRay)
        Dim radianceSpectrum = ClassicRayTracer.GetLight(objectSightRay.Ray)

        Return ObjectToObserver.
            AtSightRay(objectSightRay).
            Partly(_Options).
            TransformRadianceSpectrum(radianceSpectrum)
    End Function

End Class
