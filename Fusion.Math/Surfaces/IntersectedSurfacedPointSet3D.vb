Public Class IntersectedSurfacedPointSet3D
    Implements ISurfacedPointSet3D

    Private ReadOnly _SurfacedPointSet1 As ISurfacedPointSet3D
    Public ReadOnly Property SurfacedPointSet1 As ISurfacedPointSet3D
        Get
            Return _SurfacedPointSet1
        End Get
    End Property

    Private ReadOnly _SurfacedPointSet2 As ISurfacedPointSet3D
    Public ReadOnly Property SurfacedPointSet2 As ISurfacedPointSet3D
        Get
            Return _SurfacedPointSet2
        End Get
    End Property

    Private ReadOnly _Surface As Surfaces

    Public Sub New(surfacedPointSet1 As ISurfacedPointSet3D, surfacedPointSet2 As ISurfacedPointSet3D)
        _SurfacedPointSet1 = surfacedPointSet1
        _SurfacedPointSet2 = surfacedPointSet2
        _Surface = New Surfaces() From {New TruncatedSurface(baseSurface:=Me.SurfacedPointSet1, truncatingPointSet:=New InversePointSet3D(Me.SurfacedPointSet2)),
                                        New TruncatedSurface(baseSurface:=Me.SurfacedPointSet2, truncatingPointSet:=New InversePointSet3D(Me.SurfacedPointSet1))}
    End Sub

    Public Function Contains(point As Vector3D) As Boolean Implements IPointSet3D.Contains
        Return Me.SurfacedPointSet1.Contains(point) AndAlso Me.SurfacedPointSet2.Contains(point)
    End Function

    Public Function FirstIntersection(ray As Ray) As SurfacePoint Implements ISurface.FirstIntersection
        Return _Surface.FirstIntersection(ray)
    End Function

    Public Function Intersections(ray As Ray) As IEnumerable(Of SurfacePoint) Implements ISurface.Intersections
        Return _Surface.Intersections(ray)
    End Function
End Class