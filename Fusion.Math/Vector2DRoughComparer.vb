Public Class Vector2DRoughComparer
    Implements IEqualityComparer(Of Vector2D)

    Private ReadOnly _MaxDeviation As Double

    Public Sub New(ByVal maxDeviation As Double)
        _MaxDeviation = maxDeviation
    End Sub

    Public Function Equals1(ByVal x As Vector2D, ByVal y As Vector2D) As Boolean Implements System.Collections.Generic.IEqualityComparer(Of Vector2D).Equals
        Return (x - y).LengthSquared < _MaxDeviation ^ 2
    End Function

    Public Function GetHashCode1(ByVal obj As Vector2D) As Integer Implements System.Collections.Generic.IEqualityComparer(Of Vector2D).GetHashCode
        Return Me.GetHashCode
    End Function

End Class
