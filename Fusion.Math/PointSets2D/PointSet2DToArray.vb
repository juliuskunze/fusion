Public Class PointSet2DToArray
    Inherits CoordinateSystemToArray(Of Boolean)

    Public Property PointSet As IPointSet2D

    Public Sub New(ByVal lowerVertex As Vector2D, ByVal size As Vector2D, ByVal gridLength As Double, ByVal pointSet As IPointSet2D)
        MyBase.New(lowerVertex:=lowerVertex, size:=size, gridLength:=gridLength)
        Me.PointSet = pointSet

        Me.FillGrid()
    End Sub

    Public Sub FillGrid()
        For columnIndex = 0 To Me.ColumnCount() - 1
            For rowIndex = 0 To Me.RowCount() - 1
                Array(columnIndex, rowIndex) = Me.PointSet.Contains(PointFromRowColumn(columnIndex, rowIndex))
            Next
        Next
    End Sub
End Class
