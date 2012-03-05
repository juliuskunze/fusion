Public Class LorentzTransformationTests
    Private Const c = Constants.SpeedOfLight

    <Test()>
    Public Sub Velocity0()
        Dim transformation = New LorentzTransformation(relativeVelocityOfTInS:=New Vector3D)
        Dim viewRayInT = New Ray(origin:=New Vector3D, direction:=New Vector3D(0, 1, 0))

        Dim viewRayInS = transformation.GetViewRayInS(viewRayInT:=viewRayInT)

        Assert.AreEqual(viewRayInT.Origin, viewRayInS.Origin)
        Assert.AreEqual(viewRayInT.NormalizedDirection, viewRayInS.NormalizedDirection)
        Assert.AreEqual(transformation.GetWavelengthInS(viewRayInT, wavelengthInT:=17), 17)
        Assert.AreEqual(transformation.GetSpectralRadianceInT(viewRayInT, spectralRadianceInS:=17), 17)

        Dim randomRayInS = New Ray(origin:=New Vector3D, direction:=New Vector3D(43, -12, 4))

        Assert.AreEqual(transformation.GetWavelengthInS(randomRayInS, wavelengthInT:=17), 17)
        Assert.AreEqual(transformation.GetSpectralRadianceInT(randomRayInS, spectralRadianceInS:=17), 17)

        Dim randomEventInS = New SpaceTimeEvent(5, New Vector3D(1, 2, 3))

        Assert.AreEqual(transformation.GetEventInT(randomEventInS), randomEventInS)
    End Sub

    <Test()>
    Public Sub PositiveVelocity()
        Const beta = 0.5
        Dim gamma = 1 / Sqrt(1 - beta ^ 2)
        Dim v = New Vector3D(beta * c, 0, 0)
        Dim transformation = New LorentzTransformation(relativeVelocityOfTinS:=v)
        Assert.AreEqual(transformation.RelativeVelocity, v)
        Assert.AreEqual(transformation.NormalizedRelativeVelocity, v.Normalized)
        Assert.AreEqual(transformation.Beta, beta)
        Assert.AreEqual(transformation.Gamma, gamma)

        Dim frontRayInT = New Ray(origin:=New Vector3D, direction:=New Vector3D(1, 0, 0))
        Dim frontRayInS = transformation.GetViewRayInS(frontRayInT)
        Assert.AreEqual(frontRayInT.Origin, frontRayInS.Origin)
        Assert.AreEqual(frontRayInT.NormalizedDirection, frontRayInS.NormalizedDirection)

        Dim backRay = New Ray(origin:=New Vector3D, direction:=New Vector3D(1, 0, 0))
        Dim transformedBackRay = transformation.GetViewRayInS(backRay)
        Assert.AreEqual(backRay.Origin, transformedBackRay.Origin)
        Assert.AreEqual(backRay.NormalizedDirection, transformedBackRay.NormalizedDirection)

        Const testWavelengthInT = 0.0000005
        Dim testRayInT = New Ray(New Vector3D, New Vector3D(1, 1, 1))
        Assert.Less(testWavelengthInT, transformation.GetWavelengthInS(viewRayInS:=frontRayInS, wavelengthInT:=testWavelengthInT))
        Assert.Greater(testRayInT.NormalizedDirection.X, transformation.GetViewRayInS(viewRayInT:=testRayInT).NormalizedDirection.X)

        Const testIntensityInS = 17
        Assert.Greater(transformation.GetSpectralRadianceInT(viewRayInS:=frontRayInS, spectralRadianceInS:=testIntensityInS), testIntensityInS)

        Dim originEvent = New SpaceTimeEvent(0, New Vector3D)

        Assert.AreEqual(transformation.GetEventInT(originEvent), originEvent)

        Const t = 10
        Dim spaceOriginEvent = New SpaceTimeEvent(t, New Vector3D)

        Assert.AreEqual(transformation.GetEventInT(spaceOriginEvent), New SpaceTimeEvent(gamma * t, -gamma * t * v))

        Const x = 10
        Dim timeOriginEvent = New SpaceTimeEvent(0, New Vector3D(x, 0, 0))

        Assert.AreEqual(transformation.GetEventInT(timeOriginEvent), New SpaceTimeEvent(-gamma * v.Length * x / c ^ 2, New Vector3D(gamma * x, 0, 0)))
    End Sub

End Class
