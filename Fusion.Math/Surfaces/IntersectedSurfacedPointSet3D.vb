Public Class IntersectedSurfacedPointSet3D
    Implements ISurfacedPointSet3D

    Private _SurfacedPointSet1 As ISurfacedPointSet3D
    Public ReadOnly Property SurfacedPointSet1 As ISurfacedPointSet3D
        Get
            Return _SurfacedPointSet1
        End Get
    End Property

    Private _SurfacedPointSet2 As ISurfacedPointSet3D
    Public ReadOnly Property SurfacedPointSet2 As ISurfacedPointSet3D
        Get
            Return _SurfacedPointSet2
        End Get
    End Property

    Private _Surface As Surfaces

    Public Sub New(ByVal surfacedPointSet1 As ISurfacedPointSet3D, ByVal surfacedPointSet2 As ISurfacedPointSet3D)
        _SurfacedPointSet1 = surfacedPointSet1
        _SurfacedPointSet2 = surfacedPointSet2
        _Surface = New Surfaces() From {New TruncatedSurface(baseSurface:=Me.SurfacedPointSet1, truncatingPointSet:=New InversePointSet3D(Me.SurfacedPointSet2)),
                                        New TruncatedSurface(baseSurface:=Me.SurfacedPointSet2, truncatingPointSet:=New InversePointSet3D(Me.SurfacedPointSet1))}
    End Sub

    Public Function Contains(ByVal point As Vector3D) As Boolean Implements IPointSet3D.Contains
        Return Me.SurfacedPointSet1.Contains(point) AndAlso Me.SurfacedPointSet2.Contains(point)
    End Function

    Public Function FirstIntersection(ByVal ray As Ray) As SurfacePoint Implements ISurface.FirstIntersection
        Return _Surface.FirstIntersection(ray)
    End Function

    Public Function Intersections(ByVal ray As Ray) As IEnumerable(Of SurfacePoint) Implements ISurface.Intersections
        Return _Surface.Intersections(ray)
    End Function
End Class