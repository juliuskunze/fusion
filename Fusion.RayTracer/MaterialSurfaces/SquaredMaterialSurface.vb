﻿Public Class SquaredMaterialSurface(Of TMaterial)
    Implements ISurface(Of TMaterial)

    Public Property Surface As ISurface
    Public Property Material1 As TMaterial
    Public Property Material2 As TMaterial

    Public Sub New(surface As ISurface,
                   material1 As TMaterial,
                   material2 As TMaterial,
                   squareXVector As Vector3D,
                   squareYVector As Vector3D)
        Me.Surface = surface
        Me.Material1 = material1
        Me.Material2 = material2
        Me.SquaresXVector = squareXVector
        Me.SquaresYVector = squareYVector
    End Sub


    Private _NormalizedSquaresXVector As Vector3D
    Public ReadOnly Property NormalizedSquaresXVector As Vector3D
        Get
            Return _NormalizedSquaresXVector
        End Get
    End Property
    Public WriteOnly Property SquaresXVector As Vector3D
        Set(value As Vector3D)
            _NormalizedSquaresXVector = value.Normalized
        End Set
    End Property

    Private _NormalizedSquaresYVector As Vector3D
    Private ReadOnly Property NormalizedSquaresYVector As Vector3D
        Get
            Return _NormalizedSquaresYVector
        End Get
    End Property
    Public WriteOnly Property SquaresYVector As Vector3D
        Set(value As Vector3D)
            _NormalizedSquaresYVector = value.Normalized
        End Set
    End Property

    Private Function IsInEvenRow(value As Double, rowWidth As Double) As Boolean
        Dim rest = value Mod (2 * rowWidth)
        If rest < 0 Then
            rest += 2
        End If

        Return rest < 1
    End Function

    Public Function Intersections(ray As Math.Ray) As IEnumerable(Of SurfacePoint) Implements ISurface.Intersections
        Return Me.Surface.Intersections(ray)
    End Function

    Public Function MaterialIntersections(ray As Math.Ray) As IEnumerable(Of SurfacePoint(Of TMaterial)) Implements ISurface(Of TMaterial).MaterialIntersections
        Return From surfacePoint In Me.Intersections(ray)
               Select MaterialSurfacePointFromSurfacePoint(surfacePoint:=surfacePoint)
    End Function

    Private Function MaterialSurfacePointFromSurfacePoint(surfacePoint As SurfacePoint) As SurfacePoint(Of TMaterial)
        Return New SurfacePoint(Of TMaterial)(surfacePoint:=surfacePoint, Material:=Me.Material(surfacePoint))
    End Function

    Private Function Material(surfacePoint As SurfacePoint) As TMaterial
        Dim xLocation = _NormalizedSquaresXVector.Normalized * surfacePoint.Location
        Dim yLocation = _NormalizedSquaresYVector.Normalized * surfacePoint.Location

        Dim useMaterial1 =
                IsInEvenRow(xLocation, rowWidth:=_NormalizedSquaresXVector.Length) Xor
                IsInEvenRow(yLocation, rowWidth:=_NormalizedSquaresYVector.Length)

        Dim resultMaterial As TMaterial
        If useMaterial1 Then
            resultMaterial = Me.Material1
        Else
            resultMaterial = Me.Material2
        End If

        Return resultMaterial
    End Function

    Public Function FirstIntersection(ray As Math.Ray) As Math.SurfacePoint Implements Math.ISurface.FirstIntersection
        Return Me.Surface.FirstIntersection(ray)
    End Function

    Public Function FirstMaterialIntersection(ray As Math.Ray) As SurfacePoint(Of TMaterial) Implements ISurface(Of TMaterial).FirstMaterialIntersection
        Dim intersection = Me.FirstIntersection(ray)

        If intersection Is Nothing Then Return Nothing

        Return MaterialSurfacePointFromSurfacePoint(intersection)
    End Function
End Class
