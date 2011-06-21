Public Class ColorfulSphere
    Inherits Sphere
    Implements IMaterialSurface(Of Material2D)


    Public Sub New(ByVal sphere As Sphere, ByVal material As Material2D)
        Me.New(Center:=sphere.Center, Radius:=sphere.Radius, material:=material)
    End Sub

    Public Sub New(ByVal center As Vector3D, ByVal radius As Double, ByVal material As Material2D)
        MyBase.New(center, radius)
        Me.Material = material
    End Sub

    Public Function MaterialIntersection(ByVal ray As Ray) As MaterialSurfacePoint(Of Material2D) Implements IMaterialSurface(Of Fusion.Ry.Material2D).FirstMaterialIntersection
        Dim firstIntersection = MyBase.Intersection(ray)
        If firstIntersection Is Nothing Then Return Nothing

        Dim material = Me.Material.Clone()
        material.SourceLight = ColorFromDirection(firstIntersection.NormalizedNormal)

        Return New MaterialSurfacePoint(Of Material2D)(SurfacePoint:=firstIntersection, material:=material)
    End Function

    Private Function ColorFromDirection(ByVal normalizedNormal As Vector3D) As ExactColor
        Return New ExactColor(colorComponent(normalizedNormal.X),
                              colorComponent(normalizedNormal.Y),
                              colorComponent(normalizedNormal.Z))
    End Function

    Private Function colorComponent(ByVal value As Double) As Double
        Return (value + 1) / 2
    End Function

    Public Property Material As Material2D

    Public Function MaterialIntersections(ByVal ray As Ray) As IEnumerable(Of MaterialSurfacePoint(Of Material2D)) Implements IMaterialSurface(Of Material2D).MaterialIntersections
        Dim materialIntersection = Me.MaterialIntersection(ray)

        If materialIntersection Is Nothing Then Return Enumerable.Empty(Of MaterialSurfacePoint(Of Material2D))()

        Return {materialIntersection}
    End Function


End Class
