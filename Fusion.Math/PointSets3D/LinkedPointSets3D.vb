Public Class LinkedPointSets3D
    Implements IPointSet3D

    Public Property PointSet1 As IPointSet3D
    Public Property PointSet2 As IPointSet3D

    Public Property LinkOperator As Func(Of Boolean, Boolean, Boolean)

    Public Sub New(pointSet1 As IPointSet3D, pointSet2 As IPointSet3D, linkOperator As Func(Of Boolean, Boolean, Boolean))
        Me.PointSet1 = pointSet1
        Me.PointSet2 = pointSet2
        Me.LinkOperator = linkOperator
    End Sub

    Public Overloads Function Contains(point As Vector3D) As Boolean Implements IPointSet3D.Contains
        Return Me.LinkOperator.Invoke(Me.PointSet1.Contains(point), Me.PointSet2.Contains(point))
    End Function

End Class