Public Class AntiCylinder
    Implements ISurfacedPointSet3D

    Private _InfiniteAntiCylinder As InfiniteAntiCylinder
    Public ReadOnly Property Radius As Double
        Get
            Return _InfiniteAntiCylinder.Radius
        End Get
    End Property
    Public ReadOnly Property StartCenter As Vector3D
        Get
            Return _InfiniteAntiCylinder.Origin
        End Get
    End Property

    Public ReadOnly Property Length As Double
        Get
            Return (Me.StartCenter - Me.EndCenter).Length
        End Get
    End Property

    Private _EndCenter As Vector3D
    Public ReadOnly Property EndCenter As Vector3D
        Get
            Return _EndCenter
        End Get
    End Property

    Private ReadOnly _StartPlane As Plane
    Private ReadOnly _EndPlane As Plane
    Private ReadOnly _Surface As ISurface

    Public Sub New(cylinder As Cylinder)
        Me.New(StartCenter:=cylinder.StartCenter, EndCenter:=cylinder.EndCenter, Radius:=cylinder.Radius)
    End Sub

    Public Sub New(startCenter As Vector3D, endCenter As Vector3D, radius As Double)
        _EndCenter = endCenter

        Dim startToEndVector = endCenter - startCenter

        _InfiniteAntiCylinder = New InfiniteAntiCylinder(origin:=startCenter, direction:=startToEndVector, radius:=radius)
        _StartPlane = New Plane(location:=startCenter, normal:=startToEndVector)
        _EndPlane = New Plane(location:=endCenter, normal:=-startToEndVector)

        Dim startCircle = New TruncatedSurface(_StartPlane, truncatingPointSet:=_InfiniteAntiCylinder)
        Dim endCircle = New TruncatedSurface(_EndPlane, truncatingPointSet:=_InfiniteAntiCylinder)
        Dim girthedArea = New TruncatedSurface(_InfiniteAntiCylinder, truncatingPointSet:=New LinkedPointSets3D(_StartPlane, _EndPlane, linkOperator:=Function(a, b) a OrElse b))

        _Surface = New Surfaces From {girthedArea, startCircle, endCircle}
    End Sub

    Public Function Contains(point As Vector3D) As Boolean Implements IPointSet3D.Contains
        Return _StartPlane.CoveredHalfSpaceContains(point) OrElse
               _EndPlane.CoveredHalfSpaceContains(point) OrElse
               _InfiniteAntiCylinder.Contains(point)
    End Function

    Public Function FirstIntersection(ray As Ray) As SurfacePoint Implements ISurface.FirstIntersection
        Return _Surface.FirstIntersection(ray)
    End Function

    Public Function Intersections(ray As Ray) As System.Collections.Generic.IEnumerable(Of SurfacePoint) Implements ISurface.Intersections
        Return _Surface.Intersections(ray)
    End Function
End Class

