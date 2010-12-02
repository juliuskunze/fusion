Public Structure Vector3D

    Public Property X As Double
    Public Property Y As Double
    Public Property Z As Double

    Public Sub New(ByVal x As Double, ByVal y As Double, ByVal z As Double)
        SetFromXYZ(x, y, z)
    End Sub

    Public Sub New(ByVal v As Vector2D)
        SetFromXYZ(v.X, v.Y, 0)
    End Sub

    Public Sub New(ByVal m As Matrix)
        If m.Width = 1 AndAlso m.Height = 3 Then
            SetFromXYZ(m(0, 0), m(1, 0), m(2, 0))
        ElseIf m.Width = 3 AndAlso m.Height = 1 Then
            SetFromXYZ(m(0, 0), m(0, 1), m(0, 2))
        Else
            Throw New ArgumentException("Matrix has to be a 3D-Vector.")
        End If
    End Sub

    Private Sub SetFromXYZ(ByVal x As Double, ByVal y As Double, ByVal z As Double)
        _x = x
        _y = y
        _z = z
    End Sub

    Private Sub SetFromVector(ByVal v As Vector3D)
        SetFromXYZ(v._x, v._y, v._z)
    End Sub

    Public Shared Function FromCylinderCoordinates(ByVal rho As Double, ByVal phi As Double, ByVal z As Double) As Vector3D
        Return New Vector3D(rho * Cos(phi), rho * Sin(phi), z)
    End Function

    Public Shared Function FromSphericalCoordinates(ByVal r As Double, ByVal theta As Double, ByVal phi As Double) As Vector3D
        Return New Vector3D(r * Sin(theta) * Cos(phi),
                            r * Sin(theta) * Sin(phi),
                            r * Cos(theta))
    End Function


    Public Shared ReadOnly Property Zero() As Vector3D
        Get
            Return New Vector3D
        End Get
    End Property

    Public Property Length As Double
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

    Public ReadOnly Property LengthSquared As Double
        Get
            Return Me.X * Me.X + Me.Y * Me.Y + Me.Z * Me.Z
        End Get
    End Property

    Public Property R As Double
        Get
            Return Length
        End Get
        Set(ByVal value As Double)
            Me.Length = value
        End Set
    End Property

    Public ReadOnly Property Rho() As Double
        Get
            Return Sqrt(_x * _x + y * _y)
        End Get
    End Property

    Public ReadOnly Property Phi() As Double
        Get
            Return Atan2(_y, x)
        End Get
    End Property

    Public ReadOnly Property Theta() As Double
        Get
            Return Acos(_z / R)
        End Get
    End Property

    Public Shared Operator +(ByVal v1 As Vector3D, ByVal v2 As Vector3D) As Vector3D
        Return New Vector3D(v1._x + v2._x,
                            v1._y + v2._y,
                            v1._z + v2._z)
    End Operator

    Public Shared Operator -(ByVal v1 As Vector3D, ByVal v2 As Vector3D) As Vector3D
        Return New Vector3D(v1._X - v2._X,
                            v1._Y - v2._Y,
                            v1._Z - v2._Z)
    End Operator

    Public Shared Operator -(ByVal v As Vector3D) As Vector3D
        Return New Vector3D(-v._x, -v._y, -v._z)
    End Operator

    Public Shared Operator *(ByVal a As Double, ByVal v As Vector3D) As Vector3D
        Return New Vector3D(a * v._X,
                            a * v._Y,
                            a * v._Z)
    End Operator

    Public Shared Operator *(ByVal v As Vector3D, ByVal a As Double) As Vector3D
        Return a * v
    End Operator

    Public Shared Operator /(ByVal v As Vector3D, ByVal a As Double) As Vector3D
        Return New Vector3D(v._x / a,
                            v._y / a,
                            v._z / a)
    End Operator

    Public Shared Operator =(ByVal v1 As Vector3D, ByVal v2 As Vector3D) As Boolean
        Return v1.X = v2.X AndAlso v1.Y = v2.Y AndAlso v1.Z = v2.Z
    End Operator

    Public Shared Operator <>(ByVal v1 As Vector3D, ByVal v2 As Vector3D) As Boolean
        Return Not v1 = v2
    End Operator

    Public Function DotProduct(ByVal v As Vector3D) As Double
        Return X * v.X +
               Y * v.Y +
               Z * v.Z
    End Function

    Public Shared Operator *(ByVal v1 As Vector3D, ByVal v2 As Vector3D) As Double
        Return v1.DotProduct(v2)
    End Operator

    Public Function CrossProduct(ByVal v As Vector3D) As Vector3D
        Return New Vector3D(_y * v._z - _z * v._y,
                            _z * v._x - _x * v._z,
                            _x * v._y - _y * v._x)
    End Function

    Public Shared Function TripleProduct(ByVal v1 As Vector3D, ByVal v2 As Vector3D, ByVal v3 As Vector3D) As Double
        Return v1.CrossProduct(v2).DotProduct(v3)
    End Function


    Public Function ToColumnMatrix() As Matrix
        Dim matrixArray As Double(,) = {{_x}, {_y}, {_z}}
        Return New Matrix(matrixArray)
    End Function

    Public Function ToRowMatrix() As Matrix
        Dim matrixArray As Double(,) = {{_x, y, z}}
        Return New Matrix(matrixArray)
    End Function

    Public Function Normalized() As Vector3D
        Return Me / Me.Length
    End Function

    Public Function OrthogonalProjectionOn(ByVal v As Vector3D) As Vector3D
        Return Me.DotProduct(v) / v.LengthSquared * v
    End Function

    Public Overrides Function ToString() As String
        Return ToString("0.00")
    End Function

    Public Overloads Function ToString(ByVal numberFormat As String) As String
        Return "(" & _X.ToString(numberFormat) & "|" & _Y.ToString(numberFormat) & "|" & _Z.ToString(numberFormat) & ")"
    End Function

    Public Shared Function Fit(ByVal v1 As Vector3D, ByVal v2 As Vector3D, Optional ByVal maxRelativeError As Double = 0.00000000000001) As Boolean
        If v1 = v2 Then Return True

        Return (v1 - v2).Length / (0.5 * (v1.Length + v2.Length)) < maxRelativeError
    End Function

End Structure