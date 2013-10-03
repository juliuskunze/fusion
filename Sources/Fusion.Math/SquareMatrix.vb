Public Class SquareMatrix
    Inherits Matrix

    Public Sub New(order As Integer)
        MyBase.New(order, order)
    End Sub

    Public Sub New(elements(,) As Double)
        MyBase.New(elements)
        DemandSquareMatrix()
    End Sub

    Public Sub New(matrix As Matrix)
        MyBase.New(matrix._Elements)
    End Sub

    Private Sub DemandSquareMatrix()
        If Me.Width <> Me.Height Then
            Throw New ArgumentException("Array must have same length in both dimensions.")
        End If
    End Sub

    Public ReadOnly Property Order() As Integer
        Get
            Return Me.Width
        End Get
    End Property


    Public Shared ReadOnly Property Identity(order As Integer) As SquareMatrix
        Get
            Dim matrix = New SquareMatrix(order)
            For i = 0 To order - 1
                matrix._Elements(i, i) = 1
            Next
            Return matrix
        End Get
    End Property

    Public ReadOnly Property Determinant() As Double
        Get

            Select Case Me.Order
                Case 1
                    Return Me(0, 0)

                Case 2
                    Return Me(0, 0) * Me(1, 1) _
                         - Me(1, 0) * Me(0, 1)

                Case 3
                    Return Me(0, 0) * Me(1, 1) * Me(2, 2) _
                         + Me(1, 0) * Me(2, 1) * Me(0, 2) _
                         + Me(2, 0) * Me(0, 1) * Me(1, 2) _
                         - Me(0, 0) * Me(2, 1) * Me(1, 2) _
                         - Me(1, 0) * Me(0, 1) * Me(2, 2) _
                         - Me(2, 0) * Me(1, 1) * Me(0, 2)

                Case Else
                    Throw New NotImplementedException

            End Select

        End Get
    End Property

    Public ReadOnly Property Inverse() As SquareMatrix
        Get
            Select Case Me.Width

                Case 1
                    Return New SquareMatrix(New Double(,) {{1 / Me(0, 0)}})

                Case 2
                    Dim inverseMatrix As New SquareMatrix(2)
                    Dim detToTheMinus1 As Double = 1 / Me.Determinant()
                    inverseMatrix(0, 0) = detToTheMinus1 * Me(1, 1)
                    inverseMatrix(1, 0) = -detToTheMinus1 * Me(1, 0)
                    inverseMatrix(0, 1) = -detToTheMinus1 * Me(0, 1)
                    inverseMatrix(1, 1) = detToTheMinus1 * Me(0, 0)
                    Return inverseMatrix

                Case Else
                    Throw New NotImplementedException()

            End Select

        End Get
    End Property

    Public Overloads Shared Operator *(m1 As SquareMatrix, m2 As SquareMatrix) As SquareMatrix
        Dim m1AsMatrix = DirectCast(m1, Matrix)
        Return New SquareMatrix(m1AsMatrix * m2)
    End Operator
End Class