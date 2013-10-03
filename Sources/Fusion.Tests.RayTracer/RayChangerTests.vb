Public Class RayChangerTests
    Private ReadOnly _SourceRay As New SightRay(New Ray(origin:=New Vector3D(1, 1, 0), direction:=New Vector3D(-1, -1, 0)))
    Private ReadOnly _SurfacePoint As New SurfacePoint(Of RadianceSpectrum)(
            surfacePoint:=New SurfacePoint(location:=Vector3D.Zero, normal:=New Vector3D(0, 1, 0)),
            material:=_DummyMaterial, time:=0)

    Private ReadOnly _RayChanger As New RayChanger(Of RadianceSpectrum)(_SourceRay, _SurfacePoint)
    Private Shared ReadOnly _DummyMaterial As Material2D(Of RadianceSpectrum) = Materials2D(Of RadianceSpectrum).Black

    <Test()>
    Public Sub ReflectedRay()
        Assert.That((New Vector3D(-1, 1, 0).Normalized - _RayChanger.ReflectedRay.NormalizedDirection).Length < 0.000000001)
    End Sub

    <Test()>
    Public Sub RefractedRay()
        Dim sinus1 = 1 / Sqrt(2)
        Dim sinus2 = sinus1 / 2

        Dim material = New Material2D(Of RadianceSpectrum)(sourceLight:=New RadianceSpectrum, scatteringRemission:=Nothing, reflectionRemission:=Nothing, transparencyRemission:=Nothing, refractionIndexQuotient:=1 / 2)
        Dim surfacePoint = New SurfacePoint(Of RadianceSpectrum)(
            surfacePoint:=New SurfacePoint(location:=Vector3D.Zero, normal:=New Vector3D(0, 1, 0)),
            material:=material, time:=0)

        Dim rayChanger = New RayChanger(Of RadianceSpectrum)(_SourceRay, surfacePoint)

        Assert.AreEqual(New Vector3D(-sinus2, -Sqrt(1 - sinus2 ^ 2), 0).Normalized, rayChanger.RefractedRay.NormalizedDirection)
    End Sub

    <Test()>
    Public Sub RefractedRay_RefractionIndexQuotientIs1()
        Dim refractedRay = _RayChanger.RefractedRay()
        Dim passedRay = _RayChanger.PassedRay
        Assert.That(Vector3D.Fit(passedRay.NormalizedDirection, refractedRay.NormalizedDirection))
        Assert.That(Vector3D.Fit(passedRay.OriginLocation, refractedRay.OriginLocation, maxRelativeError:=RayChanger(Of RadianceSpectrum).SaftyDistance / 10))
    End Sub

    <Test()>
    Public Sub PassedRay_Should_Not_Intersect_IntersectionPlane()
        Dim sourceRay = New SightRay(New Ray(origin:=New Vector3D(5, 6, 1), direction:=New Vector3D(5, 7, -1)))

        Dim plane = New MaterialSurface(Of RadianceSpectrum)(New Plane(location:=Vector3D.Zero, normal:=New Vector3D(0, 0, 1)), _DummyMaterial)

        Dim intersection = plane.FirstMaterialIntersection(sourceRay)
        Dim passedRay = New RayChanger(Of RadianceSpectrum)(sourceRay, intersection).PassedRay()
        Dim secondIntersection = plane.FirstMaterialIntersection(passedRay)

        Assert.AreEqual(Nothing, secondIntersection)
    End Sub

    <Test()>
    Public Sub PassedRay_Should_Not_Intersect_IntersectionSphere()
        Dim sourceRay = New SightRay(New Ray(origin:=New Vector3D(2, 0, 0), direction:=New Vector3D(-1, 0, 0)))

        Dim sphere = New MaterialSurface(Of RadianceSpectrum)(New Sphere(center:=Vector3D.Zero, radius:=1), _DummyMaterial)

        Dim intersection = sphere.FirstMaterialIntersection(sourceRay)
        Dim passedRay = New RayChanger(Of RadianceSpectrum)(sourceRay, intersection).PassedRay()
        Dim secondIntersection = sphere.FirstMaterialIntersection(passedRay)

        Assert.AreEqual(Nothing, secondIntersection)
    End Sub

    <Test()>
    Public Sub RefractedRay_Should_Not_Intersect_IntersectionPlane()
        Dim sourceRay = New SightRay(New Ray(origin:=New Vector3D(5, 6, 1), direction:=New Vector3D(5, 7, -1)))
        Dim plane = New MaterialSurface(Of RgbLight)(New Plane(location:=Vector3D.Zero, normal:=New Vector3D(0, 0, 1)), material:=RgbLightMaterials2D.Black)

        Dim intersection = plane.MaterialIntersections(sourceRay).First
        Dim refractedRay = New RayChanger(Of RgbLight)(sourceRay, intersection).RefractedRay()

        Assert.That(plane.Intersections(refractedRay.Ray).Count = 0)
    End Sub
End Class
