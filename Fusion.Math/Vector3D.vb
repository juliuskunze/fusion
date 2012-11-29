Public Structure Vector3D

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

    Private ReadOnly _Z As Double
    Public ReadOnly Property Z As Double
        Get
            Return _Z
        End Get
    End Property

    Public Sub New(x As Double, y As Double, z As Double)
        _X = x
        _Y = y
        _Z = z
    End Sub

    Public Sub New(v As Vector2D)
        Me.New(v.X, v.Y, 0)
    End Sub

    Public Sub New(m As Matrix)
        If m.Width = 1 AndAlso m.Height = 3 Then
            _X = m(0, 0)
            _Y = m(1, 0)
            _Z = m(2, 0)
        ElseIf m.Width = 3 AndAlso m.Height = 1 Then
            _X = m(0, 0)
            _Y = m(0, 1)
            _Z = m(0, 2)
        Else
            Throw New ArgumentException("Matrix has to be a 3D-Vector.")
        End If
    End Sub

    Public Sub New(vectorString As String)
        If vectorString.First = "("c AndAlso vectorString.Last = ")"c OrElse
           vectorString.First = "["c AndAlso vectorString.Last = "]"c OrElse
           vectorString.First = "{"c AndAlso vectorString.Last = "}"c OrElse
           vectorString.First = "<"c AndAlso vectorString.Last = ">"c Then
            vectorString = vectorString.Remove(vectorString.Length - 1, 1).Remove(0, 1)
        End If

        Dim components = vectorString.Split(";"c, "|"c, ","c)

        If components.Count <> 3 Then Throw New ArgumentException("The component count of a 3D-vector must be 3.")

        Try
            _X = CDbl(components(0))
            _Y = CDbl(components(1))
            _Z = CDbl(components(2))
        Catch ex As InvalidCastException
            Throw New ArgumentException("String can not be converted into a vector.")
        End Try
    End Sub

    Public Shared Function FromCylinderCoordinates(rho As Double, phi As Double, z As Double) As Vector3D
        Return New Vector3D(rho * Cos(phi), rho * Sin(phi), z)
    End Function

    Public Shared Function FromSphericalCoordinates(r As Double, theta As Double, phi As Double) As Vector3D
        Return New Vector3D(r * Sin(theta) * Cos(phi),
                            r * Sin(theta) * Sin(phi),
                            r * Cos(theta))
    End Function


    Public Shared ReadOnly Property Zero() As Vector3D
        Get
            Return New Vector3D
        End Get
    End Property

    Public ReadOnly Property Length As Double
        Get
            Return Sqrt(LengthSquared)
        End Get
    End Property

    Public Function ScaledToLength(newLength As Double) As Vector3D
        If Length = 0 Then
            Return New Vector3D(newLength, 0, 0)
        Else
            Dim factor = newLength / Length
            Return factor * Me
        End If
    End Function

    Public ReadOnly Property LengthSquared As Double
        Get
            Return _X * _X + _Y * _Y + _Z * _Z
        End Get
    End Property

    Public ReadOnly Property R As Double
        Get
            Return Length
        End Get
    End Property

    Public ReadOnly Property Rho As Double
        Get
            Return Sqrt(_X * _X + _Y * _Y)
        End Get
    End Property

    Public ReadOnly Property Phi As Double
        Get
            Return Atan2(_Y, _X)
        End Get
    End Property

    Public ReadOnly Property Theta() As Double
        Get
            Return Acos(_Z / R)
        End Get
    End Property

    Public Shared Operator +(v1 As Vector3D, v2 As Vector3D) As Vector3D
        Return New Vector3D(v1._X + v2._X,
                            v1._Y + v2._Y,
                            v1._Z + v2._Z)
    End Operator

    Public Shared Operator -(v1 As Vector3D, v2 As Vector3D) As Vector3D
        Return New Vector3D(v1._X - v2._X,
                            v1._Y - v2._Y,
                            v1._Z - v2._Z)
    End Operator

    Public Shared Operator -(v As Vector3D) As Vector3D
        Return New Vector3D(-v._X, -v._Y, -v._Z)
    End Operator

    Public Shared Operator *(a As Double, v As Vector3D) As Vector3D
        Return New Vector3D(a * v._X,
                            a * v._Y,
                            a * v._Z)
    End Operator

    Public Shared Operator *(v As Vector3D, a As Double) As Vector3D
        Return a * v
    End Operator

    Public Shared Operator /(v As Vector3D, a As Double) As Vector3D
        Return New Vector3D(v._X / a,
                            v._Y / a,
                            v._Z / a)
    End Operator

    Public Shared Operator =(v1 As Vector3D, v2 As Vector3D) As Boolean
        Return v1.X = v2.X AndAlso v1.Y = v2.Y AndAlso v1.Z = v2.Z
    End Operator

    Public Shared Operator <>(v1 As Vector3D, v2 As Vector3D) As Boolean
        Return Not v1 = v2
    End Operator

    Public Function DotProduct(v As Vector3D) As Double
        Return X * v.X +
               Y * v.Y +
               Z * v.Z
    End Function

    Public Shared Operator *(v1 As Vector3D, v2 As Vector3D) As Double
        Return v1.DotProduct(v2)
    End Operator

    Public Function CrossProduct(v As Vector3D) As Vector3D
        Return New Vector3D(_Y * v._Z - _Z * v._Y,
                            _Z * v._X - _X * v._Z,
                            _X * v._Y - _Y * v._X)
    End Function

    Public Shared Function TripleProduct(v1 As Vector3D, v2 As Vector3D, v3 As Vector3D) As Double
        Return v1.CrossProduct(v2).DotProduct(v3)
    End Function

    Public Function ToColumnMatrix() As Matrix
        Dim matrixArray As Double(,) = {{_X}, {_Y}, {_Z}}
        Return New Matrix(matrixArray)
    End Function

    Public Function ToRowMatrix() As Matrix
        Dim matrixArray As Double(,) = {{_X, Y, Z}}
        Return New Matrix(matrixArray)
    End Function

    Public Function Normalized() As Vector3D
        Return Me / Length
    End Function

    Public Function OrthogonalProjectionOn(v As Vector3D) As Vector3D
        Return DotProduct(v) / v.LengthSquared * v
    End Function

    Public Overrides Function ToString() As String
        Return ToString("0.00")
    End Function

    Public Overloads Function ToString(numberFormat As String) As String
        Return String.Format("({0}|{1}|{2})", _X.ToString(numberFormat), _Y.ToString(numberFormat), _Z.ToString(numberFormat))
    End Function

    Public Shared Function Fit(v1 As Vector3D, v2 As Vector3D, Optional maxRelativeError As Double = 0.00000000000001) As Boolean
        If v1 = v2 Then Return True

        Return (v1 - v2).Length / (0.5 * (v1.Length + v2.Length)) < maxRelativeError
    End Function

End Structure