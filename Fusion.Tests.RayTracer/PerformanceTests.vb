Public Class PerformanceTests

    <Test()>
    Public Sub ExamplePicture()
        Dim startTime = DateTime.Now

        Dim picture = Me.IluminationRoom

        Dim neededTime = DateTime.Now - startTime

        Assert.Less(neededTime, TimeSpan.FromSeconds(0.8D), message:=neededTime.ToString)
    End Sub

    Public Function IluminationRoom() As RayTraceDrawer(Of RgbLight)
        Dim view = New View3D(observerLocation:=New Vector3D(15, 6, 33),
                              lookAt:=New Vector3D(3, 3, 0),
                              upVector:=New Vector3D(0, 1, 0),
                              horizontalViewAngle:=PI / 4)
        Dim origin = Vector3D.Zero
        Dim frontLeftDown = New Vector3D(0, 0, 6)
        Dim backLeftDown = New Vector3D(0, 0, 16)
        Dim backRightDown = New Vector3D(14, 0, 16)
        Dim frontRightDown = New Vector3D(14, 0, 0)
        Dim leftFrontDown = New Vector3D(8, 0, 0)
        Dim midDown = New Vector3D(8, 0, 6)

        Dim heightVector = New Vector3D(0, 10, 0)

        Dim originUp = origin + heightVector
        Dim frontLeftUp = frontLeftDown + heightVector
        Dim backLeftUp = backLeftDown + heightVector
        Dim backRightUp = backRightDown + heightVector
        Dim frontRightUp = frontRightDown + heightVector
        Dim leftFrontUp = leftFrontDown + heightVector
        Dim midUp = midDown + heightVector

        Dim lightMaterial = RgbLightMaterials2D.LightSource(RgbLight.White)
        Dim redMaterial = RgbLightMaterials2D.Scattering(New RgbLight(Color.Red))
        Dim whiteMaterial = RgbLightMaterials2D.Scattering(New RgbLight(Color.White))
        Dim greenMaterial = RgbLightMaterials2D.Scattering(New RgbLight(Color.Green))

        Dim glassRefractionIndex = 1.3

        Dim glass = RgbLightMaterials2D.Transparent(1 / glassRefractionIndex, reflectionAlbedo:=0.2)
        Dim innerGlass = RgbLightMaterials2D.TransparentInner(1 / glassRefractionIndex)

        Dim metal = RgbLightMaterials2D.Reflecting

        Dim groundMaterial1 = New Material2D(Of RgbLight)(sourceLight:=RgbLight.Black,
                             scatteringRemission:=New RgbLightRemission(Color.Blue),
                             reflectionRemission:=New ScaledRemission(Of RgbLight)(0.5),
                             transparencyRemission:=New BlackRemission(Of RgbLight))
        Dim groundMaterial2 = New Material2D(Of RgbLight)(sourceLight:=RgbLight.Black,
                                             scatteringRemission:=New RgbLightRemission(Color.White),
                                             reflectionRemission:=New ScaledRemission(Of RgbLight)(0.5),
                                             transparencyRemission:=New BlackRemission(Of RgbLight))
        Dim groundRectangle = New Fusion.Math.Rectangle(frontRightDown, origin, backLeftDown)

        Dim ground = New SquaredMaterialSurface(Of Material2D(Of RgbLight))(groundRectangle,
                                        squaresXVector:=New Vector3D(1, 0, 0),
                                        squaresYVector:=New Vector3D(0, 0, 1),
                                        squareLength:=1,
                                        material1:=groundMaterial1,
                                        material2:=groundMaterial2)

        Dim lightRectangle = New Fusion.Math.Rectangle(vertex1:=New Vector3D(10, 9.5, 10), vertex2:=New Vector3D(5, 9.5, 10),
                                                       vertex3:=New Vector3D(5, 9.5, 5))
        Dim light = New SingleMaterialSurface(Of Material2D(Of RgbLight))(lightRectangle, lightMaterial)

        Dim redWallPlane = New Fusion.Math.Rectangle(frontLeftDown, frontLeftUp, backLeftUp)
        Dim redWall = New SingleMaterialSurface(Of Material2D(Of RgbLight))(redWallPlane, redMaterial)

        Dim frontWallPlane1 = New Fusion.Math.Rectangle(frontLeftDown, midDown, midUp)
        Dim frontWallPlane2 = New Fusion.Math.Rectangle(midDown, leftFrontDown, leftFrontUp)
        Dim frontWallPlane3 = New Fusion.Math.Rectangle(leftFrontDown, frontRightDown, frontRightUp)

        Dim frontWallSurface = New Surfaces From {frontWallPlane1, frontWallPlane2, frontWallPlane3}
        Dim frontWall = New SingleMaterialSurface(Of Material2D(Of RgbLight))(frontWallSurface, whiteMaterial)

        Dim greenWallPlane = New Fusion.Math.Rectangle(frontRightUp, frontRightDown, backRightDown)
        Dim greenWall = New SingleMaterialSurface(Of Material2D(Of RgbLight))(greenWallPlane, greenMaterial)

        Dim backWallPlane = New Fusion.Math.Rectangle(backRightDown, backLeftDown, backLeftUp)
        Dim backWall = New SingleMaterialSurface(Of Material2D(Of RgbLight))(backWallPlane, whiteMaterial)

        Dim pointLightSource = New LinearPointLightSource(Of RgbLight)(Location:=New Vector3D(6, 9.5, 10), baseLight:=RgbLight.White * 5)
        Dim shadedLightSources = New List(Of IPointLightSource(Of RgbLight)) From {pointLightSource}

        Dim ceilingPlane = New Fusion.Math.Rectangle(backLeftUp, originUp, frontRightUp)
        Dim ceiling = New SingleMaterialSurface(Of Material2D(Of RgbLight))(ceilingPlane, whiteMaterial)

        Dim scatteringSphereRadius = 0.75
        Dim scatteringSphereCenter = backRightDown + New Vector3D(-1.5, scatteringSphereRadius, -2.5)
        Dim scatteringSphereSurface = New Sphere(scatteringSphereCenter, scatteringSphereRadius)
        Dim scatteringSphere = New SingleMaterialSurface(Of Material2D(Of RgbLight))(scatteringSphereSurface, whiteMaterial)

        Dim metalSphereRadius = 1.5
        Dim metalSphereCenter = midDown + New Vector3D(0, metalSphereRadius, 3)
        Dim metalSphereSurface = New Sphere(metalSphereCenter, metalSphereRadius)
        Dim metalSphere = New SingleMaterialSurface(Of Material2D(Of RgbLight))(metalSphereSurface, metal)

        Dim glassLocation = backLeftDown + New Vector3D(3, 0, -4.5)
        Dim glassCylinderHeight = 3

        Dim glassSphereRadius = 1.5
        Dim glassSphereCenter = glassLocation + New Vector3D(0, glassCylinderHeight + glassSphereRadius, 0)
        Dim glassSphereSurface = New Sphere(glassSphereCenter, glassSphereRadius)
        Dim glassSphere = New SingleMaterialSurface(Of Material2D(Of RgbLight))(glassSphereSurface, glass)
        Dim glassAntiSphereSurface = New AntiSphere(glassSphereSurface)
        Dim glassAntiSphere = New SingleMaterialSurface(Of Material2D(Of RgbLight))(glassAntiSphereSurface, innerGlass)

        Dim surfaces = New Surfaces(Of Material2D(Of RgbLight)) From {ground, redWall, frontWall, greenWall, backWall, ceiling, frontWall, light, scatteringSphere, glassSphere, glassAntiSphere, metalSphere}
        Dim rayTracer = New RecursiveRayTracer(Of RgbLight)(surface:=surfaces, unshadedLightSource:=New LightSources(Of RgbLight), shadedPointLightSources:=shadedLightSources, maxIntersectionCount:=10)
        'Dim rayTracer = New ScatteringRayTracer(surface:=surfaces, rayCount:=1, maxIntersectionCount:=10)

        Dim glassCylinderSurface = New Cylinder(startCenter:=glassLocation, endCenter:=glassLocation + New Vector3D(0, glassCylinderHeight, 0), radius:=0.1)


        Return New RayTraceDrawer(Of RgbLight)(rayTracer:=rayTracer, pictureSize:=New Size(100, 100), view:=view, lightToColorConverter:=New RgbLightToColorConverter)
    End Function

End Class
