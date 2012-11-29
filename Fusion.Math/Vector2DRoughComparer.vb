Public Class Vector3DRoughComparer
    Implements IEqualityComparer(Of Vector3D)

    Private ReadOnly _SquaredMaxDeviation As Double

    Public Sub New(maxDeviation As Double)
        _SquaredMaxDeviation = maxDeviation ^ 2
    End Sub

    Public Shadows Function Equals(x As Vector3D, y As Vector3D) As Boolean Implements IEqualityComparer(Of Vector3D).Equals
        Return (x - y).LengthSquared < _SquaredMaxDeviation
    End Function

    Public Overloads Function GetHashCode(obj As Vector3D) As Integer Implements IEqualityComparer(Of Vector3D).GetHashCode
        Return obj.GetHashCode
    End Function
End Class
