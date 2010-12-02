<Serializable()>
Public Structure Vector2D

    Private _x, _y As Double

    Public Sub New(ByVal x As Double, ByVal y As Double)
        SetFromXY(x, y)
    End Sub

    Public Sub New(ByVal m As Matrix)
        If m.Width = 1 AndAlso m.Height = 2 Then
            Me.SetFromXY(m(0, 0), m(1, 0))
        ElseIf m.Width = 2 AndAlso m.Height = 1 Then
            Me.SetFromXY(m(0, 0), m(0, 1))
        Else
            Throw New ArgumentException("Matrix has to be a 2D-Vector.")
        End If
    End Sub

    Public Sub New(ByVal p As PointF)
        SetFromXY(p.X, p.Y)
    End Sub

    Public Sub New(ByVal s As SizeF)
        SetFromXY(s.Width, s.Height)
    End Sub

    Public Sub New(ByVal vectorString As String)
        If vectorString(0) = "("c AndAlso vectorString(vectorString.Length - 1) = ")" OrElse _
            vectorString(0) = "["c AndAlso vectorString(vectorString.Length - 1) = "]" OrElse _
            vectorString(0) = "{"c AndAlso vectorString(vectorString.Length - 1) = "}" Then
            vectorString = vectorString.Remove(vectorString.Length - 1, 1).Remove(0, 1)
        End If

        Dim splitStrings = vectorString.Split(New Char() {";"c, "|"c})

        If splitStrings.Length <> 2 Then
            Throw New ArgumentException("String can't be converted into a vector.")
        End If

        Try
            _x = CDbl(splitStrings(0))
            _y = CDbl(splitStrings(1))
        Catch ex As Exception
            Throw New ArgumentException("String can't be converted into a vector.")
        End Try

    End Sub

    Private Sub SetFromXY(ByVal x As Double, ByVal y As Double)
        _x = x
        _y = y
    End Sub

    Private Sub SetFromVector(ByVal v As Vector2D)
        SetFromXY(v._x, v._y)
    End Sub

    Public Shared Function FromLengthAndArgument(ByVal length As Double, ByVal argument As Double) As Vector2D
        Return New Vector2D(length * Cos(argument), length * Sin(argument))
    End Function


    Public Shared ReadOnly Property Zero() As Vector2D
        Get
            Return New Vector2D
        End Get
    End Property


    Public Property X() As Double
        Get
            Return _x
        End Get
        Set(ByVal value As Double)
            _x = value
        End Set
    End Property

    Public Property Y() As Double
        Get
            Return _y
        End Get
        Set(ByVal value As Double)
            _y = value
        End Set
    End Property

    Public Property Length() As Double
        Get
            Return Sqrt(Me.LengthSquared)
        End Get
        Set(ByVal value As Double)
            If Me.Length = 0 Then
                Me.X = value
            Else
                Dim factor = value / Me.Length
                SetFromVector(factor * Me)
            End If
        End Set
    End Property

    Public Property LengthSquared() As Double
        Get
            Return X * X + Y * Y
        End Get
        Set(ByVal value As Double)
            Me.Length = Sqrt(value)
        End Set
    End Property

    Public Property Argument() As Double
        Get
            Return Atan2(Me.Y, Me.X)
        End Get
        Set(ByVal value As Double)
            SetFromVector(FromLengthAndArgument(Me.Length, value))
        End Set
    End Property


    Public Shadows Function Equals(ByVal other As Vector2D) As Boolean
        Return Me.X = other.X AndAlso Me.Y = other.Y
    End Function

    Public Shared Operator =(ByVal v1 As Vector2D, ByVal v2 As Vector2D) As Boolean
        Return v1.Equals(v2)
    End Operator

    Public Shared Operator <>(ByVal v1 As Vector2D, ByVal v2 As Vector2D) As Boolean
        Return Not v1 = v2
    End Operator

    Public Shared Operator +(ByVal v1 As Vector2D, ByVal v2 As Vector2D) As Vector2D
        Return New Vector2D(v1._x + v2._x, v1._y + v2._y)
    End Operator

    Public Shared Operator -(ByVal v1 As Vector2D, ByVal v2 As Vector2D) As Vector2D
        Return New Vector2D(v1._x - v2._x, v1._y - v2._y)
    End Operator

    Public Shared Operator -(ByVal v As Vector2D) As Vector2D
        Return New Vector2D(-v._x, -v._y)
    End Operator

    Public Shared Operator *(ByVal a As Double, ByVal v As Vector2D) As Vector2D
        Return New Vector2D(a * v._x, a * v._y)
    End Operator

    Public Shared Operator *(ByVal v As Vector2D, ByVal a As Double) As Vector2D
        Return a * v
    End Operator

    Public Shared Operator /(ByVal v As Vector2D, ByVal a As Double) As Vector2D
        Return New Vector2D(v._x / a, v._y / a)
    End Operator

    Public Shared Function DotProduct(ByVal v1 As Vector2D, ByVal v2 As Vector2D) As Double
        Return v1._x * v2._x + _
               v1._y * v2._y
    End Function

    Public Shared Function CrossProduct(ByVal v1 As Vector2D, ByVal v2 As Vector2D) As Double
        Return v1._x * v2._y - v1._y * v2._x
    End Function


    Public Shared Function Fit(ByVal v1 As Vector2D, ByVal v2 As Vector2D, Optional ByVal maxRelativeError As Double = 0.00000000000001) As Boolean
        If v1 = v2 Then
            Return True
        End If
        Return (v1 - v2).Length / (0.5 * (v1.Length + v2.Length)) < maxRelativeError
    End Function


    Public Function ToColumnMatrix() As Matrix
        Dim matrixArray As Double(,) = {{_x}, {_y}}
        Return New Matrix(matrixArray)
    End Function

    Public Function ToRowMatrix() As Matrix
        Dim matrixArray As Double(,) = {{_x, _y}}
        Return New Matrix(matrixArray)
    End Function

    Public Function ToPointF() As PointF
        Return New PointF(CSng(Me.X), CSng(Me.Y))
    End Function

    Public Function ToSizeF() As SizeF
        Return New SizeF(CSng(Me.X), CSng(Me.Y))
    End Function

    Public Overrides Function ToString() As String
        Return Me.ToString("")
    End Function

    Public Overloads Function ToString(ByVal numberFormat As String) As String
        Return "(" & _x.ToString(numberFormat) & "|" & _y.ToString(numberFormat) & ")"
    End Function

End Structure