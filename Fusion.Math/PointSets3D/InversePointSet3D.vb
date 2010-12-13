﻿Public Class InversePointSet3D
    Implements IPointSet3D

    Public Property PointSet As IPointSet3D

    Public Sub New(ByVal pointSet As IPointSet3D)
        Me.PointSet = pointSet
    End Sub

    Public Function Contains(ByVal point As Vector3D) As Boolean Implements IPointSet3D.Contains
        Return Not Me.PointSet.Contains(point)
    End Function
End Class