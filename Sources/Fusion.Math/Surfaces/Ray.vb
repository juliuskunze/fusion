Public Structure Ray
    Private ReadOnly _NormalizedDirection As Vector3D
    Public ReadOnly Property NormalizedDirection As Vector3D
        Get
            Return _NormalizedDirection
        End Get
    End Property

    Private ReadOnly _Origin As Vector3D
    Public ReadOnly Property Origin As Vector3D
        Get
            Return _Origin
        End Get
    End Property

    Public Sub New(origin As Vector3D, direction As Vector3D)
        _Origin = origin
        _NormalizedDirection = direction.Normalized
    End Sub

    Public Function PointOnRay(distanceFromOrigin As Double) As Vector3D
        If distanceFromOrigin < 0 Then Throw New ArgumentException("The distance from start location must be positve.")

        Return Origin + distanceFromOrigin * NormalizedDirection
    End Function

    Public Overrides Function ToString() As String
        Return String.Format("Ray(origin : {0}, direction : {1})", Origin.ToString, NormalizedDirection.ToString)
    End Function
End Structure
