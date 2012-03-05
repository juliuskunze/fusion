﻿Public Class Vector3DRoughComparer
    Implements IEqualityComparer(Of Vector3D)

    Private ReadOnly _MaxDeviation As Double

    Public Sub New()
        Me.New(10 ^ -10)
    End Sub

    Public Sub New(maxDeviation As Double)
        _MaxDeviation = maxDeviation
    End Sub

    Public Overloads Function Equals(x As Vector3D, y As Vector3D) As Boolean Implements IEqualityComparer(Of Vector3D).Equals
        Return (x - y).LengthSquared < _MaxDeviation ^ 2
    End Function

    Public Overloads Function GetHashCode(obj As Vector3D) As Integer Implements IEqualityComparer(Of Vector3D).GetHashCode
        Return Me.GetHashCode
    End Function

End Class
