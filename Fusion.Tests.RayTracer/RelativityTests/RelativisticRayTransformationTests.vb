Public Class RelativisticRayTransformationTests

    <Test()>
    Public Sub Test_Velocity0()
        Dim transformation = New RelativisticRayTransformation(relativeXVelocityInC:=0)
        Dim ray = New Ray(origin:=New Vector3D(0, 0, 0), direction:=New Vector3D(0, 1, 0))

        Dim transformedRay = transformation.TransformedRay(ray)

        Assert.That(ray.Origin = transformedRay.Origin)
        Assert.That(ray.NormalizedDirection = transformedRay.NormalizedDirection)
    End Sub

End Class
