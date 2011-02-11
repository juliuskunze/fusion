Public Class UnitedSurfacedPointSet3D
    Implements ISurfacedPointSet3D

    Private _surfacedPointSet1 As ISurfacedPointSet3D
    Public ReadOnly Property SurfacedPointSet1 As ISurfacedPointSet3D
        Get
            Return _surfacedPointSet1
        End Get
    End Property

    Private _surfacedPointSet2 As ISurfacedPointSet3D
    Public ReadOnly Property SurfacedPointSet2 As ISurfacedPointSet3D
        Get
            Return _surfacedPointSet2
        End Get
    End Property

    Private _surface As Surfaces

    Public Sub New(ByVal surfacedPointSet1 As ISurfacedPointSet3D, ByVal surfacedPointSet2 As ISurfacedPointSet3D)
        _surfacedPointSet1 = SurfacedPointSet1
        _surfacedPointSet2 = SurfacedPointSet2
        _surface = New Surfaces() From {New TruncatedSurface(baseSurface:=Me.SurfacedPointSet1, truncatingPointSet:=Me.SurfacedPointSet2),
                                        New TruncatedSurface(baseSurface:=Me.SurfacedPointSet2, truncatingPointSet:=Me.SurfacedPointSet1)}
    End Sub

    Public Function Contains(ByVal point As Vector3D) As Boolean Implements IPointSet3D.Contains
        Return Me.SurfacedPointSet1.Contains(point) OrElse Me.SurfacedPointSet2.Contains(point)
    End Function

    Public Function Intersections(ByVal ray As Ray) As IEnumerable(Of SurfacePoint) Implements ISurface.Intersections
        Return _surface.Intersections(ray)
    End Function

    Public Function FirstIntersection(ByVal ray As Ray) As SurfacePoint Implements ISurface.FirstIntersection
        Return _surface.FirstIntersection(ray)
    End Function
End Class