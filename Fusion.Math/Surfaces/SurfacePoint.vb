Public Class SurfacePoint

    Public Sub New(ByVal location As Vector3D, ByVal normal As Vector3D)
        Me.Location = location
        Me.Normal = normal
    End Sub

    Public Property Location As Vector3D

    Public WriteOnly Property Normal As Vector3D
        Set(ByVal value As Vector3D)
            _normalizedNormal = value.Normalized
        End Set
    End Property

    Private _normalizedNormal As Vector3D
    Public ReadOnly Property NormalizedNormal As Vector3D
        Get
            Return _normalizedNormal
        End Get
    End Property

End Class
