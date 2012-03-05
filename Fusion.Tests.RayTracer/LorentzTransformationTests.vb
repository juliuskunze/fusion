Public Class LorentzTransformationTests
    Private Const c = Constants.SpeedOfLight

    Private ReadOnly _NullTransformation As New LorentzTransformation(relativeVelocity:=New Vector3D)

    <Test()>
    Public Sub Velocity0()

        Dim sightRayInT = New Ray(origin:=New Vector3D, direction:=New Vector3D(0, 1, 0))

        Dim sightRayInS = _NullTransformation.InverseSemiTransformSightRay(sightRayInTWithOriginInS:=sightRayInT)

        Assert.AreEqual(sightRayInT.Origin, sightRayInS.Origin)
        Assert.AreEqual(sightRayInT.NormalizedDirection, sightRayInS.NormalizedDirection)
        Assert.AreEqual(_NullTransformation.Inverse.AtDirection(sightRayInT.NormalizedDirection).TransformWavelength(wavelength:=17), 17)
        Assert.AreEqual(_NullTransformation.AtDirection(sightRayInT.NormalizedDirection).TransformSpectralRadiance(spectralRadiance:=17), 17)

        Dim randomRayInS = New Ray(origin:=New Vector3D, direction:=New Vector3D(43, -12, 4))

        Assert.AreEqual(_NullTransformation.Inverse.AtDirection(randomRayInS.NormalizedDirection).TransformWavelength(wavelength:=17), 17)
        Assert.AreEqual(_NullTransformation.AtDirection(randomRayInS.NormalizedDirection).TransformSpectralRadiance(spectralRadiance:=17), 17)

        Dim randomEventInS = New SpaceTimeEvent(5, New Vector3D(1, 2, 3))

        Assert.AreEqual(_NullTransformation.TransformEvent(randomEventInS), randomEventInS)

        Dim randomVelocityInS = New Vector3D(1, 2, 3)

        Assert.AreEqual(_NullTransformation.TransformVelocity(randomVelocityInS), randomVelocityInS)
    End Sub

    Private Const beta = 0.5
    Private ReadOnly gamma As Double = 1 / Sqrt(1 - beta ^ 2)
    Private Const v = beta * c
    Private _RelativeVelocity As New Vector3D(v, 0, 0)
    Private ReadOnly _Transformation As New LorentzTransformation(relativeVelocity:=_RelativeVelocity)
    Private ReadOnly _VectorComparer As New Vector3DRoughComparer(10 ^ -15)

    <Test()>
    Public Sub PositiveVelocity()
        Assert.AreEqual(_Transformation.RelativeVelocity, _RelativeVelocity)
        Assert.AreEqual(_Transformation.NormalizedRelativeVelocity, _RelativeVelocity.Normalized)
        Assert.AreEqual(_Transformation.Beta, beta)
        Assert.AreEqual(_Transformation.Gamma, gamma)

        Dim frontRayInT = New Ray(origin:=New Vector3D, direction:=New Vector3D(1, 0, 0))
        Dim frontRayInS = _Transformation.InverseSemiTransformSightRay(frontRayInT)
        Assert.AreEqual(frontRayInT.Origin, frontRayInS.Origin)
        Assert.That(_VectorComparer.Equals(frontRayInT.NormalizedDirection, frontRayInS.NormalizedDirection))

        Dim backRay = New Ray(origin:=New Vector3D, direction:=New Vector3D(1, 0, 0))
        Dim transformedBackRay = _Transformation.InverseSemiTransformSightRay(backRay)
        Assert.AreEqual(backRay.Origin, transformedBackRay.Origin)
        Assert.That(_VectorComparer.Equals(backRay.NormalizedDirection, transformedBackRay.NormalizedDirection))

        Const testWavelengthInT = 0.0000005
        Dim testRayInT = New Ray(New Vector3D, New Vector3D(1, 1, 1))
        Assert.Less(testWavelengthInT, _Transformation.Inverse.AtDirection(frontRayInS.NormalizedDirection).TransformWavelength(wavelength:=testWavelengthInT))
        Assert.Greater(testRayInT.NormalizedDirection.X, _Transformation.InverseSemiTransformSightRay(sightRayInTWithOriginInS:=testRayInT).NormalizedDirection.X)

        Const testIntensityInS = 17
        Assert.Greater(_Transformation.AtDirection(frontRayInS.NormalizedDirection).TransformSpectralRadiance(spectralRadiance:=testIntensityInS), testIntensityInS)

        Dim originEvent = New SpaceTimeEvent(0, New Vector3D)

        Assert.AreEqual(_Transformation.TransformEvent(originEvent), originEvent)

        Const t = 10
        Dim spaceOriginEvent = New SpaceTimeEvent(t, New Vector3D)

        Assert.AreEqual(_Transformation.TransformEvent(spaceOriginEvent), New SpaceTimeEvent(gamma * t, New Vector3D(-gamma * t * v, 0, 0)))

        Const x = 10
        Dim timeOriginEvent = New SpaceTimeEvent(0, New Vector3D(x, 0, 0))

        Assert.AreEqual(_Transformation.TransformEvent(timeOriginEvent), New SpaceTimeEvent(-gamma * v * x / c ^ 2, New Vector3D(gamma * x, 0, 0)))

    End Sub

    <Test()>
    Public Sub TestInverse()
        Dim softVectorComparer As New Vector3DRoughComparer(10 ^ -8)

        Dim randomVelocityInS = New Vector3D(0, -3, 0)

        Assert.That(softVectorComparer.Equals(_Transformation.Inverse.TransformVelocity(_Transformation.TransformVelocity(randomVelocityInS)), randomVelocityInS))
        Assert.AreEqual(_Transformation.Inverse.Before(_Transformation).TransformVelocity(randomVelocityInS), randomVelocityInS)
        Assert.AreEqual(_Transformation.Before(_Transformation.Inverse).TransformVelocity(randomVelocityInS), randomVelocityInS)
    End Sub

    <Test()>
    Public Sub TransformedVelocity_And_Before()
        Const ux = 10000
        Dim parallelVelocity = New Vector3D(ux, 0, 0)

        Dim transformedVelocity = _Transformation.TransformVelocity(parallelVelocity)

        Assert.AreEqual(transformedVelocity, New Vector3D((ux - v) / (1 - v * ux / c ^ 2), 0, 0))
        Assert.AreEqual(transformedVelocity, _Transformation.Before(_NullTransformation).TransformVelocity(parallelVelocity))
        Assert.AreEqual(transformedVelocity, _NullTransformation.Before(_Transformation).TransformVelocity(parallelVelocity))

        Const uy = 10
        Dim orthogonalVelocity = New Vector3D(0, uy, 0)

        Assert.AreEqual(_Transformation.TransformVelocity(orthogonalVelocity), New Vector3D(-v, uy / gamma, 0))

        Dim parallelLightVelocity = New Vector3D(c, 0, 0)

        Assert.AreEqual(_Transformation.TransformVelocity(parallelLightVelocity), parallelLightVelocity)

        Dim antiParallelLightVelocity = New Vector3D(-c, 0, 0)

        Assert.AreEqual(_Transformation.TransformVelocity(antiParallelLightVelocity), antiParallelLightVelocity)

        Dim orthogonalLightVelocity = New Vector3D(0, c, 0)

        Assert.AreEqual(_Transformation.TransformVelocity(orthogonalLightVelocity).Length, c)
    End Sub
End Class
