Public Class SurfacePoint(Of TMaterial)
    Inherits SurfacePoint

    Private ReadOnly _SpaceTimeEvent As SpaceTimeEvent
    Private ReadOnly _Material As TMaterial

    Public Sub New(spaceTimeEvent As SpaceTimeEvent, normal As Vector3D, material As TMaterial)
        MyBase.New(location:=spaceTimeEvent.Location, normal:=normal)
        _SpaceTimeEvent = spaceTimeEvent
        _Material = material
    End Sub

    Public Sub New(surfacePoint As SurfacePoint, material As TMaterial, time As Double)
        MyBase.New(Location:=surfacePoint.Location, Normal:=surfacePoint.NormalizedNormal)
        _SpaceTimeEvent = New SpaceTimeEvent(surfacePoint.Location, time)
        _Material = material
    End Sub


    Public ReadOnly Property Material As TMaterial
        Get
            Return _Material
        End Get
    End Property

    Public ReadOnly Property SpaceTimeEvent As SpaceTimeEvent
        Get
            Return _SpaceTimeEvent
        End Get
    End Property
End Class
