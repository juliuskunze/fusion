Public Class SurfacePoint(Of TMaterial)
    Inherits SurfacePoint

    Private _Material As TMaterial

    Public Sub New(location As Vector3D, normal As Vector3D, material As TMaterial)
        MyBase.New(location:=location, normal:=normal)
        _Material = material
    End Sub

    Public Sub New(surfacePoint As SurfacePoint, material As TMaterial)
        MyBase.New(Location:=surfacePoint.Location, Normal:=surfacePoint.NormalizedNormal)
        _Material = material
    End Sub


    Public ReadOnly Property Material As TMaterial
        Get
            Return _Material
        End Get
    End Property


End Class
