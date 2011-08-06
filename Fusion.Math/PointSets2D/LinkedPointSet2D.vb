Public Class LinkedPointSet2D
    Implements IPointSet2D

    Public Property PointSet1 As IPointSet2D
    Public Property PointSet2 As IPointSet2D
    Public Property LinkOperator As Func(Of Boolean, Boolean, Boolean)

    Public Sub New(pointSet1 As IPointSet2D, pointSet2 As IPointSet2D, linkOperator As Func(Of Boolean, Boolean, Boolean))
        Me.PointSet1 = pointSet1
        Me.PointSet2 = pointSet2
        Me.LinkOperator = linkOperator
    End Sub

    Public Function Contains(point As Vector2D) As Boolean Implements IPointSet2D.Contains
        Return Me.LinkOperator.Invoke(Me.PointSet1.Contains(point), Me.PointSet2.Contains(point))
    End Function

End Class