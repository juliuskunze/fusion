Public Interface ICoordinateSystemToArray(Of T)
    Property Array() As T(,)
    ReadOnly Property ColumnCount() As Integer
    ReadOnly Property RowCount() As Integer
    Function PointFromRowColumn(ByVal column As Integer, ByVal row As Integer) As Vector2D
    Property GridLength() As Double
    Property Size() As Vector2D
    Property LowerVertex() As Vector2D
End Interface
