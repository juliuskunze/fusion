Public Class RelativisticRadianceTransformationTests

    <Test()>
    Public Sub Velocity0()
        Dim transformation = New RelativisticRadianceTransformation(relativeVelocityOfTInS:=New Vector3D)
        Dim viewRayInT = New Ray(origin:=New Vector3D, direction:=New Vector3D(0, 1, 0))

        Dim viewRayInS = transformation.GetViewRayInS(viewRayInT:=viewRayInT)

        Assert.AreEqual(viewRayInT.Origin, viewRayInS.Origin)
        Assert.AreEqual(viewRayInT.NormalizedDirection, viewRayInS.NormalizedDirection)
        Assert.AreEqual(transformation.GetWavelengthInS(viewRayInT, wavelengthInT:=17), 17)
        Assert.AreEqual(transformation.GetSpectralRadianceInT(viewRayInT, spectralRadianceInS:=17), 17)

        Dim randomRayInS = New Ray(origin:=New Vector3D, direction:=New Vector3D(43, -12, 4))

        Assert.AreEqual(transformation.GetWavelengthInS(randomRayInS, wavelengthInT:=17), 17)
        Assert.AreEqual(transformation.GetSpectralRadianceInT(randomRayInS, spectralRadianceInS:=17), 17)
    End Sub

    <Test()>
    Public Sub PositiveVelocity()
        Dim transformation = New RelativisticRadianceTransformation(relativeVelocityOfTInS:=New Vector3D(0.5 * Physics.Constants.SpeedOfLight, 0, 0))
        Assert.AreEqual(transformation.RelativeVelocity, New Vector3D(0.5 * Physics.Constants.SpeedOfLight, 0, 0))
        Assert.AreEqual(transformation.NormalizedRelativeVelocityDirection, New Vector3D(1, 0, 0))

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
    End Sub

End Class
