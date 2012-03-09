Public Class RelativisticRayTracerTermContextBuilder(Of TLight As {ILight(Of TLight), New}, TMaterial As Material2D(Of TLight))

    Private ReadOnly _RayTracerPictureType As New NamedType("RayTracerPicture", GetType(RayTracerPicture(Of TLight)), "A picture produced by a ray tracer.")
    Private ReadOnly _RayTracerVideoType As New NamedType("RayTracerVideo", GetType(RayTracerVideo(Of TLight)), "A video produced by a ray tracer.")
    Private ReadOnly _MaterialType As New NamedType("Material", GetType(TMaterial), "A material of a 2D surface.")

    Private ReadOnly _SpectralRadianceFunctionType As New NamedType("RadianceSpectrum", New FunctionType(NamedType.Real, Parameters:={New NamedParameter("wavelength", NamedType.Real)}), "A light spectrum as wavelength spectrum of spectral radiance.")
    Private ReadOnly _PictureFunctionType As New NamedType("PictureFunction", New FunctionType(_RayTracerPictureType, Parameters:={New NamedParameter("time", NamedType.Real)}), "A function that returns a picture for each point of time.")
    Private ReadOnly _MaterialFunctionType As New NamedType("MaterialFunction", New FunctionType(_MaterialType, Parameters:={New NamedParameter("location", NamedType.Vector3D)}), "A function that returns a material for each location in 3D space.")
    Private ReadOnly _PointSelectorType As New NamedType("PointSelector", New FunctionType(NamedType.Boolean, Parameters:={New NamedParameter("point", NamedType.Vector3D)}), "A function that returns whether a point is in a point set or not.")

    Private ReadOnly _NamedTypes As New NamedTypes(
        {
            New NamedType("Plane", GetType(Plane), "Represents a plane surface and the covered half space in a 3D space."),
            New NamedType("Sphere", GetType(Sphere), "Represents an outer surface of a sphere in a 3D space."),
            New NamedType("AntiSphere", GetType(AntiSphere), "Represents a the inner surface of a sphere in a 3D space."),
            New NamedType("PointSet", GetType(IPointSet3D), "Represents a point set in 3D space."),
            New NamedType("Box", GetType(Box), "Represents a cuboid surface, which faces a parallel to x-, y- or z-axis."),
            New NamedType("Remission", GetType(IRemission(Of TLight)), "Represents a behaviour how (incoming) light with a specific radiance spectrum gets changed. (For example which wavelengths get absorbed.)"),
            New NamedType("MaterialSurface", GetType(ISurface(Of TMaterial)), "Represents a surface with a specified surface material for each surface point."),
            New NamedType("Surface", GetType(ISurface), "Represents a 2D surface in 3D space."),
            New NamedType("PictureSize", GetType(System.Drawing.Size), "Represents a size of a picture."),
            New NamedType("View", GetType(View3D), "Represents a camera view into a 3D space."),
            New NamedType("LightSource", GetType(ILightSource(Of TLight)), description:="Represents a light source that illuminates surfaces in 3D space."),
            New NamedType("PointLightSource", GetType(IPointLightSource(Of TLight)), "Represents a light source that illuminates surfaces from a point in 3D space."),
            New NamedType("RayTracer", GetType(IRayTracer(Of TLight)), "Computes a resulting light radiance spectrum for each sight ray in 3D space."),
            New NamedType("RecursiveRayTracer", GetType(RecursiveRayTracer(Of TLight)), "Computes a resulting light radiance spectrum for each sight ray in 3D space."),
            New NamedType("RadianceSpectrumToRgbColorConverter", GetType(ILightToRgbColorConverter(Of RadianceSpectrum)), "Converts a radiance spectrum into a gamma corrected rgb color that can be displayed by standard monitors."),
            New NamedType("RecursiveRayTracerReferenceFrame", GetType(RecursiveRayTracerReferenceFrame), "A specified recursive ray tracer 3D scene in a reference frame that has a specified velocity relative to the observer."),
            _MaterialType,
            _RayTracerPictureType,
            _RayTracerVideoType,
            _SpectralRadianceFunctionType,
            _PictureFunctionType,
            _MaterialFunctionType,
            _PointSelectorType
        }
    )

    Protected ReadOnly _TypeDictionary As New TypeDictionary(NamedTypes.Default.Merge(_NamedTypes))
    Public ReadOnly Property TypeDictionary As TypeDictionary
        Get
            Return _TypeDictionary
        End Get
    End Property

    Private ReadOnly _Constants As IEnumerable(Of ConstantInstance) =
                         {
                             New ConstantInstance(Of Double)("c", SpeedOfLight, _TypeDictionary),
                             New ConstantInstance(Of IRemission(Of TLight))("BlackRemission", New BlackRemission(Of TLight), _TypeDictionary, "A remission that absorbs the whole light radiance spectrum."),
                             New ConstantInstance(Of IRemission(Of TLight))("FullRemission", New FullRemission(Of TLight), _TypeDictionary, "A remission that passes the whole light radiance spectrum.")
                         }
    Private ReadOnly _Functions As IEnumerable(Of FunctionInstance) =
                         {
                             FunctionInstance.FromLambdaExpression(
                                 "Peak",
                                 Function(position As Double, height As Double, width As Double) Peak(position:=position, height:=height, width:=width), _TypeDictionary,
                                 "A function which graph is peak with a specified width and height around a specified position. The function returns 0 else."),
                             FunctionInstance.FromLambdaExpression(
                                 "Plane",
                                 Function(location As Vector3D, normal As Vector3D) New Plane(location:=location, normal:=normal), _TypeDictionary,
                                 "The plane with a specified normal and containing the specified point."),
                             FunctionInstance.FromLambdaExpression(
                                 "Plane",
                                 Function(point1 As Vector3D, point2 As Vector3D, point3 As Vector3D) New Plane(point1:=point1, point2:=point2, point3:=point3), _TypeDictionary,
                                 "The plane containing three specified points."),
                             FunctionInstance.FromLambdaExpression(
                                 "Box",
                                 Function(vertex As Vector3D, oppositeVertex As Vector3D) New Box(vertex:=vertex, oppositeVertex:=oppositeVertex), _TypeDictionary,
                                 "A box with the specified opposite vertexes."),
                             FunctionInstance.FromLambdaExpression(
                                 "Sphere",
                                 Function(center As Vector3D, radius As Double) New Sphere(center:=center, radius:=radius), _TypeDictionary,
                                 "A sphere with the specified center and radius."),
                             FunctionInstance.FromLambdaExpression(
                                 "AntiSphere",
                                 Function(center As Vector3D, radius As Double) New AntiSphere(center:=center, radius:=radius), _TypeDictionary,
                                 "An anti sphere with the specified center and radius."),
                             FunctionInstance.FromLambdaExpression(
                                 "AntiSphere",
                                 Function(sphere As Sphere) New AntiSphere(sphere:=sphere), _TypeDictionary,
                                 "The anti sphere equivalent for the specified sphere."),
                             FunctionInstance.FromLambdaExpression(
                                 "PointSet",
                                 Function(pointSelector As Func(Of Vector3D, Boolean)) DirectCast(New PointSet3D(pointSelector:=pointSelector), IPointSet3D), _TypeDictionary,
                                 "A 3D point set that has a specified point selector function."),
                             FunctionInstance.FromLambdaExpression(
                                 "PictureSize",
                                 Function(width As Double, height As Double) New System.Drawing.Size(CInt(width), CInt(height)), _TypeDictionary,
                                 "A picture size with the specified width and height."),
                             FunctionInstance.FromLambdaExpression(
                                 "View",
                                 Function(observerLocation As Vector3D, lookAt As Vector3D, upDirection As Vector3D, horizontalViewAngle As Double) New View3D(observerLocation:=observerLocation, lookAt:=lookAt, upDirection:=upDirection, horizontalViewAngle:=horizontalViewAngle), _TypeDictionary,
                                 "A view with specified observer location, point the observer is looking on, up direction and horizontal visible view angle."),
                             FunctionInstance.FromLambdaExpression(
                                 "RayTracerPicture",
                                 Function(rayTracer As IRayTracer(Of TLight), pictureSize As System.Drawing.Size, view As View3D, lightToRgbColorConverter As ILightToRgbColorConverter(Of TLight)) New RayTracerPicture(Of TLight)(rayTracer:=rayTracer, pictureSize:=pictureSize, view:=view, lightToRgbColorConverter:=lightToRgbColorConverter), _TypeDictionary,
                                 "A picture with a specified size of a specified view into a scene specified by the ray tracer after converting the light with the specified lightToRgbColorConverter."),
                             FunctionInstance.FromLambdaExpression(
                                 "RayTracerVideo",
                                 Function(pictureFunction As Func(Of Double, RayTracerPicture(Of TLight)), framesPerSecond As Double, duration As Double, startTime As Double, timeStep As Double) New RayTracerVideo(Of TLight)(pictureFunction:=pictureFunction, framesPerSecond:=framesPerSecond, duration:=duration, startTime:=startTime, timeStep:=timeStep), _TypeDictionary,
                                 "A video with specified duration and frame count per second. The specified time step and start time define the points of time where the pictures are chosen from the picture function."),
                             FunctionInstance.FromLambdaExpression(
                                 "SingleMaterialSurface",
                                 Function(surface As ISurface, material As TMaterial) DirectCast(New SingleMaterialSurface(Of TMaterial)(surface:=surface, material:=material), ISurface(Of TMaterial)), _TypeDictionary,
                                 "A material surface that has the same specified material at all points of the specified surface."),
                             FunctionInstance.FromLambdaExpression(
                                 "BlackbodyRadianceSpectrum",
                                 Function(temperature As Double) New Func(Of Double, Double)(Function(wavelength As Double) New RadianceSpectrum(New BlackBodyRadianceSpectrum(temperature)).Function(wavelength)), _TypeDictionary,
                                 "The radiance spectrum of a black body at a specified temperature."),
                             FunctionInstance.FromLambdaExpression(
                                 "MaterialBox",
                                 Function(
                                                                      box As Box,
                                                                      lowerXMaterial As TMaterial,
                                                                      upperXMaterial As TMaterial,
                                                                      lowerYMaterial As TMaterial,
                                                                      upperYMaterial As TMaterial,
                                                                      lowerZMaterial As TMaterial,
                                                                      upperZMaterial As TMaterial) DirectCast(New MaterialBox(Of TMaterial)(box:=box, lowerXMaterial:=lowerXMaterial, upperXMaterial:=upperXMaterial, lowerYMaterial:=lowerYMaterial, upperYMaterial:=upperYMaterial, lowerZMaterial:=lowerZMaterial, upperZMaterial:=upperZMaterial), ISurface(Of TMaterial)), _TypeDictionary,
                                 "A material box with specified materials for each face of the box."),
                             FunctionInstance.FromLambdaExpression(
                                 "MaterialSurface",
                                 Function(surface As ISurface,
                                          materialFunction As Func(Of Vector3D, TMaterial)) DirectCast(New MaterialSurface(Of TMaterial)(surface:=surface, materialFunction:=materialFunction), ISurface(Of TMaterial)), _TypeDictionary,
                                 "A material surface that has a material specified by the material function depending on the location of the point of the specified surface."),
                             FunctionInstance.FromLambdaExpression(
                                 "Checkerboard",
                                 Function(
                                                                      xVector As Vector3D,
                                                                      yVector As Vector3D,
                                                                      material1 As TMaterial,
                                                                      material2 As TMaterial) MaterialFunctions(Of TMaterial).Checkerboard(xVector:=xVector, yVector:=yVector, material1:=material1, material2:=material2), _TypeDictionary,
                                 "A material function that looks like the checkerboard pattern for planes that are parallel to xVector and yvector. The length of xVector and yVector are the edge lengths of the resulting checkerboard fields."),
                             FunctionInstance.FromLambdaExpression(
                                 "Surfaces",
                                 Function(surfaces As IEnumerable(Of ISurface)) DirectCast(New Surfaces(surfaces:=surfaces), ISurface), _TypeDictionary,
                                 "A surface that consists of the given set of surfaces."),
                             FunctionInstance.FromLambdaExpression(
                                 "MaterialSurfaces",
                                 Function(materialSurfaces As IEnumerable(Of ISurface(Of TMaterial))) DirectCast(New Surfaces(Of TMaterial)(materialSurfaces), ISurface(Of TMaterial)), _TypeDictionary,
                                 "A material surface that consists of the given set of material surfaces."),
                             FunctionInstance.FromLambdaExpression(
                                 "LightSources",
                                 Function(lightSources As IEnumerable(Of ILightSource(Of TLight))) DirectCast(New LightSources(Of TLight)(lightSources), ILightSource(Of TLight)), _TypeDictionary,
                                 "A light source that consists of the given set of light sources."),
                             FunctionInstance.FromLambdaExpression(
                                 "TruncatedSurface",
                                 Function(baseSurface As ISurface, truncatingPointSet As IPointSet3D) DirectCast(New TruncatedSurface(baseSurface:=baseSurface, truncatingPointSet:=truncatingPointSet), ISurface), _TypeDictionary,
                                 "A surface that is truncated by a point set."),
                             FunctionInstance.FromLambdaExpression(
                                 "InversePointSet",
                                 Function(pointSet As IPointSet3D) DirectCast(New InversePointSet3D(pointSet:=pointSet), IPointSet3D), _TypeDictionary,
                                 "The inverse of a specified point set.")}

        Private ReadOnly _TermContext As TermContext
    Public ReadOnly Property TermContext As TermContext
        Get
            Return _TermContext
        End Get
    End Property

    Public Sub New()
        _TermContext = TermContext.Default.Merge(New TermContext(Constants:=_Constants, Functions:=_Functions, types:=_NamedTypes))
    End Sub

    Protected Sub New(termContext As TermContext)
        _TermContext = termContext
    End Sub

End Class

Public Class RelativisticRayTracerTermContextBuilder
    Inherits RelativisticRayTracerTermContextBuilder(Of RadianceSpectrum, Material2D(Of RadianceSpectrum))

    Private ReadOnly _ExtraFunctions As IEnumerable(Of FunctionInstance) =
                         {
                             FunctionInstance.FromLambdaExpression("RecursiveRayTracer",
                                                                   Function(
                                                                      surface As ISurface(Of Material2D(Of RadianceSpectrum)),
                                                                      unshadedLightSource As ILightSource(Of RadianceSpectrum),
                                                                      shadedPointLightSources As IEnumerable(Of IPointLightSource(Of RadianceSpectrum)),
                                                                      maxIntersectionCount As Double) New RecursiveRayTracer(Of RadianceSpectrum)(surface:=surface, unshadedLightSource:=unshadedLightSource, shadedPointLightSources:=shadedPointLightSources, maxIntersectionCount:=CInt(maxIntersectionCount)),
                                                                   _TypeDictionary,
                                                                   "A ray tracer that supports only direct illumiation (and shadows). The scene consists of a specified material surface and unshaded and shaded light sources. The sight ray tracing stops if the sight ray is reflected or refracted more than a specified maximum intersection count."),
                             FunctionInstance.FromLambdaExpression("ScatteringRayTracer",
                                                                   Function(
                                                                      surface As ISurface(Of Material2D(Of RadianceSpectrum)),
                                                                      rayCountPerPixel As Double,
                                                                      maxIntersectionCount As Double) DirectCast(New ScatteringRayTracer(Of RadianceSpectrum)(surface:=surface, rayCountPerPixel:=CInt(rayCountPerPixel), maxIntersectionCount:=CInt(maxIntersectionCount)), IRayTracer(Of RadianceSpectrum)),
                                                                   _TypeDictionary,
                                                                   "A ray tracer that supports full illumination (and shadows). The scene consists of a specified material surface. The sight ray tracing stops if the sight ray is reflected or refracted more than a specified maximum intersection count."),
                             FunctionInstance.FromLambdaExpression("TransparentMaterial",
                                                                   Function(
                                                                      emittedLight As Func(Of Double, Double),
                                                                      scatteringRemission As IRemission(Of RadianceSpectrum),
                                                                      reflectionRemission As IRemission(Of RadianceSpectrum),
                                                                      transparencyRemission As IRemission(Of RadianceSpectrum),
                                                                      refractionIndexQuotient As Double) New Material2D(Of RadianceSpectrum)(sourceLight:=New RadianceSpectrum(Function(wavelength) emittedLight(wavelength)),
                                                                                                                                                                                       scatteringRemission:=scatteringRemission,
                                                                                                                                                                                       reflectionRemission:=reflectionRemission,
                                                                                                                                                                                       transparencyRemission:=transparencyRemission,
                                                                                                                                             refractionIndexQuotient:=refractionIndexQuotient),
                                                                   _TypeDictionary,
                                                                   "A material that has a specified light emission and a specified light scattering, reflection and transparancy remission and a specified quotient of the refraction index of the medium before and the medium behind the surface."),
                             FunctionInstance.FromLambdaExpression("EmissionMaterial",
                                                                   Function(
                                                                      emittedLight As Func(Of Double, Double)) New Material2D(Of RadianceSpectrum)(sourceLight:=New RadianceSpectrum(Function(wavelength) emittedLight(wavelength)),
                                                                                                                                                   scatteringRemission:=New BlackRemission(Of RadianceSpectrum),
                                                                                                                                                   reflectionRemission:=New BlackRemission(Of RadianceSpectrum),
                                                                                                                                                   transparencyRemission:=New BlackRemission(Of RadianceSpectrum)),
                                                                   _TypeDictionary,
                                                                   "A material that absorbs everything and emits a specified light."),
                             FunctionInstance.FromLambdaExpression("IntransparentMaterial",
                                                                   Function(
                                                                      emittedLight As Func(Of Double, Double),
                                                                      scatteringRemission As IRemission(Of RadianceSpectrum),
                                                                      reflectionRemission As IRemission(Of RadianceSpectrum)) New Material2D(Of RadianceSpectrum)(sourceLight:=New RadianceSpectrum(Function(wavelength) emittedLight(wavelength)),
                                                                                                                                                   scatteringRemission:=scatteringRemission,
                                                                                                                                                   reflectionRemission:=reflectionRemission,
                                                                                                                                                   transparencyRemission:=New BlackRemission(Of RadianceSpectrum)),
                                                                   _TypeDictionary,
                                                                   "A material that is not permeable for light and that has a specified light emission and a specified light scattering and reflection remission."),
                             FunctionInstance.FromLambdaExpression("Remission",
                                                                   Function(albedoSpectrum As Func(Of Double, Double)) DirectCast(New Remission(albedoSpectrum:=albedoSpectrum), IRemission(Of RadianceSpectrum)),
                                                                   _TypeDictionary,
                                                                   "A remission that passes light with a specified albedo for every wavelength."),
                             FunctionInstance.FromLambdaExpression("RelativisticRayTracer",
                                                                   Function(
                                                                      classicRayTracer As IRayTracer(Of RadianceSpectrum),
                                                                      observerVelocity As Vector3D,
                                                                      ignoreGeometryEffect As Boolean,
                                                                      ignoreDopplerEffect As Boolean,
                                                                      ignoreSearchlightEffect As Boolean) DirectCast(New SingleObjectFrameRelativisticRayTracer(classicRayTracer:=classicRayTracer, observerVelocity:=observerVelocity, options:=New LorentzTransformationAtSightRayOptions(ignoreGeometryEffect:=ignoreGeometryEffect, ignoreDopplerEffect:=ignoreDopplerEffect, ignoreSearchlightEffect:=ignoreSearchlightEffect)), IRayTracer(Of RadianceSpectrum)),
                                                                   _TypeDictionary,
                                                                   "A ray tracer that supports effects of special relatity at a specified observer velocity based on a specified classic ray tracer. It is possible to ignore the geometry, Doppler or searchlight effect."),
                             FunctionInstance.FromLambdaExpression("PointLightSource", Function(location As Vector3D, baseLight As Func(Of Double, Double)) DirectCast(New PointLightSource(Of RadianceSpectrum)(location:=location, baseLight:=New RadianceSpectrum(Function(wavelength) baseLight(wavelength))), IPointLightSource(Of RadianceSpectrum)), _TypeDictionary,
                                                                   "A point light source that has a specified location and a specified base light (radiance spectrum at the distance of 1m)."),
                             FunctionInstance.FromLambdaExpression("DirectionalLightSource", Function(direction As Vector3D, light As Func(Of Double, Double)) DirectCast(New DirectionalLightSource(Of RadianceSpectrum)(direction:=direction, light:=New RadianceSpectrum(Function(wavelength) light(wavelength))), ILightSource(Of RadianceSpectrum)), _TypeDictionary,
                                                                   "A light source that emits a specified radiance spectrum in a specified direction in whole space."),
                             FunctionInstance.FromLambdaExpression("UndirectionalLightSource", Function(light As Func(Of Double, Double)) DirectCast(New UndirectionalLightSource(Of RadianceSpectrum)(light:=New RadianceSpectrum(Function(wavelength) light(wavelength))), ILightSource(Of RadianceSpectrum)), _TypeDictionary,
                                                                   "A light source that emits a specified radiance spectrum in every direction in whole space."),
                             FunctionInstance.FromLambdaExpression("RadianceSpectrumToRgbColorConverter", Function(spectralRadiancePerWhite As Double) DirectCast(New RadianceSpectrumToRgbColorConverter(spectralRadiancePerWhite, testedWavelengthsCount:=100), ILightToRgbColorConverter(Of RadianceSpectrum)), _TypeDictionary,
                                              "A RadianceSpectrumToRgbColorConverter that converts a radiance spectrum linear into an rgb color. If the whole spectrum is the specified spectralRadiancePerWhite, the rgb color will be the white (255, 255, 255). If the red, green or blue component would get greater than 255, all three components are scaled down so that it fits into the possible range. The tested wavelengths count is set to 100."),
                             FunctionInstance.FromLambdaExpression("RadianceSpectrumToRgbColorConverter", Function(spectralRadiancePerWhite As Double, testedWavelengthsCount As Double) DirectCast(New RadianceSpectrumToRgbColorConverter(spectralRadiancePerWhite, testedWavelengthsCount:=CInt(testedWavelengthsCount)), ILightToRgbColorConverter(Of RadianceSpectrum)), _TypeDictionary,
                                              "A RadianceSpectrumToRgbColorConverter that converts a radiance spectrum linear into an rgb color. If the whole spectrum is the specified spectralRadiancePerWhite, the rgb color will be the white (255, 255, 255). If the red, green or blue component would get greater than 255, all three components are scaled down so that it fits into the possible range. The accuracy of the conversion will grow with a higher specified tested wavelengths count."),
                             FunctionInstance.FromLambdaExpression("RecursiveRayTracerReferenceFrame", Function(recursiveRayTracer As RecursiveRayTracer(Of RadianceSpectrum), objectVelocityRelativeToObserver As Vector3D) New RecursiveRayTracerReferenceFrame(recursiveRayTracer:=recursiveRayTracer, objectVelocityRelativeToObserver:=objectVelocityRelativeToObserver), _TypeDictionary),
                             FunctionInstance.FromLambdaExpression("RecursiveRelativisticRayTracer",
                                                                   Function(observerTime As Double,
                                                                            referenceFrames As IEnumerable(Of RecursiveRayTracerReferenceFrame),
                                                                            ignoreGeometryEffect As Boolean,
                                                                            ignoreDopplerEffect As Boolean,
                                                                            ignoreSearchlightEffect As Boolean) DirectCast(New RecursiveRelativisticRayTracer(observerTime, referenceFrames, New LorentzTransformationAtSightRayOptions(ignoreGeometryEffect:=ignoreGeometryEffect, ignoreDopplerEffect:=ignoreDopplerEffect, ignoreSearchlightEffect:=ignoreSearchlightEffect)), IRayTracer(Of RadianceSpectrum)), _TypeDictionary,
                                                                   "A ray tracer that ca have multiple object reference frames that have a constant velocity relative to the observer.")
       }


    Private ReadOnly _ExtraConstants As IEnumerable(Of ConstantInstance) = {New ConstantInstance(Of Material2D(Of RadianceSpectrum))("BlackMaterial", Materials2D(Of RadianceSpectrum).Black, TypeDictionary:=_TypeDictionary, description:="A material that absorbs light totally.")}

    Public Sub New()
        _TermContext = MyBase.TermContext.Merge(New TermContext(Functions:=_ExtraFunctions)).Merge(New TermContext(Constants:=_ExtraConstants))
    End Sub

    Private ReadOnly _TermContext As TermContext
    Public Overloads ReadOnly Property TermContext As TermContext
        Get
            Return _TermContext
        End Get
    End Property

End Class