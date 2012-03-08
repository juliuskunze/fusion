Public Class Vector2DRoughComparer
    Implements IEqualityComparer(Of Vector2D)

    Private ReadOnly _MaxDeviation As Double

    Public Sub New(maxDeviation As Double)
        _MaxDeviation = maxDeviation
    End Sub

    Public Overloads Function Equals(x As Vector2D, y As Vector2D) As Boolean Implements IEqualityComparer(Of Vector2D).Equals
        Return (x - y).LengthSquared < _MaxDeviation ^ 2
    End Function

    Public Overloads Function GetHashCode(obj As Vector2D) As Integer Implements IEqualityComparer(Of Vector2D).GetHashCode
        Return obj.GetHashCode
    End Function

End Class
