Public Class RelativisticRadianceTransformationTests

    <Test()>
    Public Sub Velocity0()
        Dim transformation = New RelativisticRadianceTransformation(relativeVelocity:=New Vector3D)
        Dim ray = New Ray(origin:=New Vector3D, direction:=New Vector3D(0, 1, 0))

        Dim transformedRay = transformation.GetTransformedRay(ray)

        Assert.AreEqual(ray.Origin, transformedRay.Origin)
        Assert.AreEqual(ray.NormalizedDirection, transformedRay.NormalizedDirection)
        Assert.AreEqual(transformation.GetTransformedWavelength(ray, wavelength:=17), 17)
        Assert.AreEqual(transformation.GetTransformedSpectralRadiance(ray, intensity:=17), 17)

        Dim randomRay = New Ray(origin:=New Vector3D, direction:=New Vector3D(43, -12, 4))

        Assert.AreEqual(transformation.GetTransformedWavelength(randomRay, wavelength:=17), 17)
        Assert.AreEqual(transformation.GetTransformedSpectralRadiance(randomRay, intensity:=17), 17)
    End Sub

    <Test()>
    Public Sub PositiveVelocity()
        Dim transformation = New RelativisticRadianceTransformation(relativeVelocity:=New Vector3D(0.5 * Physics.Constants.SpeedOfLight, 0, 0))
        Assert.AreEqual(transformation.RelativeVelocity, New Vector3D(0.5 * Physics.Constants.SpeedOfLight, 0, 0))
        Assert.AreEqual(transformation.NormalizedRelativeVelocityDirection, New Vector3D(1, 0, 0))

        Dim frontRay = New Ray(origin:=New Vector3D, direction:=New Vector3D(1, 0, 0))
        Dim transformedFrontRay = transformation.GetTransformedRay(frontRay)
        Assert.AreEqual(frontRay.Origin, transformedFrontRay.Origin)
        Assert.AreEqual(frontRay.NormalizedDirection, transformedFrontRay.NormalizedDirection)

        Dim backRay = New Ray(origin:=New Vector3D, direction:=New Vector3D(1, 0, 0))
        Dim transformedBackRay = transformation.GetTransformedRay(backRay)
        Assert.AreEqual(backRay.Origin, transformedBackRay.Origin)
        Assert.AreEqual(backRay.NormalizedDirection, transformedBackRay.NormalizedDirection)
    End Sub

End Class
