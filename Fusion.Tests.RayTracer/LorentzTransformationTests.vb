Public Class LorentzTransformationTests
    Private Const c = Constants.SpeedOfLight

    <Test()>
    Public Sub Velocity0()
        Dim nullTransformation = New LorentzTransformation(relativeVelocity:=New Vector3D)
        Dim viewRayInT = New Ray(origin:=New Vector3D, direction:=New Vector3D(0, 1, 0))

        Dim viewRayInS = nullTransformation.InverseSemiTransformViewRay(viewRayInTWithOriginInS:=viewRayInT)

        Assert.AreEqual(viewRayInT.Origin, viewRayInS.Origin)
        Assert.AreEqual(viewRayInT.NormalizedDirection, viewRayInS.NormalizedDirection)
        Assert.AreEqual(nullTransformation.GetWavelengthInS(viewRayInT, wavelengthInT:=17), 17)
        Assert.AreEqual(nullTransformation.GetSpectralRadianceInT(viewRayInT, spectralRadianceInS:=17), 17)

        Dim randomRayInS = New Ray(origin:=New Vector3D, direction:=New Vector3D(43, -12, 4))

        Assert.AreEqual(nullTransformation.GetWavelengthInS(randomRayInS, wavelengthInT:=17), 17)
        Assert.AreEqual(nullTransformation.GetSpectralRadianceInT(randomRayInS, spectralRadianceInS:=17), 17)

        Dim randomEventInS = New SpaceTimeEvent(5, New Vector3D(1, 2, 3))

        Assert.AreEqual(nullTransformation.TransformEvent(randomEventInS), randomEventInS)

        Dim randomVelocityInS = New Vector3D(1, 2, 3)

        Assert.AreEqual(nullTransformation.TransformVelocity(randomVelocityInS), randomVelocityInS)
    End Sub

    Private Const beta = 0.5
    Private ReadOnly gamma As Double = 1 / Sqrt(1 - beta ^ 2)
    Private Const v = beta * c
    Private _RelativeVelocity As New Vector3D(v, 0, 0)
    Private ReadOnly _Transformation As New LorentzTransformation(relativeVelocity:=_RelativeVelocity)

    <Test()>
    Public Sub PositiveVelocity()
        Assert.AreEqual(_Transformation.RelativeVelocity, _RelativeVelocity)
        Assert.AreEqual(_Transformation.NormalizedRelativeVelocity, _RelativeVelocity.Normalized)
        Assert.AreEqual(_Transformation.Beta, beta)
        Assert.AreEqual(_Transformation.Gamma, gamma)

        Dim frontRayInT = New Ray(origin:=New Vector3D, direction:=New Vector3D(1, 0, 0))
        Dim frontRayInS = _Transformation.InverseSemiTransformViewRay(frontRayInT)
        Assert.AreEqual(frontRayInT.Origin, frontRayInS.Origin)
        Assert.AreEqual(frontRayInT.NormalizedDirection, frontRayInS.NormalizedDirection)

        Dim backRay = New Ray(origin:=New Vector3D, direction:=New Vector3D(1, 0, 0))
        Dim transformedBackRay = _Transformation.InverseSemiTransformViewRay(backRay)
        Assert.AreEqual(backRay.Origin, transformedBackRay.Origin)
        Assert.AreEqual(backRay.NormalizedDirection, transformedBackRay.NormalizedDirection)

        Const testWavelengthInT = 0.0000005
        Dim testRayInT = New Ray(New Vector3D, New Vector3D(1, 1, 1))
        Assert.Less(testWavelengthInT, _Transformation.GetWavelengthInS(viewRayInS:=frontRayInS, wavelengthInT:=testWavelengthInT))
        Assert.Greater(testRayInT.NormalizedDirection.X, _Transformation.InverseSemiTransformViewRay(viewRayInTWithOriginInS:=testRayInT).NormalizedDirection.X)

        Const testIntensityInS = 17
        Assert.Greater(_Transformation.GetSpectralRadianceInT(viewRayInS:=frontRayInS, spectralRadianceInS:=testIntensityInS), testIntensityInS)

        Dim originEvent = New SpaceTimeEvent(0, New Vector3D)

        Assert.AreEqual(_Transformation.TransformEvent(originEvent), originEvent)

        Const t = 10
        Dim spaceOriginEvent = New SpaceTimeEvent(t, New Vector3D)

        Assert.AreEqual(_Transformation.TransformEvent(spaceOriginEvent), New SpaceTimeEvent(gamma * t, New Vector3D(-gamma * t * v, 0, 0)))

        Const x = 10
        Dim timeOriginEvent = New SpaceTimeEvent(0, New Vector3D(x, 0, 0))

        Assert.AreEqual(_Transformation.TransformEvent(timeOriginEvent), New SpaceTimeEvent(-gamma * v * x / c ^ 2, New Vector3D(gamma * x, 0, 0)))

        Const ux = 10
        Dim parallelVelocity = New Vector3D(ux, 0, 0)

        Assert.AreEqual(_Transformation.TransformVelocity(parallelVelocity), New Vector3D((ux - v) / (1 - v * ux / c ^ 2), 0, 0))

        Const uy = 10
        Dim orthogonalVelocity = New Vector3D(0, uy, 0)

        Assert.AreEqual(_Transformation.TransformVelocity(orthogonalVelocity), New Vector3D(-v, uy / gamma, 0))
    End Sub

    <Test()>
    Public Sub TestInverse()
        Dim randomVelocityInS = New Vector3D(1, 2, 3)

        Assert.That(New Vector3DRoughComparer(10 ^ -8).Equals(_Transformation.Inverse.TransformVelocity(_Transformation.TransformVelocity(randomVelocityInS)), randomVelocityInS))
    End Sub

    <Test()>
    Public Sub TestVelocityDirection()
        Dim randomVelocityInS = New Vector3D(-10, 20, -30)

        Assert.AreEqual(_Transformation.TransformVelocity(randomVelocityInS.ScaledToLength(c)).Normalized, _Transformation.Inverse.InverseTransformLightDirection(randomVelocityInS.Normalized))
    End Sub

End Class
