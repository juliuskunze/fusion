Public Structure Ray

    Private ReadOnly _normalizedDirection As Vector3D
    Public ReadOnly Property NormalizedDirection As Vector3D
        Get
            Return _normalizedDirection
        End Get
    End Property

    Private ReadOnly _origin As Vector3D
    Public ReadOnly Property Origin As Vector3D
        Get
            Return _origin
        End Get
    End Property

    Public Sub New(ByVal origin As Vector3D, ByVal direction As Vector3D)
        _origin = origin
        _normalizedDirection = direction.Normalized
    End Sub

    Public Function PointOnRay(ByVal distanceFromOrigin As Double) As Vector3D
        If distanceFromOrigin < 0 Then Throw New ArgumentException("The distance from start location must be positve.")

        Return Me.Origin + distanceFromOrigin * Me.NormalizedDirection
    End Function

End Structure
