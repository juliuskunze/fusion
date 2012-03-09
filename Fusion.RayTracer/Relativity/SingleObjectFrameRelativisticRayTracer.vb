Public Class SingleObjectFrameRelativisticRayTracer
    Inherits SingleReferenceFrameRelativisticRayTracerBase(Of RadianceSpectrum)

    Private ReadOnly _Options As LorentzTransformationAtSightRayOptions

    Public Sub New(classicRayTracer As IRayTracer(Of RadianceSpectrum),
                   observerVelocity As Vector3D,
                   options As LorentzTransformationAtSightRayOptions)
        MyBase.New(classicRayTracer:=classicRayTracer, observerVelocity:=observerVelocity)
        _Options = options
    End Sub

    Public Overrides Function GetLight(observerSightRayWithObjectOrigin As Ray) As RadianceSpectrum
        Dim observerSightRay = New SightRay(observerSightRayWithObjectOrigin)
        Dim observerToObjectAtSightRay = ObjectToObserver.Inverse.AtSightRay(observerSightRay).Partly(_Options)
        Dim objectSightRay = observerToObjectAtSightRay.SemiTransformSightRay
        Dim radianceSpectrum = ClassicRayTracer.GetLight(objectSightRay.Ray)

        Return observerToObjectAtSightRay.InversePartly.TransformRadianceSpectrum(radianceSpectrum)
    End Function

End Class
