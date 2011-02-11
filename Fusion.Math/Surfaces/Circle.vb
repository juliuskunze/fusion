Public Class Circle
    Implements ISurface

    Public Sub New(ByVal center As Vector3D, ByVal normal As Vector3D, ByVal radius As Double)
        Me.Center = center
        Me.Normal = normal
        Me.Radius = radius
    End Sub

    Public Property Center As Vector3D
        Get
            Return _containingPlane.Location
        End Get
        Set(ByVal value As Vector3D)
            _containingPlane.Location = value
        End Set
    End Property

    Public Property Radius As Double

    Public WriteOnly Property Normal As Vector3D
        Set(ByVal value As Vector3D)
            _containingPlane.Normal = value.Normalized
        End Set
    End Property

    Public ReadOnly Property NormalizedNormal As Vector3D
        Get
            Return _containingPlane.NormalizedNormal
        End Get
    End Property

    Private Sub adaptContainingPlane(ByVal center As Vector3D, ByVal normal As Vector3D)
        _containingPlane = New Plane(location:=center, normal:=normal)
    End Sub
    Private _containingPlane As Plane

    Public Function Intersection(ByVal ray As Ray) As SurfacePoint Implements ISurface.FirstIntersection
        Dim planeIntersection = _containingPlane.Intersection(ray)

        If planeIntersection Is Nothing Then Return Nothing

        If (Me.Center - planeIntersection.NormalizedNormal).Length > Me.Radius Then Return Nothing

        Return planeIntersection
    End Function

    Public Function Intersections(ByVal ray As Ray) As System.Collections.Generic.IEnumerable(Of SurfacePoint) Implements ISurface.Intersections
        Dim intersection = Me.Intersection(ray)

        If intersection Is Nothing Then Return Enumerable.Empty(Of SurfacePoint)()
        Return {intersection}
    End Function
End Class