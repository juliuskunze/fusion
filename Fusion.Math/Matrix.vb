Public Class Matrix
    Protected Friend _Elements(,) As Double

    Public Sub New(width As Integer, height As Integer)
        ReDim _Elements(height - 1, width - 1)
    End Sub

    Public Sub New(elements(,) As Double)
        _Elements = elements
    End Sub

    Public ReadOnly Property Width() As Integer
        Get
            Return _Elements.GetLength(1)
        End Get
    End Property

    Public ReadOnly Property Height() As Integer
        Get
            Return _Elements.GetLength(0)
        End Get
    End Property

    Default Public Property Element(rowIndex As Integer, columnIndex As Integer) As Double
        Get
            Return _Elements(rowIndex, columnIndex)
        End Get
        Set(value As Double)
            _Elements(rowIndex, columnIndex) = value
        End Set
    End Property

    Public Function ToArray() As Double(,)
        Dim array As Double(,) = {{0}}
        System.Array.Copy(sourceArray:=_Elements,
                          destinationArray:=array,
                          length:=_Elements.Length)
        Return array
    End Function


    Private Function SwapRows(m As Integer, n As Integer, ByRef determinant As Double) As SquareMatrix
        determinant = -determinant
        Return SwapRows(m, n)
    End Function

    Public Function SwapRows(m As Integer, n As Integer) As SquareMatrix
        Dim resultArray = DirectCast(_Elements.Clone(), Double(,))

        For iColumn = 0 To Me.Width - 1
            resultArray(m, iColumn) = _Elements(n, iColumn)
            resultArray(n, iColumn) = _Elements(m, iColumn)
        Next

        Return New SquareMatrix(resultArray)
    End Function

    Private Function MultiplyRow(rowIndex As Integer, factor As Double, ByRef determinant As Double) As SquareMatrix
        determinant *= factor
        Return MultiplyRow(rowIndex, factor)
    End Function

    Public Function MultiplyRow(rowIndex As Integer, factor As Double) As SquareMatrix
        Dim resultArray = DirectCast(_Elements.Clone(), Double(,))

        For iColumn = 0 To Me.Width - 1
            resultArray(rowIndex, iColumn) = factor * _Elements(rowIndex, iColumn)
        Next

        Return New SquareMatrix(resultArray)
    End Function

    Public Function MultiplyAddRow(sourceRowIndex As Integer, targetRowIndex As Integer, factor As Double) As SquareMatrix
        Dim resultArray = DirectCast(_Elements.Clone(), Double(,))

        For iColumn = 0 To Me.Width - 1
            resultArray(targetRowIndex, iColumn) = factor * _Elements(sourceRowIndex, iColumn)
        Next

        Return New SquareMatrix(resultArray)
    End Function


    Public Shared Operator *(m1 As Matrix, m2 As Matrix) As Matrix
        If m1.Width <> m2.Height Then
            Throw New ArgumentException("Width of first matrix has to be height of second matrix.")
        End If

        Dim product = New Matrix(m2.Width, m1.Height)
        For columnIndex = 0 To m2.Width - 1
            For rowIndex = 0 To m1.Height - 1

                Dim element = 0.0
                For i = 0 To m1.Width - 1
                    element += m1(rowIndex, i) * m2(i, columnIndex)
                Next
                product(rowIndex, columnIndex) = element

            Next
        Next
        Return product

    End Operator


    Public Shared Operator =(m1 As Matrix, m2 As Matrix) As Boolean

        If m1.Width <> m2.Width OrElse _
           m1.Height <> m2.Height Then
            Return False
        End If

        For iColumn = 0 To m1.Width - 1
            For iRow = 0 To m1.Height - 1
                If m1.Element(iRow, iColumn) <> m2.Element(iRow, iColumn) Then
                    Return False
                End If
            Next
        Next

        Return True

    End Operator

    Public Shared Operator <>(m1 As Matrix, m2 As Matrix) As Boolean
        Return Not m1 = m2
    End Operator
End Class