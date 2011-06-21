Public Class RayTracerTests

    <Test()>
    Public Sub Reflection()
        Dim reflectingSphere = New SingleMaterialSurface(Of Material2D)(New Sphere(center:=New Vector3D(-1, 0, 0), radius:=1),
                                                         material:=New Material2D(sourceLight:=New ExactColor(0, 0, 1),
                                                                                  scatteringRemission:=New BlackColorRemission,
                                                                                  reflectionRemission:=New BlackColorRemission,
                                                                                  transparencyRemission:=New BlackColorRemission))

        Dim colorSphere = New SingleMaterialSurface(Of Material2D)(New Sphere(center:=New Vector3D(3, -3, 0), radius:=1),
                                                         material:=Materials2D.LightSource(sourceLight:=New ExactColor(0, 0, 1)))

        Dim surfaces = New MaterialSurfaces(Of Material2D) From {reflectingSphere, colorSphere}
        Dim rayTracer = New Fusion.Ry.ScatteringRayTracer(surfaces)

        Dim startRay = New Ray(origin:=New Vector3D(1, 1, 0), direction:=New Vector3D(-1, -1, 0))
        Dim color = rayTracer.GetColor(startRay)
        Assert.AreEqual(0, color.Red)
        Assert.AreEqual(0, color.Green)
        Assert.AreEqual(1, color.Blue)
    End Sub

End Class
