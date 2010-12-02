Public Class DirectionalLightSource
    Implements ILightSource

    Public Sub New(ByVal direction As Vector3D, ByVal color As Color)
        Me.New(direction, New ExactColor(color))
    End Sub

    Public Sub New(ByVal direction As Vector3D, ByVal color As ExactColor)
        Me.Direction = direction
        Me.Color = color
    End Sub

    Public WriteOnly Property Direction As Vector3D
        Set(ByVal value As Vector3D)
            _normalizedDirection = value.Normalized
        End Set
    End Property

    Private _normalizedDirection As Vector3D
    Public ReadOnly Property NormalizedDirection As Vector3D
        Get
            Return _normalizedDirection
        End Get
    End Property

    Public Property Color As ExactColor

    Public Function LightColor(ByVal surfacePoint As SurfacePoint) As ExactColor Implements ILightSource.LightColor
        Dim valueFactor = surfacePoint.NormalizedNormal * _normalizedDirection

        If valueFactor < 0 Then
            Return ExactColor.Black
        End If

        Return Me.Color * valueFactor
    End Function

End Class
