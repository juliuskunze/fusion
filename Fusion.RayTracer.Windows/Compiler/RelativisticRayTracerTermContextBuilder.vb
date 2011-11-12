Public Class RelativisticRayTracerTermContextBuilder

    Private ReadOnly _RayTracerPictureType As New NamedType("RayTracerPicture", GetType(RayTracerPicture(Of RadianceSpectrum)))
    Private ReadOnly _RayTracerVideoType As New NamedType("RayTracerVideo", GetType(RayTracerVideo(Of RadianceSpectrum)))

    Private ReadOnly _SpectralRadianceFunctionDelegateType As New NamedType("SpectralRadianceFunction", New DelegateType(NamedType.Real, Parameters:={New NamedParameter("wavelength", NamedType.Real)}))
    Private ReadOnly _PictureFunctionDelegateType As New NamedType("PictureFunction", New DelegateType(_RayTracerPictureType, Parameters:={New NamedParameter("time", NamedType.Real)}))

    Private ReadOnly _NamedTypes As New NamedTypes({New NamedType("Plane", GetType(Plane)),
                                                      New NamedType("Sphere", GetType(Sphere)),
                                                      New NamedType("Remission", GetType(IRemission(Of RadianceSpectrum))),
                                                      New NamedType("Material", GetType(Material2D(Of RadianceSpectrum))),
                                                      New NamedType("MaterialSurface", GetType(ISurface(Of Material2D(Of RadianceSpectrum)))),
                                                      New NamedType("Surface", GetType(ISurface)),
                                                      New NamedType("PictureSize", GetType(System.Drawing.Size)),
                                                      New NamedType("View", GetType(View3D)),
                                                      New NamedType("RadianceSpectrumToRgbColorConverter", GetType(ILightToColorConverter(Of RadianceSpectrum))),
                                                      New NamedType("RayTracer", GetType(IRayTracer(Of RadianceSpectrum))),
                                                      _RayTracerPictureType,
                                                      _RayTracerVideoType,
                                                      _SpectralRadianceFunctionDelegateType,
                                                      _PictureFunctionDelegateType})

    Private ReadOnly _TypeDictionary As New TypeNamedTypeDictionary(NamedTypes.Default.Merge(_NamedTypes))
    Public ReadOnly Property TypeDictionary As TypeNamedTypeDictionary
        Get
            Return _TypeDictionary
        End Get
    End Property

    Private ReadOnly _Constants As IEnumerable(Of ConstantInstance) = {New ConstantInstance(Of Double)("c", SpeedOfLight, _TypeDictionary),
                                                                       New ConstantInstance(Of IRemission(Of RadianceSpectrum))("BlackRemission", New BlackRemission(Of RadianceSpectrum), _TypeDictionary),
                                                                       New ConstantInstance(Of IRemission(Of RadianceSpectrum))("FullRemission", New FullRemission(Of RadianceSpectrum), _TypeDictionary)}
    Private ReadOnly _Functions As IEnumerable(Of FunctionInstance) = {FunctionInstance.FromLambdaExpression("Plane", Function(location As Vector3D, normal As Vector3D) New Plane(location:=location, normal:=normal), _TypeDictionary),
                                                                       FunctionInstance.FromLambdaExpression("Plane", Function(point1 As Vector3D, point2 As Vector3D, point3 As Vector3D) New Plane(point1:=point1, point2:=point2, point3:=point3), _TypeDictionary),
                                                                       FunctionInstance.FromLambdaExpression("Sphere", Function(center As Vector3D, radius As Double) New Sphere(center:=center, radius:=radius), _TypeDictionary),
                                                                       FunctionInstance.FromLambdaExpression("PictureSize", Function(width As Double, height As Double) New System.Drawing.Size(CInt(width), CInt(height)), _TypeDictionary),
                                                                       FunctionInstance.FromLambdaExpression("Material",
                                                                                                             Function(sourceLight As Func(Of Double, Double),
                                                                                                                scatteringRemission As IRemission(Of RadianceSpectrum),
                                                                                                                reflectionRemission As IRemission(Of RadianceSpectrum),
                                                                                                                transparencyRemission As IRemission(Of RadianceSpectrum),
                                                                                                                refractionIndexQuotient As Double) New Material2D(Of RadianceSpectrum)(sourceLight:=New RadianceSpectrum(Function(wavelength) sourceLight(wavelength)),
                                                                                                                                                                                       scatteringRemission:=scatteringRemission,
                                                                                                                                                                                       reflectionRemission:=reflectionRemission,
                                                                                                                                                                                       transparencyRemission:=transparencyRemission,
                                                                                                                                                                                       refractionIndexQuotient:=refractionIndexQuotient),
                                                                                                             _TypeDictionary),
                                                                       FunctionInstance.FromLambdaExpression("View", Function(observerLocation As Vector3D, lookAt As Vector3D, upDirection As Vector3D, horizontalViewAngle As Double) New View3D(observerLocation:=observerLocation, lookAt:=lookAt, upDirection:=upDirection, horizontalViewAngle:=horizontalViewAngle), _TypeDictionary),
                                                                       FunctionInstance.FromLambdaExpression("ScatteringRayTracer", Function(surface As ISurface(Of Material2D(Of RadianceSpectrum)), rayCountPerPixel As Double, maxIntersectionCount As Double) CType(New ScatteringRayTracer(Of RadianceSpectrum)(surface:=surface, rayCountPerPixel:=CInt(rayCountPerPixel), maxIntersectionCount:=CInt(maxIntersectionCount)), IRayTracer(Of RadianceSpectrum)), _TypeDictionary),
                                                                       FunctionInstance.FromLambdaExpression("RelativisticRayTracer", Function(classicRayTracer As IRayTracer(Of RadianceSpectrum), observerVelocity As Vector3D, ignoreGeometryEffect As Boolean, ignoreDopplerEffect As Boolean, ignoreSearchlightEffect As Boolean) CType(New RelativisticRayTracer(classicRayTracer:=classicRayTracer, observerVelocity:=observerVelocity, ignoreGeometryEffect:=ignoreGeometryEffect, ignoreDopplerEffect:=ignoreDopplerEffect, ignoreSearchlightEffect:=ignoreSearchlightEffect), IRayTracer(Of RadianceSpectrum)), _TypeDictionary),
                                                                       FunctionInstance.FromLambdaExpression("RayTracerPicture", Function(rayTracer As IRayTracer(Of RadianceSpectrum), pictureSize As System.Drawing.Size, view As View3D, radianceSpectrumToRgbColorConverter As ILightToColorConverter(Of RadianceSpectrum)) New RayTracerPicture(Of RadianceSpectrum)(rayTracer:=rayTracer, pictureSize:=pictureSize, view:=view, lightToColorConverter:=radianceSpectrumToRgbColorConverter), _TypeDictionary),
                                                                       FunctionInstance.FromLambdaExpression("RayTracerVideo", Function(pictureFunction As Func(Of Double, RayTracerPicture(Of RadianceSpectrum)), framesPerSecond As Double, duration As Double, startTime As Double, timeStep As Double) New RayTracerVideo(Of RadianceSpectrum)(pictureFunction:=pictureFunction, framesPerSecond:=framesPerSecond, duration:=duration, startTime:=startTime, timeStep:=timeStep), _TypeDictionary),
                                                                       FunctionInstance.FromLambdaExpression("SingleMaterialSurface", Function(surface As ISurface(Of Material2D(Of RadianceSpectrum)), material As Material2D(Of RadianceSpectrum)) CType(New SingleMaterialSurface(Of Material2D(Of RadianceSpectrum))(surface:=surface, material:=material), ISurface(Of Material2D(Of RadianceSpectrum))), _TypeDictionary),
                                                                       FunctionInstance.FromLambdaExpression("BlackbodyRadianceSpectrum", Function(temperature As Double) New Func(Of Double, Double)(Function(wavelength As Double) New RadianceSpectrum(New BlackBodyRadianceSpectrum(temperature)).Function(wavelength)), _TypeDictionary),
                                                                       FunctionInstance.FromLambdaExpression("SquaredMaterialSurface",
                                                                                                             Function(
                                                                                                                surface As ISurface,
                                                                                                                material1 As Material2D(Of RadianceSpectrum),
                                                                                                                material2 As Material2D(Of RadianceSpectrum),
                                                                                                                squareXVector As Vector3D,
                                                                                                                squareYVector As Vector3D) DirectCast(New SquaredMaterialSurface(Of Material2D(Of RadianceSpectrum))(surface:=surface, material1:=material1, material2:=material2, squareXVector:=squareXVector, squareYVector:=squareYVector), ISurface(Of Material2D(Of RadianceSpectrum))), _TypeDictionary)}

    Private ReadOnly _TermContext As TermContext
    Public ReadOnly Property TermContext As TermContext
        Get
            Return _TermContext
        End Get
    End Property

    Public Sub New()
        _TermContext = TermContext.Default.Merge(New TermContext(Constants:=_Constants, Functions:=_Functions, types:=_NamedTypes))
    End Sub

End Class
