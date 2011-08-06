Public Class SurfacePoint(Of TMaterial)
    Inherits SurfacePoint

    Public Sub New(location As Vector3D, normal As Vector3D, material As TMaterial)
        MyBase.New(location:=location, normal:=normal)
        Me.Material = material
    End Sub

    Public Sub New(surfacePoint As SurfacePoint, material As TMaterial)
        MyBase.New(Location:=surfacePoint.Location, Normal:=surfacePoint.NormalizedNormal)
        Me.Material = material
    End Sub

    Public Material As TMaterial

End Class
