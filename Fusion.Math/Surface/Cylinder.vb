Public Class Cylinder
    Implements ISurfacedPointSet3D

    Private _infiniteCylinder As InfiniteCylinder
    Public ReadOnly Property Radius As Double
        Get
            Return _infiniteCylinder.Radius
        End Get
    End Property
    Public ReadOnly Property StartCenter As Vector3D
        Get
            Return _infiniteCylinder.Origin
        End Get
    End Property

    Public ReadOnly Property Length As Double
        Get
            Return (Me.StartCenter - Me.EndCenter).Length
        End Get
    End Property

    Private _endCenter As Vector3D
    Public ReadOnly Property EndCenter As Vector3D
        Get
            Return _endCenter
        End Get
    End Property

    Private _startPlane As Plane
    Private _endPlane As Plane
    Private _surface As ISurface

    Public Sub New(ByVal startCenter As Vector3D, ByVal endCenter As Vector3D, ByVal radius As Double)
        _endCenter = endCenter

        Dim startToEndVector = endCenter - startCenter

        _infiniteCylinder = New InfiniteCylinder(origin:=startCenter, direction:=startToEndVector, radius:=radius)
        _startPlane = New Plane(location:=startCenter, normal:=-startToEndVector)
        _endPlane = New Plane(location:=endCenter, normal:=startToEndVector)

        Dim startCircle = New TruncatedSurface(_startPlane, New InversePointSet3D(_infiniteCylinder))
        Dim endCircle = New TruncatedSurface(_endPlane, New InversePointSet3D(_infiniteCylinder))
        Dim girthedArea = New TruncatedSurface(baseSurface:=_infiniteCylinder, truncatingPointSet:=New InversePointSet3D(New LinkedPointSets3D(_startPlane, _endPlane, linkOperator:=Function(a, b) a AndAlso b)))

        _surface = New Surfaces From {girthedArea, startCircle, endCircle}
    End Sub

    Public Function Contains(ByVal point As Vector3D) As Boolean Implements IPointSet3D.Contains
        Return _startPlane.CoveredHalfSpaceContains(point) AndAlso
               _endPlane.CoveredHalfSpaceContains(point) AndAlso
               _infiniteCylinder.Contains(point)
    End Function

    Public Function FirstIntersection(ByVal ray As Ray) As SurfacePoint Implements ISurface.FirstIntersection
        Return _surface.FirstIntersection(ray)
    End Function

    Public Function Intersections(ByVal ray As Ray) As System.Collections.Generic.IEnumerable(Of SurfacePoint) Implements ISurface.Intersections
        Return _surface.Intersections(ray)
    End Function
End Class
