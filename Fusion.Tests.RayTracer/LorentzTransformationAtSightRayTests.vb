Public Class LorentzTransformationAtSightRayTests

    Private Const c = Fusion.Physics.Constants.SpeedOfLight

    Private Const _RandomWavelength = 700 * 10 ^ -9
    Private Const _RandomSpectralRadiance = 0.37

    Private ReadOnly _SightRay As New Ray(New Vector3D, New Vector3D(0, 1, 0))
    Private ReadOnly _T As New LorentzTransformationAtSightRay(relativeVelocity:=New Vector3D(c / 2, 0, 0), SightRay:=_SightRay)

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
        Assert.That(New DoubleRoughComparer(10 ^ -10).Equals(_T.GammaTheta, 1 / _Inverse.GammaTheta))
    End Sub

    Private ReadOnly _Parallel As New LorentzTransformationAtSightRay(relativeVelocity:=New Vector3D(c / 2, 0, 0), SightRay:=New SightRay(New Ray(New Vector3D, New Vector3D(1, 0, 0))))

    <Test()>
    Public Sub Parallel()
        Assert.Greater(_RandomWavelength, _Parallel.TransformWavelength(_RandomWavelength))
    End Sub
End Class
