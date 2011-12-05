Public Class PointSet3D
    Implements IPointSet3D

    Private ReadOnly _PointSelector As Func(Of Vector3D, Boolean)

    Public Sub New(pointSelector As Func(Of Vector3D, Boolean))
        _PointSelector = pointSelector
    End Sub

    Public Function Contains(point As Vector3D) As Boolean Implements IPointSet3D.Contains
        Return _PointSelector(point)
    End Function

    Public Function Inverse() As PointSet3D
        Return New PointSet3D(Function(point) Not Me.Contains(point))
    End Function

End Class
