Public Class RoughDoubleComparer
    Implements IEqualityComparer(Of Double)

    Private ReadOnly _MaxDeviation As Double

    Public Sub New(maxDeviation As Double)
        _MaxDeviation = maxDeviation
    End Sub

    Public Shadows Function Equals(x As Double, y As Double) As Boolean Implements IEqualityComparer(Of Double).Equals
        Return Abs(x - y) < _MaxDeviation
    End Function

    Public Overloads Function GetHashCode(obj As Double) As Integer Implements IEqualityComparer(Of Double).GetHashCode
        Return obj.GetHashCode
    End Function
End Class
