<Serializable()>
Public Structure Vector2D

    Private ReadOnly _X As Double
    Public ReadOnly Property X As Double
        Get
            Return _X
        End Get
    End Property

    Private ReadOnly _Y As Double
    Public ReadOnly Property Y As Double
        Get
            Return _Y
        End Get
    End Property

    Public Sub New(x As Double, y As Double)
        _X = x
        _Y = y
    End Sub

    Public Sub New(m As Matrix)
        If m.Width = 1 AndAlso m.Height = 2 Then
            _X = m(0, 0)
            _Y = m(1, 0)
        ElseIf m.Width = 2 AndAlso m.Height = 1 Then
            _X = m(0, 0)
            _Y = m(0, 1)
        Else
            Throw New ArgumentException("Matrix has to be a 2D-Vector.")
        End If
    End Sub

    Public Sub New(p As PointF)
        Me.New(p.X, p.Y)
    End Sub

    Public Sub New(s As SizeF)
        Me.New(s.Width, s.Height)
    End Sub

    Public Sub New(vectorString As String)
        If vectorString(0) = "("c AndAlso vectorString(vectorString.Length - 1) = ")" OrElse
           vectorString(0) = "["c AndAlso vectorString(vectorString.Length - 1) = "]" OrElse
           vectorString(0) = "{"c AndAlso vectorString(vectorString.Length - 1) = "}" OrElse
           vectorString(0) = "<"c AndAlso vectorString(vectorString.Length - 1) = ">" Then
            vectorString = vectorString.Remove(vectorString.Length - 1, 1).Remove(0, 1)
        End If

        Dim splitStrings = vectorString.Split(";"c, "|"c, ","c)

        If splitStrings.Count <> 2 Then
            Throw New ArgumentException("String can't be converted into a vector.")
        End If

        Try
            _X = CDbl(splitStrings(0))
            _Y = CDbl(splitStrings(1))
        Catch ex As Exception
            Throw New ArgumentException("String can't be converted into a vector.")
        End Try

    End Sub

    Public Shared Function FromLengthAndArgument(length As Double, argument As Double) As Vector2D
        Return New Vector2D(length * Cos(argument), length * Sin(argument))
    End Function

    Public Shared ReadOnly Property Zero() As Vector2D
        Get
            Return New Vector2D
        End Get
    End Property


    Public ReadOnly Property Length As Double
        Get
            Return Sqrt(Me.LengthSquared)
        End Get
    End Property

    Public ReadOnly Property LengthSquared As Double
        Get
            Return _X * _X + _Y * _Y
        End Get
    End Property

    Public Function ScaledToLength(newLength As Double) As Vector2D
        If Me.Length = 0 Then
            Return New Vector2D(newLength, 0)
        Else
            Dim factor = newLength / Me.Length
            Return factor * Me
        End If
    End Function

    Public ReadOnly Property Argument() As Double
        Get
            Return Atan2(_Y, _X)
        End Get
    End Property

    Public Function RotateToArgument(newArgument As Double) As Vector2D
        Return Vector2D.FromLengthAndArgument(Me.Length, newArgument)
    End Function

    Public Shadows Function Equals(other As Vector2D) As Boolean
        Return _X = other.X AndAlso _Y = other.Y
    End Function

    Public Shared Operator =(v1 As Vector2D, v2 As Vector2D) As Boolean
        Return v1.Equals(v2)
    End Operator

    Public Shared Operator <>(v1 As Vector2D, v2 As Vector2D) As Boolean
        Return Not v1 = v2
    End Operator

    Public Shared Operator +(v1 As Vector2D, v2 As Vector2D) As Vector2D
        Return New Vector2D(v1._X + v2._X, v1._Y + v2._Y)
    End Operator

    Public Shared Operator -(v1 As Vector2D, v2 As Vector2D) As Vector2D
        Return New Vector2D(v1._X - v2._X, v1._Y - v2._Y)
    End Operator

    Public Shared Operator -(v As Vector2D) As Vector2D
        Return New Vector2D(-v._X, -v._Y)
    End Operator

    Public Shared Operator *(a As Double, v As Vector2D) As Vector2D
        Return New Vector2D(a * v._X, a * v._Y)
    End Operator

    Public Shared Operator *(v As Vector2D, a As Double) As Vector2D
        Return a * v
    End Operator

    Public Shared Operator /(v As Vector2D, a As Double) As Vector2D
        Return New Vector2D(v._X / a, v._Y / a)
    End Operator

    Public Shared Function DotProduct(v1 As Vector2D, v2 As Vector2D) As Double
        Return v1._X * v2._X +
               v1._Y * v2._Y
    End Function

    Public Shared Function CrossProduct(v1 As Vector2D, v2 As Vector2D) As Double
        Return v1._X * v2._Y - v1._Y * v2._X
    End Function


    Public Shared Function Fit(v1 As Vector2D, v2 As Vector2D, Optional maxRelativeError As Double = 0.00000000000001) As Boolean
        If v1 = v2 Then Return True

        Return (v1 - v2).Length / (0.5 * (v1.Length + v2.Length)) < maxRelativeError
    End Function


    Public Function ToColumnMatrix() As Matrix
        Dim matrixArray As Double(,) = {{_X}, {_Y}}
        Return New Matrix(matrixArray)
    End Function

    Public Function ToRowMatrix() As Matrix
        Dim matrixArray As Double(,) = {{_X, _Y}}
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

    Public Overloads Function ToString(numberFormat As String) As String
        Return "(" & _X.ToString(numberFormat) & "|" & _Y.ToString(numberFormat) & ")"
    End Function

End Structure