Public Class RayChangerTests

    <Test()>
    Public Sub ReflectedRay()
        Dim sourceRay = New Ray(origin:=New Vector3D(1, 1, 0), direction:=New Vector3D(-1, -1, 0))
        Dim intersection = New SurfacePoint(location:=Vector3D.Zero, normal:=New Vector3D(0, 1, 0))

        Dim reflectedRay = New RayChanger(sourceRay).ReflectedRay(intersection)
        Assert.That((New Vector3D(-1, 1, 0).Normalized - reflectedRay.NormalizedDirection).Length < 0.000000001)
    End Sub

    <Test()>
    Public Sub RefractedRay()
        Dim sourceRay = New Ray(origin:=New Vector3D(1, 1, 0), direction:=New Vector3D(-1, -1, 0))
        Dim intersection = New SurfacePoint(Of Material2D(Of ExactColor))(location:=Vector3D.Zero, normal:=New Vector3D(0, 1, 0), material:=New Material2D(Of ExactColor)(sourceLight:=New ExactColor, scatteringRemission:=Nothing, reflectionRemission:=Nothing, transparencyRemission:=Nothing, refractionIndexQuotient:=1 / 2))

        Dim sinus1 = 1 / Sqrt(2)
        Dim sinus2 = sinus1 / 2

        Dim refractedRay = New RayChanger(sourceRay).RefractedRay(intersection)
        Assert.AreEqual(New Vector3D(-sinus2, -Sqrt(1 - sinus2 ^ 2), 0).Normalized, refractedRay.NormalizedDirection)
    End Sub

    <Test()>
    Public Sub RefractedRay_RefractionIndexQuotientIs1()
        Dim sourceRay = New Ray(origin:=New Vector3D(1, 1, 0), direction:=New Vector3D(-1, -1, 0))
        Dim intersection = New SurfacePoint(Of Material2D(Of ExactColor))(location:=Vector3D.Zero, normal:=New Vector3D(0, 1, 0), material:=ColorMaterials2D.Black)

        Dim refractedRay = New RayChanger(sourceRay).RefractedRay(intersection)
        Dim passedRay = New RayChanger(sourceRay).PassedRay(intersection)
        Assert.That(Vector3D.Fit(passedRay.NormalizedDirection, refractedRay.NormalizedDirection))
        Assert.That(Vector3D.Fit(passedRay.Origin, refractedRay.Origin, maxRelativeError:=RayChanger.SaftyDistance / 10))
    End Sub

    <Test()>
    Public Sub PassedRay_Should_Not_Intersect_IntersectionPlane()
        Dim sourceRay = New Ray(origin:=New Vector3D(5, 6, 1), direction:=New Vector3D(5, 7, -1))

        Dim plane = New Plane(location:=Vector3D.Zero, normal:=New Vector3D(0, 0, 1))

        Dim intersection = plane.Intersection(sourceRay)
        Dim passedRay = New RayChanger(sourceRay).PassedRay(intersection)
        Dim secondIntersection = plane.Intersection(passedRay)

        Assert.AreEqual(Nothing, secondIntersection)
    End Sub

    <Test()>
    Public Sub PassedRay_Should_Not_Intersect_IntersectionSphere()
        Dim sourceRay = New Ray(origin:=New Vector3D(2, 0, 0), direction:=New Vector3D(-1, 0, 0))

        Dim sphere = New Sphere(center:=Vector3D.Zero, radius:=1)

        Dim intersection = sphere.Intersection(sourceRay)
        Dim passedRay = New RayChanger(sourceRay).PassedRay(intersection)
        Dim secondIntersection = sphere.Intersection(passedRay)

        Assert.AreEqual(Nothing, secondIntersection)
    End Sub

    <Test()>
    Public Sub RefractedRay_Should_Not_Intersect_IntersectionPlane()
        Dim sourceRay = New Ray(origin:=New Vector3D(5, 6, 1), direction:=New Vector3D(5, 7, -1))
        Dim plane = New SingleMaterialSurface(Of Material2D(Of ExactColor))(New Plane(location:=Vector3D.Zero, normal:=New Vector3D(0, 0, 1)), material:=ColorMaterials2D.Black)

        Dim intersection = plane.MaterialIntersections(sourceRay).First
        Dim refractedRay = New RayChanger(sourceRay).RefractedRay(intersection)

        Assert.That(plane.Intersections(refractedRay).Count = 0)
    End Sub

End Class
