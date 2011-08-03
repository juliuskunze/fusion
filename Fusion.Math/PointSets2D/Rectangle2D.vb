Public Class Rectangle2D
    Implements IPointSet2D

    Private ReadOnly _Size As Vector2D
    Public ReadOnly Property Size As Vector2D
        Get
            Return _Size
        End Get
    End Property

    Private ReadOnly _Location As Vector2D
    Public ReadOnly Property Location As Vector2D
        Get
            Return _Location
        End Get
    End Property

    Public ReadOnly Property Left As Double
        Get
            Return _Location.X
        End Get
    End Property

    Public ReadOnly Property Right As Double
        Get
            Return _Location.X + _Size.X
        End Get
    End Property

    Public ReadOnly Property Top As Double
        Get
            Return _Location.Y
        End Get
    End Property

    Public ReadOnly Property Bottom As Double
        Get
            Return _Location.Y + _Size.Y
        End Get
    End Property


    Public Sub New(ByVal location As Vector2D, ByVal size As Vector2D)
        _Location = location
        _Size = size
    End Sub

    Public Function Contains(ByVal point As Vector2D) As Boolean Implements IPointSet2D.Contains
        Return point.X >= _Location.X AndAlso point.Y >= _Location.Y AndAlso
               point.X <= _Location.X + _Size.X AndAlso point.Y <= _Location.Y + _Size.Y
    End Function
End Class
