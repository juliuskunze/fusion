Public Class LinearMap2D
    Implements IMap2D

    Public Const Order As Integer = 2

    Public Sub New(mappingMatrix As SquareMatrix)
        If mappingMatrix.Order <> Order Then
            Throw New ArgumentException("Order of the matrix has to be " & Order.ToString & ".")
        End If

        _MappingMatrix = mappingMatrix
    End Sub

    Public Sub New(mappingMatrixArray As Double(,))
        Me.New(New SquareMatrix(mappingMatrixArray))
    End Sub

    Public Sub New()
        Me.New(SquareMatrix.Identity(Order))
    End Sub

    Public Shared Operator =(l1 As LinearMap2D, l2 As LinearMap2D) As Boolean
        Return l1.MappingMatrix = l2.MappingMatrix
    End Operator

    Public Shared Operator <>(l1 As LinearMap2D, l2 As LinearMap2D) As Boolean
        Return Not l1 = l2
    End Operator

    Private _MappingMatrix As SquareMatrix

    Public ReadOnly Property MappingMatrix() As SquareMatrix
        Get
            Return _MappingMatrix
        End Get
    End Property

    Public Shared Function Identity() As LinearMap2D
        Return New LinearMap2D()
    End Function

    Public Function Apply(v As Vector2D) As Vector2D Implements IMap2D.Apply
        Return New Vector2D(_MappingMatrix * v.ToColumnMatrix)
    End Function

    Public Function After(map As LinearMap2D) As LinearMap2D
        Return New LinearMap2D(_MappingMatrix * map._MappingMatrix)
    End Function

    Public Function Before(map As LinearMap2D) As LinearMap2D
        Return map.After(Me)
    End Function


    Public Shared Function Rotation(rotationAngle As Double) As LinearMap2D
        Return New LinearMap2D(New Double(,) {{Cos(rotationAngle), -Sin(rotationAngle)}, _
                                              {Sin(rotationAngle), Cos(rotationAngle)}})
    End Function

    Public Shared Function Reflection(axisAngle As Double) As LinearMap2D
        Return New LinearMap2D(New Double(,) {{Cos(2 * axisAngle), Sin(2 * axisAngle)}, _
                                              {Sin(2 * axisAngle), -Cos(2 * axisAngle)}})
    End Function

    Public Shared Function Scaling(scalingFactor As Double) As LinearMap2D
        Return New LinearMap2D(New Double(,) {{scalingFactor, 0}, _
                                              {0, scalingFactor}})
    End Function

    Public Shared Function Scaling(xFactor As Double, yFactor As Double) As LinearMap2D
        Return New LinearMap2D(New Double(,) {{xFactor, 0}, _
                                              {0, yFactor}})
    End Function

    Public Shared Function HorizontalShearing(shearingFactor As Double) As LinearMap2D
        Return New LinearMap2D(New Double(,) {{1, shearingFactor}, _
                                              {0, 1}})
    End Function

    Public Shared Function HorizontalScaling(scalingFactor As Double) As LinearMap2D
        Return New LinearMap2D(New Double(,) {{scalingFactor, 0}, _
                                              {0, 1}})
    End Function

    Public ReadOnly Property ZoomOut() As Double
        Get
            Return Sqrt(Abs(Me.MappingMatrix.Determinant))
        End Get
    End Property

    Public ReadOnly Property ZoomIn() As Double
        Get
            Return 1 / Me.ZoomOut
        End Get
    End Property

    Public Function Inverse() As LinearMap2D
        Return New LinearMap2D(Me.MappingMatrix.Inverse)
    End Function
End Class
