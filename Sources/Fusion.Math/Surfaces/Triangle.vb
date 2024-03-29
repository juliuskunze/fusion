﻿''' <summary>
''' The triangle is only visible when the vertices are counter-clockwise.
''' </summary>
''' <remarks></remarks>
Public Class Triangle
    Implements ISurface

    Private ReadOnly _Surface As ISurface
    
    Public Sub New(vertex1 As Vector3D, vertex2 As Vector3D, vertex3 As Vector3D)
        _Vertex1 = vertex1
        _Vertex2 = vertex2
        _Vertex3 = vertex3

        Dim containingPlane = New Plane(vertex1, vertex2, vertex3)

        Dim edgeVector1To2 = Me.Vertex2 - Me.Vertex1
        Dim edgeVector1To3 = Me.Vertex3 - Me.Vertex1
        Dim truncatingPlane1 = New Plane(Me.Vertex1, Me.Vertex2, Me.Vertex1 + edgeVector1To2.CrossProduct(edgeVector1To3))

        Dim edgeVector2To3 = Me.Vertex3 - Me.Vertex2
        Dim edgeVector2To1 = -edgeVector1To2
        Dim truncatingPlane2 = New Plane(Me.Vertex2, Me.Vertex3, Me.Vertex2 + edgeVector2To3.CrossProduct(edgeVector2To1))

        Dim edgeVector3To1 = -edgeVector1To3
        Dim edgeVector3To2 = -edgeVector2To3
        Dim truncatingPlane3 = New Plane(Me.Vertex3, Me.Vertex1, Me.Vertex3 + edgeVector3To1.CrossProduct(edgeVector3To2))

        Dim truncatingPointSet = New InversePointSet3D(New LinkedPointSets3D(
                                                            New LinkedPointSets3D(pointSet1:=truncatingPlane1,
                                                                                  pointSet2:=truncatingPlane2,
                                                                                  linkOperator:=Function(a, b) a AndAlso b),
                                                      pointSet2:=truncatingPlane3,
                                                      linkOperator:=Function(a, b) a AndAlso b))


        _Surface = New TruncatedSurface(baseSurface:=containingPlane, truncatingPointSet:=truncatingPointSet)
    End Sub

    Private ReadOnly _Vertex1 As Vector3D
    Public ReadOnly Property Vertex1 As Vector3D
        Get
            Return _Vertex1
        End Get
    End Property

    Private ReadOnly _Vertex2 As Vector3D
    Public ReadOnly Property Vertex2 As Vector3D
        Get
            Return _Vertex2
        End Get
    End Property

    Private ReadOnly _Vertex3 As Vector3D
    Public ReadOnly Property Vertex3 As Vector3D
        Get
            Return _Vertex3
        End Get
    End Property

    Public Function FirstIntersection(ray As Ray) As SurfacePoint Implements ISurface.FirstIntersection
        Return _Surface.FirstIntersection(ray)
    End Function

    Public Function Intersections(ray As Ray) As IEnumerable(Of SurfacePoint) Implements ISurface.Intersections
        Dim intersection = Me.FirstIntersection(ray)

        If intersection Is Nothing Then Return Enumerable.Empty(Of SurfacePoint)()

        Return {intersection}
    End Function
End Class
