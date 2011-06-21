Public Class DirectionalLightSource(Of TLight As {ILight(Of TLight), New})
    Implements ILightSource(Of TLight)

    Public Sub New(ByVal direction As Vector3D, ByVal baseLight As TLight)
        Me.Direction = direction
        Me.BaseLight = baseLight
    End Sub

    Public WriteOnly Property Direction As Vector3D
        Set(ByVal value As Vector3D)
            _NormalizedDirection = value.Normalized
        End Set
    End Property

    Private _NormalizedDirection As Vector3D
    Public ReadOnly Property NormalizedDirection As Vector3D
        Get
            Return _NormalizedDirection
        End Get
    End Property

    Public Property BaseLight As TLight

    Public Function LightColor(ByVal surfacePoint As SurfacePoint) As TLight Implements ILightSource(Of TLight).GetLight
        Dim valueFactor = surfacePoint.NormalizedNormal * _NormalizedDirection

        If valueFactor < 0 Then
            Return New TLight
        End If

        Return Me.BaseLight.MultiplyBrighness(valueFactor)
    End Function

End Class
