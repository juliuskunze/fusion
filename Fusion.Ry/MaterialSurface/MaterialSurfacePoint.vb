Public Class MaterialSurfacePoint(Of MaterialType)
    Inherits SurfacePoint

    Public Sub New(ByVal location As Vector3D, ByVal normal As Vector3D, ByVal material As MaterialType)
        MyBase.New(location:=location, normal:=normal)
        Me.Material = material
    End Sub

    Public Sub New(ByVal surfacePoint As SurfacePoint, ByVal material As MaterialType)
        MyBase.New(Location:=surfacePoint.Location, Normal:=surfacePoint.NormalizedNormal)
        Me.Material = material
    End Sub

    Public Material As MaterialType

End Class
