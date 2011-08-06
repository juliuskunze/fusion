Public Class Cylinder
    Implements ISurfacedPointSet3D

    Private _InfiniteCylinder As InfiniteCylinder
    Public ReadOnly Property Radius As Double
        Get
            Return _InfiniteCylinder.Radius
        End Get
    End Property
    Public ReadOnly Property StartCenter As Vector3D
        Get
            Return _InfiniteCylinder.Origin
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

    Private _StartPlane As Plane
    Private _EndPlane As Plane
    Private _Surface As ISurface

    Public Sub New(startCenter As Vector3D, endCenter As Vector3D, radius As Double)
        _EndCenter = endCenter

        Dim startToEndVector = endCenter - startCenter

        _InfiniteCylinder = New InfiniteCylinder(origin:=startCenter, direction:=startToEndVector, radius:=radius)
        _StartPlane = New Plane(location:=startCenter, normal:=-startToEndVector)
        _EndPlane = New Plane(location:=endCenter, normal:=startToEndVector)

        Dim startCircle = New TruncatedSurface(_StartPlane, New InversePointSet3D(_InfiniteCylinder))
        Dim endCircle = New TruncatedSurface(_EndPlane, New InversePointSet3D(_InfiniteCylinder))
        Dim girthedArea = New TruncatedSurface(baseSurface:=_InfiniteCylinder, truncatingPointSet:=New InversePointSet3D(New LinkedPointSets3D(_StartPlane, _EndPlane, linkOperator:=Function(a, b) a AndAlso b)))

        _Surface = New Surfaces From {girthedArea, startCircle, endCircle}
    End Sub

    Public Function Contains(point As Vector3D) As Boolean Implements IPointSet3D.Contains
        Return _StartPlane.CoveredHalfSpaceContains(point) AndAlso
               _EndPlane.CoveredHalfSpaceContains(point) AndAlso
               _InfiniteCylinder.Contains(point)
    End Function

    Public Function FirstIntersection(ray As Ray) As SurfacePoint Implements ISurface.FirstIntersection
        Return _Surface.FirstIntersection(ray)
    End Function

    Public Function Intersections(ray As Ray) As System.Collections.Generic.IEnumerable(Of SurfacePoint) Implements ISurface.Intersections
        Return _Surface.Intersections(ray)
    End Function
End Class
