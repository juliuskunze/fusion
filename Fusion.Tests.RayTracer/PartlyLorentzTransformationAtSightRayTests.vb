Public Class PartlyLorentzTransformationAtSightRayTests
    Private Const c = Constants.SpeedOfLight

    Private Const _RandomWavelength = 700 * 10 ^ -9
    Private Const _RandomSpectralRadiance = 0.37

    Const _Beta = 0.5
    Private ReadOnly _SightRay As New Ray(New Vector3D, New Vector3D(0, 1, 0))
    Private ReadOnly _T As New PartialLorentzTransformationAtSightRay(relativeVelocity:=New Vector3D(c * _Beta, 0, 0), SightRay:=New SightRay(_SightRay),
                                                                     options:=New LorentzTransformationAtSightRayOptions(ignoreAberrationEffect:=True))

    Private ReadOnly _Inverse As LorentzTransformationAtSightRay = _T.InverseAtSightRay

    <Test()>
    Public Sub Inverse()
        Assert.That(Abs(_RandomWavelength - _Inverse.TransformWavelength(_T.TransformWavelength(_RandomWavelength))) < 10 ^ -10)
        Assert.That(Abs(_RandomSpectralRadiance - _Inverse.TransformSpectralRadiance(_T.TransformSpectralRadiance(_RandomSpectralRadiance))) < 10 ^ -10)
    End Sub

    <Test()>
    Public Sub InverseRay()
        Assert.That(New RayRoughComparer(10 ^ -10).Equals(_SightRay, _Inverse.TransformSightRay.Ray))
    End Sub

    <Test()>
    Public Sub GammaTheta()
        Assert.That(New RoughDoubleComparer(10 ^ -10).Equals(_T.GammaTheta, 1 / _Inverse.GammaTheta))
    End Sub

    Private ReadOnly _Parallel As New PartialLorentzTransformationAtSightRay(relativeVelocity:=New Vector3D(c / 2, 0, 0), SightRay:=New SightRay(New Ray(New Vector3D, New Vector3D(1, 0, 0))), options:=New LorentzTransformationAtSightRayOptions(ignoreAberrationEffect:=True))

    <Test()>
    Public Sub Parallel()
        Assert.That(New RoughDoubleComparer(10 ^ -12).Equals(Sqrt((1 - _Beta) / (1 + _Beta)) * _RandomWavelength, _Parallel.TransformWavelength(_RandomWavelength)))
    End Sub
End Class
