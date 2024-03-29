﻿Public Class AntiSphere
    Implements ISurfacedPointSet3D

    Private ReadOnly _Sphere As Sphere
    Public ReadOnly Property Center As Vector3D
        Get
            Return _Sphere.Center
        End Get
    End Property

    Public ReadOnly Property Radius As Double
        Get
            Return _Sphere.Radius
        End Get
    End Property

    Public Sub New(center As Vector3D, radius As Double)
        _Sphere = New Sphere(center, radius)
    End Sub

    Public Sub New(sphere As Sphere)
        _Sphere = sphere
    End Sub

    Public Function FirstIntersection(ray As Ray) As SurfacePoint Implements ISurface.FirstIntersection
        Dim allIntersectionRayLengths = _Sphere.SurfaceIntersectionRayLengths(ray)

        If allIntersectionRayLengths.Count = 0 Then Return Nothing

        Dim rayLength = allIntersectionRayLengths.Max
        Dim intersectionLocation = ray.PointOnRay(distanceFromOrigin:=rayLength)
        Dim normal = Center - intersectionLocation
        Return New SurfacePoint(location:=intersectionLocation, normal:=normal)
    End Function

    Public Function Contains(point As Vector3D) As Boolean Implements IPointSet3D.Contains
        Return Not _Sphere.Contains(point)
    End Function

    Public Function Intersections(ray As Ray) As IEnumerable(Of SurfacePoint) Implements ISurface.Intersections
        Dim intersection = Me.FirstIntersection(ray)

        If intersection Is Nothing Then Return Enumerable.Empty(Of SurfacePoint)()
        Return {intersection}
    End Function
End Class
