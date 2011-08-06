''' <summary>
''' Repräsentiert einen Quader, dessen Kanten parallel zu den Koordinatenachsen ausgerichtet sind.
''' </summary>
''' <remarks></remarks>
Public Class Box
    Implements ISurfacedPointSet3D

    Private ReadOnly _LowerVertex As Vector3D
    Public ReadOnly Property LowerVertex As Vector3D
        Get
            Return _LowerVertex
        End Get
    End Property

    Private ReadOnly _UpperVertex As Vector3D
    Public ReadOnly Property UpperVertex As Vector3D
        Get
            Return _UpperVertex
        End Get
    End Property

    Private ReadOnly _Surface As ISurface

    Public Sub New(vertex As Vector3D, oppositeVertex As Vector3D)
        Dim lowerBoundX = Min(vertex.X, oppositeVertex.X)
        Dim upperBoundX = Max(vertex.X, oppositeVertex.X)

        Dim lowerBoundY = Min(vertex.Y, oppositeVertex.Y)
        Dim upperBoundY = Max(vertex.Y, oppositeVertex.Y)

        Dim lowerBoundZ = Min(vertex.Z, oppositeVertex.Z)
        Dim upperBoundZ = Max(vertex.Z, oppositeVertex.Z)

        _LowerVertex = New Vector3D(lowerBoundX, lowerBoundY, lowerBoundZ)
        _UpperVertex = New Vector3D(upperBoundX, upperBoundY, upperBoundZ)

        Dim lowerBoundXPlane = New Plane(_LowerVertex, New Vector3D(-1, 0, 0))
        Dim upperBoundXPlane = New Plane(_UpperVertex, New Vector3D(1, 0, 0))

        Dim lowerBoundYPlane = New Plane(_LowerVertex, New Vector3D(0, -1, 0))
        Dim upperBoundYPlane = New Plane(_UpperVertex, New Vector3D(0, 1, 0))

        Dim lowerBoundZPlane = New Plane(_LowerVertex, New Vector3D(0, 0, -1))
        Dim upperBoundZPlane = New Plane(_UpperVertex, New Vector3D(0, 0, 1))

        Dim andOperator = Function(a As Boolean, b As Boolean) a OrElse b

        Dim xPlaneBoundPointSet = New InversePointSet3D(New IntersectedSurfacedPointSet3D(New IntersectedSurfacedPointSet3D(lowerBoundYPlane, upperBoundYPlane),
                                                                                          New IntersectedSurfacedPointSet3D(lowerBoundZPlane, upperBoundZPlane)))

        Dim xSurface = New TruncatedSurface(New Surfaces From {lowerBoundXPlane, upperBoundXPlane}, xPlaneBoundPointSet)

        Dim yPlaneBoundPointSet = New InversePointSet3D(New IntersectedSurfacedPointSet3D(New IntersectedSurfacedPointSet3D(lowerBoundXPlane, upperBoundXPlane),
                                                                                          New IntersectedSurfacedPointSet3D(lowerBoundZPlane, upperBoundZPlane)))

        Dim ySurface = New TruncatedSurface(New Surfaces From {lowerBoundYPlane, upperBoundYPlane}, yPlaneBoundPointSet)

        Dim zPlaneBoundPointSet = New InversePointSet3D(New IntersectedSurfacedPointSet3D(New IntersectedSurfacedPointSet3D(lowerBoundXPlane, upperBoundXPlane),
                                                                                          New IntersectedSurfacedPointSet3D(lowerBoundYPlane, upperBoundYPlane)))

        Dim zSurface = New TruncatedSurface(New Surfaces From {lowerBoundZPlane, upperBoundZPlane}, zPlaneBoundPointSet)

        _Surface = New Surfaces From {xSurface, ySurface, zSurface}
    End Sub

    Public Function Contains(point As Vector3D) As Boolean Implements IPointSet3D.Contains
        Return _LowerVertex.X <= point.X AndAlso point.X <= _UpperVertex.X AndAlso
               _LowerVertex.Y <= point.Y AndAlso point.Y <= _UpperVertex.Y AndAlso
               _LowerVertex.Z <= point.Z AndAlso point.Z <= _UpperVertex.Z
    End Function

    Public Function FirstIntersection(ray As Ray) As SurfacePoint Implements ISurface.FirstIntersection
        Return _Surface.FirstIntersection(ray)
    End Function

    Public Function Intersections(ray As Ray) As System.Collections.Generic.IEnumerable(Of SurfacePoint) Implements ISurface.Intersections
        Return _Surface.Intersections(ray)
    End Function
End Class
