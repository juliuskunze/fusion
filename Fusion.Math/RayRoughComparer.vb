Public Class RayRoughComparer
    Implements IEqualityComparer(Of Ray)

    Private ReadOnly _VectorComparer As Vector3DRoughComparer

    Public Sub New(maxDeviation As Double)
        _VectorComparer = New Vector3DRoughComparer(maxDeviation)
    End Sub

    Public Shadows Function Equals(x As Ray, y As Ray) As Boolean Implements IEqualityComparer(Of Ray).Equals
        Return _VectorComparer.Equals(x.Origin, y.Origin) AndAlso _VectorComparer.Equals(x.NormalizedDirection, y.NormalizedDirection)
    End Function

    Public Overloads Function GetHashCode(obj As Ray) As Integer Implements IEqualityComparer(Of Ray).GetHashCode
        Return obj.GetHashCode
    End Function
End Class