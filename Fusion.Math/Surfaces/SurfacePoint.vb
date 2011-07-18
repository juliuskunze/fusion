﻿Public Class SurfacePoint

    Public Sub New(ByVal location As Vector3D, ByVal normal As Vector3D)
        _Location = location
        _NormalizedNormal = normal.Normalized
    End Sub

    Private ReadOnly _Location As Vector3D
    Public ReadOnly Property Location As Vector3D
        Get
            Return _Location
        End Get
    End Property

    Private _NormalizedNormal As Vector3D
    Public ReadOnly Property NormalizedNormal As Vector3D
        Get
            Return _NormalizedNormal
        End Get
    End Property

End Class
