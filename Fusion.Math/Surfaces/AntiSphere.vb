﻿Public Class AntiSphere
    Implements ISurfacedPointSet3D

    Private _Sphere As Sphere
    Public Property Center As Vector3D
        Get
            Return _Sphere.Center
        End Get
        Set(ByVal value As Vector3D)
            _Sphere.Center = value
        End Set
    End Property
    Public Property Radius As Double
        Get
            Return _Sphere.Radius
        End Get
        Set(ByVal value As Double)
            _Sphere.Radius = value
        End Set
    End Property

    Public Sub New(ByVal center As Vector3D, ByVal radius As Double)
        _Sphere = New Sphere(center, radius)
    End Sub

    Public Sub New(ByVal sphere As Sphere)
        _Sphere = sphere
    End Sub

    Public Function Intersection(ByVal ray As Ray) As SurfacePoint Implements ISurfacedPointSet3D.FirstIntersection
        Dim allIntersectionRayLengths = _Sphere.SurfaceIntersectionRayLengths(ray)

        If allIntersectionRayLengths.Count = 0 Then Return Nothing

        Dim rayLength = allIntersectionRayLengths.Max
        Dim intersectionLocation = ray.PointOnRay(distanceFromOrigin:=rayLength)
        Dim normal = Me.Center - intersectionLocation
        Return New SurfacePoint(location:=intersectionLocation, normal:=normal)
    End Function

    Public Function Contains(ByVal point As Fusion.Math.Vector3D) As Boolean Implements Fusion.Math.IPointSet3D.Contains
        Return Not _Sphere.Contains(point)
    End Function

    Public Function Intersections(ByVal ray As Ray) As System.Collections.Generic.IEnumerable(Of SurfacePoint) Implements ISurface.Intersections
        Dim intersection = Me.Intersection(ray)

        If intersection Is Nothing Then Return Enumerable.Empty(Of SurfacePoint)()
        Return {intersection}
    End Function
End Class
