Public Class SurfacePoint

    Public Sub New(ByVal location As Vector3D, ByVal normal As Vector3D)
        Me.Location = location
        Me.Normal = normal
    End Sub

    Public Property Location As Vector3D

    Public WriteOnly Property Normal As Vector3D
        Set(ByVal value As Vector3D)
            _NormalizedNormal = value.Normalized
        End Set
    End Property

    Private _NormalizedNormal As Vector3D
    Public ReadOnly Property NormalizedNormal As Vector3D
        Get
            Return _NormalizedNormal
        End Get
    End Property

End Class
