Public Class LensVideo
    Implements IRayTraceVideo

    Private _videoSize As Size
    Public ReadOnly Property VideoSize As Size
        Get
            Return _videoSize
        End Get
    End Property

    Public Sub New(ByVal videoSize As Size)
        _videoSize = videoSize
    End Sub

    Public Function RoomDrawer(ByVal glassRefractionIndex As Double) As RayTraceDrawer
        Dim view = New View3D(cameraLocation:=New Vector3D(5, 5, 25),
                              lookAt:=New Vector3D(5, 5, 0),
                              upVector:=New Vector3D(0, 1, 0),
                              xAngleFromMinus1To1:=PI / 3)
        Dim lamp = New LinearPointLightSource(Location:=New Vector3D(5, 9.9000000000000004, 8), colorAtDistance1:=ExactColor.White * 5)

        Dim ground = New SingleMaterialSurface(Of Material2D)(New Plane(Location:=Vector3D.Zero, normal:=New Vector3D(0, 1, 0)),
                                               material:=New Material2D(
                                                   scatteringRemission:=New ComponentScaledColorRemission(Color.Gray),
                                                   reflectionRemission:=New BlackColorRemission,
                                                   transparencyRemission:=New BlackColorRemission))

        Dim rightWall = New SingleMaterialSurface(Of Material2D)(New Plane(Location:=New Vector3D(10, 0, 0), normal:=New Vector3D(-1, 0, 0)),
                                                  material:=New Material2D(
                                                   scatteringRemission:=New ComponentScaledColorRemission(Color.MediumBlue),
                                                   reflectionRemission:=New BlackColorRemission,
                                                   transparencyRemission:=New BlackColorRemission))
        Dim leftWall = New SingleMaterialSurface(Of Material2D)(New Plane(Location:=Vector3D.Zero, normal:=New Vector3D(1, 0, 0)),
                                                 material:=New Material2D(
                                                   scatteringRemission:=New ComponentScaledColorRemission(Color.Orange),
                                                   reflectionRemission:=New BlackColorRemission,
                                                   transparencyRemission:=New BlackColorRemission))
        Dim ceiling = New SingleMaterialSurface(Of Material2D)(New Plane(Location:=New Vector3D(0, 10, 0), normal:=New Vector3D(0, -1, 0)),
                                       material:=New Material2D(
                                           scatteringRemission:=New ComponentScaledColorRemission(New ExactColor(0.20000000000000001, 0.20000000000000001, 0.20000000000000001)),
                                           reflectionRemission:=New BlackColorRemission,
                                           transparencyRemission:=New BlackColorRemission))

        Dim frontWall = New SingleMaterialSurface(Of Material2D)(New Plane(Location:=Vector3D.Zero, normal:=New Vector3D(0, 0, 1)),
                                       material:=New Material2D(
                                           scatteringRemission:=New ComponentScaledColorRemission(Color.Gray),
                                           reflectionRemission:=New BlackColorRemission,
                                           transparencyRemission:=New BlackColorRemission))

        Dim backWall = New SingleMaterialSurface(Of Material2D)(New Plane(Location:=New Vector3D(0, 0, 40), normal:=New Vector3D(0, 0, -1)),
                                        material:=New Material2D(
                                            scatteringRemission:=New ComponentScaledColorRemission(Color.Gray),
                                            reflectionRemission:=New BlackColorRemission,
                                            transparencyRemission:=New BlackColorRemission))

        Dim lampSurface = New SingleMaterialSurface(Of Material2D)(New Sphere(center:=New Vector3D(5, 10, 8), radius:=2),
                                        material:=New Material2D(lightSourceColor:=ExactColor.White * 10,
                                            scatteringRemission:=New BlackColorRemission,
                                            reflectionRemission:=New BlackColorRemission,
                                            transparencyRemission:=New BlackColorRemission))

        Dim reflectingSphereRadius = 2
        Dim reflectingSphere = New SingleMaterialSurface(Of Material2D)(New Sphere(center:=New Vector3D(3.5, reflectingSphereRadius, 8), radius:=reflectingSphereRadius),
                                material:=New Material2D(
                                    scatteringRemission:=New BlackColorRemission,
                                    reflectionRemission:=New FullColorRemission,
                                    transparencyRemission:=New BlackColorRemission))

        Dim refractionMaterial = New Material2D(scatteringRemission:=New BlackColorRemission,
                                                reflectionRemission:=New ScaledColorRemission(0.40000000000000002),
                                                transparencyRemission:=New FullColorRemission,
                                                refractionIndexQuotient:=1 / glassRefractionIndex)
        Dim refractingSphereRadius = 1.8
        Dim refractionSphereCenter = New Vector3D(8, refractingSphereRadius, 13)
        Dim sphere = New Sphere(center:=refractionSphereCenter,
                                radius:=refractingSphereRadius)
        Dim cutOutSphere = New Sphere(refractionSphereCenter + New Vector3D(0, 1, 1), 2.2999999999999998)
        Dim cutOutAntiSphere = New AntiSphere(cutOutSphere)

        Dim refractingSphere = New SingleMaterialSurface(Of Material2D)(New IntersectedSurfacedPointSet3D(sphere, cutOutAntiSphere),
                                                         material:=refractionMaterial)
        Dim innerRefractionMaterial = refractionMaterial.Clone
        innerRefractionMaterial.RefractionIndexRatio = glassRefractionIndex
        innerRefractionMaterial.ReflectionRemission = New BlackColorRemission

        Dim innerRefractingSphere = New SingleMaterialSurface(Of Material2D)(New UnitedSurfacedPointSet3D(New AntiSphere(sphere), cutOutSphere),
                                                              material:=innerRefractionMaterial)


        Dim allSurfaces = New MaterialSurfaces(Of Material2D) From {ground, ceiling, rightWall, leftWall, frontWall, backWall, lampSurface,
                                                     reflectingSphere, refractingSphere, innerRefractingSphere}

        Dim rayTracer = New RecursiveRayTracer(surface:=allSurfaces,
                                               lightSource:=New LightSources,
                                               shadedPointLightSources:=New List(Of IPointLightSource) From {lamp})

        Return New RayTraceDrawer(rayTracer, Me.VideoSize, view)
    End Function


    Public Function GetRayTracerDrawer(ByVal pointOfTime As Double) As RayTraceDrawer Implements IRayTraceVideo.GetRayTracerDrawer
        Return New RayTracingExamples(Me.VideoSize).FirstRoom(glassRefractionIndex:=pointOfTime)
    End Function

    Public Sub CreateVideo(ByVal directoryPath As String, ByVal timeIntervalStart As Double, ByVal timeIntervalEnd As Double, ByVal timeStep As Double)

        Dim imageIndex As Integer = 0
        Dim maxIndex As Integer = CInt(Floor((timeIntervalEnd - timeIntervalStart) / timeStep))
        Dim formatString = New String("0"c, maxIndex.ToString.Length)

        For time = timeIntervalStart To timeIntervalEnd Step timeStep
            Dim drawer = GetRayTracerDrawer(time)

            drawer.Picture.Save(directoryPath & "\image" & imageIndex.ToString(formatString) & ".png")
            imageIndex += 1
        Next
    End Sub
End Class
