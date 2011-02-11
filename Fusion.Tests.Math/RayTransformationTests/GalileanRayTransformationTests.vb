Public Class GalileanRayTransformationTests

    <Test()>
    Public Sub Test()
        Dim transformation = New RelativisticRayTransformation(relativeXVelocityInC:=10)
        Dim ray = New Ray(origin:=New Vector3D(0, 0, 0), direction:=New Vector3D(0, 1, 0))

        Dim transformedRay = transformation.TransformedRay(ray)

        Assert.That(ray.Origin = transformedRay.Origin)
        Assert.That(transformedRay.NormalizedDirection = New Vector3D(-10, 1, 0).Normalized)
    End Sub

End Class
