Public Class CoordinateSystemToArray(Of T)
    Implements ICoordinateSystemToArray(Of T)

    Private _LowerVertex As Vector2D
    Public Property LowerVertex As Vector2D Implements ICoordinateSystemToArray(Of T).LowerVertex
        Get
            Return _LowerVertex
        End Get
        Set(ByVal value As Vector2D)
            _LowerVertex = value
            Me.RefreshGridSize()
        End Set
    End Property

    Private _Size As Vector2D
    Public Property Size As Vector2D Implements ICoordinateSystemToArray(Of T).Size
        Get
            Return _Size
        End Get
        Set(ByVal value As Vector2D)
            _Size = value
            Me.RefreshGridSize()
        End Set
    End Property

    Private _GridLength As Double
    Public Property GridLength As Double Implements ICoordinateSystemToArray(Of T).GridLength
        Get
            Return _GridLength
        End Get
        Set(ByVal value As Double)
            _GridLength = value
            Me.RefreshGridSize()
        End Set
    End Property

    Public Property Array As T(,) Implements ICoordinateSystemToArray(Of T).Array

    Public Sub New(ByVal lowerVertex As Vector2D, ByVal size As Vector2D, ByVal gridLength As Double)
        _LowerVertex = lowerVertex
        _Size = size
        _GridLength = gridLength

        Me.RefreshGridSize()
    End Sub

    Protected Sub RefreshGridSize()
        ReDim Array(Me.ColumnCount() - 1, Me.RowCount() - 1)
    End Sub

    Public ReadOnly Property RowCount() As Integer Implements ICoordinateSystemToArray(Of T).RowCount
        Get
            Return CInt(Size.Y / GridLength) + 1
        End Get
    End Property

    Public ReadOnly Property ColumnCount() As Integer Implements ICoordinateSystemToArray(Of T).ColumnCount
        Get
            Return CInt(Size.X / GridLength) + 1
        End Get
    End Property

    Public Function PointFromRowColumn(ByVal column As Integer, ByVal row As Integer) As Vector2D Implements ICoordinateSystemToArray(Of T).PointFromRowColumn
        Return New Vector2D(Me.LowerVertex.X + Me.GridLength * column, Me.LowerVertex.Y + Me.GridLength * row)
    End Function

End Class
