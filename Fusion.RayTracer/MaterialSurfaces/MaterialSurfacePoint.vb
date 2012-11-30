Public Class SurfacePoint(Of TLight)
    Inherits SurfacePoint

    Private ReadOnly _SpaceTimeEvent As SpaceTimeEvent
    Private ReadOnly _Material As Material2D(Of TLight)

    Public Sub New(spaceTimeEvent As SpaceTimeEvent, normal As Vector3D, material As Material2D(Of TLight))
        MyBase.New(location:=spaceTimeEvent.Location, normal:=normal)
        _SpaceTimeEvent = spaceTimeEvent
        _Material = material
    End Sub

    Public Sub New(surfacePoint As SurfacePoint, material As Material2D(Of TLight), time As Double)
        MyBase.New(Location:=surfacePoint.Location, Normal:=surfacePoint.NormalizedNormal)
        _SpaceTimeEvent = New SpaceTimeEvent(surfacePoint.Location, time)
        _Material = material
    End Sub


    Public ReadOnly Property Material As Material2D(Of TLight)
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
