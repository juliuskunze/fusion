Public Class Sphere
    Implements ISurfacedPointSet3D

    Public Sub New(ByVal center As Vector3D, ByVal radius As Double)
        Me.Center = center
        Me.Radius = radius
    End Sub

    Public Property Center As Vector3D
    Public Property Radius As Double

    Public Function Intersection(ByVal ray As Ray) As SurfacePoint Implements ISurfacedPointSet3D.FirstIntersection
        If Me.Contains(ray.Origin) Then Return Nothing

        Dim allIntersectionLocations = Me.SurfaceIntersectionRayLengths(ray)

        If allIntersectionLocations.Count = 0 Then Return Nothing

        Dim rayLength = allIntersectionLocations.Min
        Dim intersectionLocation As Vector3D = ray.PointOnRay(distanceFromOrigin:=rayLength)
        Dim normal = intersectionLocation - Me.Center
        Return New SurfacePoint(location:=intersectionLocation, normal:=normal)
    End Function

    Friend Function SurfaceIntersectionRayLengths(ByVal ray As Ray) As IEnumerable(Of Double)
        ' the quadratic equation was derived from the the sphere equation and the parametrization of the sight ray
        Dim relativeRayOrigin = ray.Origin - Me.Center
        Dim rayLengthQuadraticEquation As New QuadraticEquation(
            quadraticCoefficient:=1,
            linearCoefficient:=2 * ray.NormalizedDirection.DotProduct(relativeRayOrigin),
            absoluteCoefficient:=relativeRayOrigin.LengthSquared - Me.Radius ^ 2)
        Return rayLengthQuadraticEquation.Solve.Where(Function(rayLength) rayLength >= 0)
    End Function

    Public Function AllSurfaceIntersectionLocations(ByVal ray As Ray) As List(Of Vector3D)
        Dim distances = Me.SurfaceIntersectionRayLengths(ray)
        Dim surfaceIntersections = New List(Of Vector3D)
        For Each distance In distances
            surfaceIntersections.Add(ray.PointOnRay(distanceFromOrigin:=distance))
        Next
        Return surfaceIntersections
    End Function

    Public Function Contains(ByVal point As Fusion.Math.Vector3D) As Boolean Implements Fusion.Math.IPointSet3D.Contains
        Return (point - Center).Length <= Me.Radius
    End Function

    Public Function Intersections(ByVal ray As Ray) As IEnumerable(Of SurfacePoint) Implements ISurface.Intersections
        Dim intersection = Me.Intersection(ray)

        If intersection Is Nothing Then Return Enumerable.Empty(Of SurfacePoint)()

        Return {intersection}
    End Function
End Class