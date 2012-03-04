Imports System.Math

Public Class Complex
    Private ReadOnly _Re As Double

    Public ReadOnly Property Re As Double
        Get
            Return _Re
        End Get
    End Property

    Public ReadOnly Property Im As Double
        Get
            Return _Im
        End Get
    End Property

    Private ReadOnly _Im As Double

    Public Sub New(vector As Vector2D)
        _Re = vector.X
        _Im = vector.Y
    End Sub

    Public Sub New(re As Double, im As Double)
        _Re = re
        _Im = im
    End Sub

    Public Shared Function FromLengthAndArgument(length As Double, argument As Double) As Complex
        Return New Complex(length * Cos(argument), length * Sin(argument))
    End Function

    Public ReadOnly Property Argument As Double
        Get
            Return Atan2(_Im, _Re)
        End Get
    End Property

    Public ReadOnly Property Abs() As Double
        Get
            Return (Re ^ 2 + Im ^ 2) ^ 0.5
        End Get
    End Property

    Public ReadOnly Property Length() As Double
        Get
            Return Abs
        End Get
    End Property

    Public ReadOnly Property AbsSquared As Double
        Get
            Return Re ^ 2 + Im ^ 2
        End Get
    End Property

    Public ReadOnly Property Conjugate As Complex
        Get
            Return New Complex(Re, -Im)
        End Get
    End Property

    Public Shared Function Exp(c As Complex) As Complex
        Return FromLengthAndArgument(System.Math.Exp(c.Re), c.Im)
    End Function


    Public ReadOnly Property Vector As Vector2D
        Get
            Return New Vector2D(Re, Im)
        End Get
    End Property

    Public Shared Operator +(c1 As Complex, c2 As Complex) As Complex
        Return New Complex(c1.Re + c2.Re, c1.Im + c2.Im)
    End Operator

    Public Shared Operator -(c1 As Complex, c2 As Complex) As Complex
        Return New Complex(c1.Re - c2.Re, c1.Im - c2.Im)
    End Operator

    Public Shared Operator -(c As Complex) As Complex
        Return New Complex(-c.Re, c.Im)
    End Operator

    Public Shared Operator *(c1 As Complex, c2 As Complex) As Complex
        Return New Complex(c1.Re * c2.Re - c1.Im * c2.Im, c1.Re * c2.Im + c1.Im * c2.Re)
    End Operator

    Public Shared Operator /(c1 As Complex, c2 As Complex) As Complex
        Return New Complex((c1.Re * c2.Re + c1.Im * c2.Im) / (c2.Re ^ 2 + c2.Im ^ 2), _
                                (c1.Im * c2.Re - c1.Re * c2.Im) / (c2.Re ^ 2 + c2.Im ^ 2))
    End Operator

    Public Shared Operator ^(c As Complex, exponent As Integer) As Complex
        Dim result = New Complex(1, 0)
        For n = 1 To exponent
            result *= c
        Next
        Return result
    End Operator

    Public Overrides Function Equals(obj As Object) As Boolean
        Dim c = TryCast(obj, Complex)
        If c Is Nothing Then Return False
        Return Re = c.Re AndAlso Im = c.Im
    End Function

    Public Overrides Function ToString() As String
        Return "Re: " & Re.ToString & "; Im: " & Im.ToString
    End Function

    Public Overloads Function ToString(format As String) As String
        Return "Re: " & Re.ToString(format) & "; Im: " & Im.ToString(format)
    End Function

    Public Function ToVector() As Vector2D
        Return New Vector2D(Re, Im)
    End Function

    Public Function Inverse() As Complex
        Return New Complex(1, 0) / Me
    End Function
End Class
