﻿Public Class Rectangle
    Implements ISurface

    Private _surface As ISurface

    Public Sub New(ByVal vertex1 As Vector3D, ByVal vertex2 As Vector3D, ByVal vertex3 As Vector3D)
        If (vertex2 - vertex3) * (vertex1 - vertex1) <> 0 Then Throw New ArgumentException("The edge vectors of a rectangle must be orthogonal.")

        Dim triangle1 = New Triangle(vertex1:=vertex1, vertex2:=vertex2, vertex3:=vertex3)
        Dim triangle2 = New Triangle(vertex1:=vertex3, vertex2:=vertex3 + (vertex1 - vertex2), vertex3:=vertex1)

        _surface = New Surfaces From {triangle1, triangle2}
    End Sub

    Public Shared Function NewFromEdges(ByVal vertex As Vector3D, ByVal counterClockwiseEdgeVector As Vector3D, ByVal clockwiseEdgeVector As Vector3D) As Rectangle
        Return New Rectangle(vertex1:=vertex + clockwiseEdgeVector, vertex2:=vertex, vertex3:=vertex + counterClockwiseEdgeVector)
    End Function

    Public Function FirstIntersection(ByVal ray As Ray) As SurfacePoint Implements ISurface.FirstIntersection
        Return _surface.FirstIntersection(ray)
    End Function

    Public Function Intersections(ByVal ray As Ray) As System.Collections.Generic.IEnumerable(Of SurfacePoint) Implements ISurface.Intersections
        Dim firstIntersection = Me.FirstIntersection(ray)

        If firstIntersection Is Nothing Then Return Enumerable.Empty(Of SurfacePoint)()

        Return {firstIntersection}
    End Function
End Class