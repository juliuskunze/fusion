Public Class TextLocation
    Public Sub New(startIndex As Integer, length As Integer)
        _StartIndex = startIndex
        _Length = length
    End Sub

    Private ReadOnly _StartIndex As Integer
    Public ReadOnly Property StartIndex As Integer
        Get
            Return _StartIndex
        End Get
    End Property

    Private ReadOnly _Length As Integer
    Public ReadOnly Property Length As Integer
        Get
            Return _Length
        End Get
    End Property

    Public ReadOnly Property EndIndex As Integer
        Get
            Return _StartIndex + _Length
        End Get
    End Property

    Public Function SubLocation(startIndex As Integer) As TextLocation
        Return Me.SubLocation(startIndex:=startIndex,
                              length:=_Length - startIndex)
    End Function

    Public Function SubLocation(startIndex As Integer, length As Integer) As TextLocation
        Return New TextLocation(startIndex:=Me.StartIndex + startIndex,
                                length:=length)
    End Function

    Public Function ContainsCharIndex(index As Integer) As Boolean
        Return _StartIndex <= index AndAlso index < Me.EndIndex
    End Function

    Public Function ContainsPointer(pointer As Integer) As Boolean
        Return _StartIndex <= pointer AndAlso pointer <= Me.EndIndex
    End Function

    Public Function ContainsRange(range As TextLocation) As Boolean
        Return Me.ContainsPointer(range.StartIndex) AndAlso Me.ContainsPointer(range.EndIndex)
    End Function

    Public Overrides Function Equals(obj As Object) As Boolean
        Dim other = TryCast(obj, TextLocation)
        If other Is Nothing Then Return False

        Return Me._StartIndex = other._StartIndex AndAlso Me._Length = other._Length
    End Function

    Public Shared Operator =(l1 As TextLocation, l2 As TextLocation) As Boolean
        Return l1.Equals(l2)
    End Operator

    Public Shared Operator <>(l1 As TextLocation, l2 As TextLocation) As Boolean
        Return Not l1 = l2
    End Operator
End Class
