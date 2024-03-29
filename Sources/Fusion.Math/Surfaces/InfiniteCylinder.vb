﻿Public Class InfiniteCylinder
    Implements ISurfacedPointSet3D

    Private ReadOnly _Origin As Vector3D
    Public ReadOnly Property Origin As Vector3D
        Get
            Return _Origin
        End Get
    End Property

    Private ReadOnly _NormalizedDirection As Vector3D
    Public ReadOnly Property NormalizedDirection As Vector3D
        Get
            Return _NormalizedDirection
        End Get
    End Property

    Private ReadOnly _Radius As Double
    Private ReadOnly _RadiusSquared As Double
    Public ReadOnly Property Radius As Double
        Get
            Return _Radius
        End Get
    End Property

    Public Sub New(origin As Vector3D, direction As Vector3D, radius As Double)
        _Origin = origin
        _NormalizedDirection = direction.Normalized
        _Radius = radius
        _RadiusSquared = radius ^ 2
    End Sub

    Public Function Contains(point As Vector3D) As Boolean Implements IPointSet3D.Contains
        Return (point - Me.Origin).CrossProduct(Me.NormalizedDirection).LengthSquared <= _RadiusSquared
    End Function

    Friend Function SurfaceIntersectionRayLengths(ray As Ray) As IEnumerable(Of Double)
        ' the quadratic equation was derived from the the cylinder equation and the parametrization of the sight ray
        Dim relativeRayOrigin = ray.Origin - Me.Origin
        Dim temp1 = Me.NormalizedDirection.CrossProduct(ray.NormalizedDirection)
        Dim temp2 = Me.NormalizedDirection.CrossProduct(relativeRayOrigin)
        Dim rayLengthQuadraticEquation = New QuadraticEquation(quadraticCoefficient:=temp1 * temp1,
                                                      linearCoefficient:=2 * temp1 * temp2,
                                                      absoluteCoefficient:=temp2 * temp2 - _RadiusSquared)
        Return rayLengthQuadraticEquation.Solve.Where(Function(rayLength) rayLength >= 0)
    End Function

    Public Function FirstIntersection(ray As Ray) As SurfacePoint Implements ISurface.FirstIntersection
        If Me.Contains(ray.Origin) Then Return Nothing

        Dim allIntersectionLocations = Me.SurfaceIntersectionRayLengths(ray)

        If allIntersectionLocations.Count = 0 Then Return Nothing

        Dim rayLength = allIntersectionLocations.Min
        Dim intersectionLocation As Vector3D = ray.PointOnRay(distanceFromOrigin:=rayLength)

        Dim relativeIntersection = ray.Origin - Me.Origin
        Dim normal = relativeIntersection - relativeIntersection.OrthogonalProjectionOn(NormalizedDirection)
        Return New SurfacePoint(location:=intersectionLocation, normal:=normal)
    End Function

    Public Function Intersections(ray As Ray) As IEnumerable(Of SurfacePoint) Implements ISurface.Intersections
        Dim intersection = Me.FirstIntersection(ray)

        If intersection Is Nothing Then Return Enumerable.Empty(Of SurfacePoint)()

        Return {intersection}
    End Function
End Class
