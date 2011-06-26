﻿Public Class ColorfulSphere
    Inherits Sphere
    Implements ISurface(Of Material2D(Of RgbLight))


    Public Sub New(ByVal sphere As Sphere, ByVal material As Material2D(Of RgbLight))
        Me.New(Center:=sphere.Center, Radius:=sphere.Radius, material:=material)
    End Sub

    Public Sub New(ByVal center As Vector3D, ByVal radius As Double, ByVal material As Material2D(Of RgbLight))
        MyBase.New(center, radius)
        Me.Material = material
    End Sub

    Public Function MaterialIntersection(ByVal ray As Ray) As SurfacePoint(Of Material2D(Of RgbLight)) Implements ISurface(Of Material2D(Of RgbLight)).FirstMaterialIntersection
        Dim firstIntersection = MyBase.Intersection(ray)
        If firstIntersection Is Nothing Then Return Nothing

        Dim material = Me.Material.Clone()
        material.SourceLight = ColorFromDirection(firstIntersection.NormalizedNormal)

        Return New SurfacePoint(Of Material2D(Of RgbLight))(SurfacePoint:=firstIntersection, material:=material)
    End Function

    Private Function ColorFromDirection(ByVal normalizedNormal As Vector3D) As RgbLight
        Return New RgbLight(colorComponent(normalizedNormal.X),
                              colorComponent(normalizedNormal.Y),
                              colorComponent(normalizedNormal.Z))
    End Function

    Private Function colorComponent(ByVal value As Double) As Double
        Return (value + 1) / 2
    End Function

    Public Property Material As Material2D(Of RgbLight)

    Public Function MaterialIntersections(ByVal ray As Ray) As IEnumerable(Of SurfacePoint(Of Material2D(Of RgbLight))) Implements ISurface(Of Material2D(Of RgbLight)).MaterialIntersections
        Dim materialIntersection = Me.MaterialIntersection(ray)

        If materialIntersection Is Nothing Then Return Enumerable.Empty(Of SurfacePoint(Of Material2D(Of RgbLight)))()

        Return {materialIntersection}
    End Function


End Class