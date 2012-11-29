Public Class MaterialSurfaceEvent(Of TMaterial)
    Inherits SurfacePoint(Of TMaterial)

    Private ReadOnly _Time As Double

    Public Sub New(surfacePoint As SurfacePoint(Of TMaterial), time As Double)
        MyBase.New(surfacePoint:=surfacePoint, Material:=surfacePoint.Material)
        _Time = time
    End Sub

    Public ReadOnly Property [Event] As SpaceTimeEvent
        Get
            Return New SpaceTimeEvent(_Time, Location)
        End Get
    End Property
End Class
