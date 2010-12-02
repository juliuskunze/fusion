Public Class Ray

    Public Sub New(ByVal origin As Vector3D, ByVal direction As Vector3D)
        Me.Origin = origin
        Me.Direction = direction
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

    Public Property Origin As Vector3D

    Public Function PointOnRay(ByVal distanceFromOrigin As Double) As Vector3D
        If distanceFromOrigin < 0 Then Throw New ArgumentException("The distance from start location must be positve.")

        Return Me.Origin + distanceFromOrigin * Me.NormalizedDirection
    End Function

End Class
