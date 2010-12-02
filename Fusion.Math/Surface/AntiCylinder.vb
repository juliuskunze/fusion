Public Class AntiCylinder
    Implements ISurfacedPointSet3D

    Private _infiniteAntiCylinder As InfiniteAntiCylinder
    Public ReadOnly Property Radius As Double
        Get
            Return _infiniteAntiCylinder.Radius
        End Get
    End Property
    Public ReadOnly Property StartCenter As Vector3D
        Get
            Return _infiniteAntiCylinder.Origin
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

        _infiniteAntiCylinder = New InfiniteAntiCylinder(origin:=startCenter, direction:=startToEndVector, radius:=radius)
        _startPlane = New Plane(location:=startCenter, normal:=startToEndVector)
        _endPlane = New Plane(location:=endCenter, normal:=-startToEndVector)

        Dim startCircle = New TruncatedSurface(_startPlane, _infiniteAntiCylinder)
        Dim endCircle = New TruncatedSurface(_endPlane, _infiniteAntiCylinder)
        Dim girthedArea = New TruncatedSurface(baseSurface:=_infiniteAntiCylinder, truncatingPointSet:=New LinkedPointSets3D(linkOperator:=Function(a, b) Not a AndAlso Not b) From {_startPlane, _endPlane})

        _surface = New Surfaces From {girthedArea, startCircle, endCircle}
    End Sub

    Public Function Contains(ByVal point As Vector3D) As Boolean Implements IPointSet3D.Contains
        Return _startPlane.CoveredHalfSpaceContains(point) OrElse
               _endPlane.CoveredHalfSpaceContains(point) OrElse
               _infiniteAntiCylinder.Contains(point)
    End Function

    Public Function FirstIntersection(ByVal ray As Ray) As SurfacePoint Implements ISurface.FirstIntersection
        Return _surface.FirstIntersection(ray)
    End Function

    Public Function Intersections(ByVal ray As Ray) As System.Collections.Generic.IEnumerable(Of SurfacePoint) Implements ISurface.Intersections
        Return _surface.Intersections(ray)
    End Function
End Class

