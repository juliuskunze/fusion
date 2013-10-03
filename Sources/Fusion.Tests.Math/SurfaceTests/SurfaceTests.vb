''' <summary>
''' All instances of implementations of ISurface should pass these tests with every possible ray.
''' </summary>
''' <remarks></remarks>
Public Class SurfaceTests
    Public Shared Sub SurfaceRayIntersection(surface As ISurface, ray As Ray)
        NotFromBehind(surface, ray)
        IntersectionLiesOnRayAndNotContraRayDirection(surface, ray)
    End Sub

    Private Shared Sub NotFromBehind(surface As ISurface, ray As Ray)
        For Each intersection In surface.Intersections(ray)
            Assert.That(intersection.NormalizedNormal * ray.NormalizedDirection < 0,
                        "A ray should not intersect a surface from behind.")
        Next
    End Sub

    Private Shared Sub IntersectionLiesOnRayAndNotContraRayDirection(surface As ISurface, ray As Ray)
        For Each intersection In surface.Intersections(ray)
            Dim relativeIntersectionLocation = intersection.Location - ray.Origin
            Assert.That(relativeIntersectionLocation.Length * ray.NormalizedDirection = relativeIntersectionLocation,
                        "The intersection point should be located on the ray.")
            Assert.That(relativeIntersectionLocation.DotProduct(ray.NormalizedDirection) >= 0,
                        "The intersection point should be located in ray direction.")
        Next
    End Sub
End Class
