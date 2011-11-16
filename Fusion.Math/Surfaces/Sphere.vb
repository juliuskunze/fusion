Public Class Sphere
    Implements ISurfacedPointSet3D

    Public Sub New(center As Vector3D, radius As Double)
        _Center = center
        _Radius = radius
        _RadiusSquared = radius ^ 2
    End Sub

    Private ReadOnly _Center As Vector3D
    Public ReadOnly Property Center As Vector3D
        Get
            Return _Center
        End Get
    End Property

    Private ReadOnly _RadiusSquared As Double
    Private ReadOnly _Radius As Double
    Public ReadOnly Property Radius As Double
        Get
            Return _Radius
        End Get
    End Property

    Public Function Intersection(ray As Ray) As SurfacePoint Implements ISurfacedPointSet3D.FirstIntersection
        If Me.Contains(ray.Origin) Then Return Nothing

        Dim allIntersectionLocations = Me.SurfaceIntersectionRayLengths(ray)

        If allIntersectionLocations.Count = 0 Then Return Nothing

        Dim rayLength = allIntersectionLocations.Min
        Dim intersectionLocation As Vector3D = ray.PointOnRay(distanceFromOrigin:=rayLength)
        Dim normal = intersectionLocation - Me.Center
        Return New SurfacePoint(location:=intersectionLocation, normal:=normal)
    End Function

    Friend Function SurfaceIntersectionRayLengths(ray As Ray) As IEnumerable(Of Double)
        ' the quadratic equation was derived from the the sphere equation and the parametrization of the sight ray
        Dim relativeRayOrigin = ray.Origin - Me.Center
        Dim rayLengthQuadraticEquation As New QuadraticEquation(
            quadraticCoefficient:=1,
            linearCoefficient:=2 * ray.NormalizedDirection.DotProduct(relativeRayOrigin),
            absoluteCoefficient:=relativeRayOrigin.LengthSquared - _RadiusSquared)
        Return rayLengthQuadraticEquation.Solve.Where(Function(rayLength) rayLength >= 0)
    End Function

    Public Function AllSurfaceIntersectionLocations(ray As Ray) As List(Of Vector3D)
        Dim distances = Me.SurfaceIntersectionRayLengths(ray)
        Dim surfaceIntersections = New List(Of Vector3D)
        For Each distance In distances
            surfaceIntersections.Add(ray.PointOnRay(distanceFromOrigin:=distance))
        Next
        Return surfaceIntersections
    End Function

    Public Function Contains(point As Fusion.Math.Vector3D) As Boolean Implements Fusion.Math.IPointSet3D.Contains
        Return (point - Center).LengthSquared <= _RadiusSquared
    End Function

    Public Function Intersections(ray As Ray) As IEnumerable(Of SurfacePoint) Implements ISurface.Intersections
        Dim intersection = Me.Intersection(ray)

        If intersection Is Nothing Then Return Enumerable.Empty(Of SurfacePoint)()

        Return {intersection}
    End Function
End Class