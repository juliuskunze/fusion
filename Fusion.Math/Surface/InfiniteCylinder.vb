Public Class InfiniteCylinder
    Implements ISurfacedPointSet3D

    Private ReadOnly _origin As Vector3D
    Public ReadOnly Property Origin As Vector3D
        Get
            Return _origin
        End Get
    End Property

    Private ReadOnly _normalizedDirection As Vector3D
    Public ReadOnly Property NormalizedDirection As Vector3D
        Get
            Return _normalizedDirection
        End Get
    End Property

    Private ReadOnly _radius As Double
    Private ReadOnly _radiusSquared As Double
    Public ReadOnly Property Radius As Double
        Get
            Return _radius
        End Get
    End Property

    Public Sub New(ByVal origin As Vector3D, ByVal direction As Vector3D, ByVal radius As Double)
        _origin = origin
        _normalizedDirection = direction.Normalized
        _radius = radius
        _radiusSquared = radius ^ 2
    End Sub

    Public Function Contains(ByVal point As Vector3D) As Boolean Implements IPointSet3D.Contains
        Return (point - Me.Origin).CrossProduct(Me.NormalizedDirection).LengthSquared <= _radiusSquared
    End Function

    Friend Function SurfaceIntersectionRayLengths(ByVal ray As Ray) As IEnumerable(Of Double)
        ' the quadratic equation was derived from the the cylinder equation and the parametrization of the sight ray
        Dim relativeRayOrigin = ray.Origin - Me.Origin
        Dim temp1 = Me.NormalizedDirection.CrossProduct(ray.NormalizedDirection)
        Dim temp2 = Me.NormalizedDirection.CrossProduct(relativeRayOrigin)
        Dim rayLengthQuadraticEquation = New QuadraticEquation(quadraticCoefficient:=temp1 * temp1,
                                                      linearCoefficient:=2 * temp1 * temp2,
                                                      absoluteCoefficient:=temp2 * temp2 - _radiusSquared)
        Return rayLengthQuadraticEquation.Solve.Where(Function(rayLength) rayLength >= 0)
    End Function

    Public Function Intersection(ByVal ray As Ray) As SurfacePoint Implements ISurfacedPointSet3D.FirstIntersection
        If Me.Contains(ray.Origin) Then Return Nothing

        Dim allIntersectionLocations = Me.SurfaceIntersectionRayLengths(ray)

        If allIntersectionLocations.Count = 0 Then Return Nothing

        Dim rayLength = allIntersectionLocations.Min
        Dim intersectionLocation As Vector3D = ray.PointOnRay(distanceFromOrigin:=rayLength)

        Dim relativeRayOrigin = ray.Origin - Me.Origin
        Dim normal = relativeRayOrigin - relativeRayOrigin.OrthogonalProjectionOn(Me.NormalizedDirection)
        Return New SurfacePoint(location:=intersectionLocation, normal:=normal)
    End Function

    Public Function Intersections(ByVal ray As Ray) As System.Collections.Generic.IEnumerable(Of SurfacePoint) Implements ISurface.Intersections
        Dim intersection = Me.Intersection(ray)

        If intersection Is Nothing Then Return Enumerable.Empty(Of SurfacePoint)()

        Return {intersection}
    End Function
End Class
