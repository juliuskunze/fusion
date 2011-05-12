Public Class RelativisticRayTransformationTests

    <Test()>
    Public Sub Velocity0()
        Dim transformation = New RelativisticRayTransformation(relativeXVelocityInC:=0)
        Dim ray = New Ray(origin:=New Vector3D(0, 0, 0), direction:=New Vector3D(0, 1, 0))

        Dim transformedRay = transformation.TransformedRay(ray)

        Assert.That(ray.Origin = transformedRay.Origin)
        Assert.That(ray.NormalizedDirection = transformedRay.NormalizedDirection)
        Assert.AreEqual(transformation.TransformedWavelength(ray, waveLength:=17), 17)
        Assert.AreEqual(transformation.TransformedIntensity(ray, intensity:=17), 17)

        Dim randomRay = New Ray(origin:=New Vector3D(0, 0, 0), direction:=New Vector3D(43, -12, 4))

        Assert.AreEqual(transformation.TransformedWavelength(randomRay, waveLength:=17), 17)
        Assert.AreEqual(transformation.TransformedIntensity(randomRay, intensity:=17), 17)
    End Sub

    <Test()>
    Public Sub PositiveVelocity()

    End Sub

End Class
