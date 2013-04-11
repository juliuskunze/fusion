Public Module Functions
    Public Function Gcd(a As Long, b As Long) As Long
        If a = 0 AndAlso b = 0 Then
            Throw New ArgumentException("Gcd of 0 and 0 is not defined.")
        End If

        a = Abs(a)
        b = Abs(b)
        Dim k As Long = 1

        Do Until a = 0 OrElse b = 0
            Do While a Mod 2 = 0 AndAlso b Mod 2 = 0
                a \= 2
                b \= 2
                k *= 2
            Loop
            Do While a Mod 2 = 0
                a \= 2
            Loop
            Do While b Mod 2 = 0
                b \= 2
            Loop
            Do While a Mod 2 = 1 AndAlso b Mod 2 = 1
                If a > b Then
                    a = (a - b) \ 2
                Else
                    b = (b - a) \ 2
                End If
            Loop
        Loop
        If a = 0 Then
            Return CLng(b * k)
        Else 'b = 0
            Return CLng(a * k)
        End If
    End Function

    Public Function Lcm(a As Long, b As Long) As Long
        If a = 0 AndAlso b = 0 Then Return 0

        Return Abs(a * b) \ Gcd(a, b)
    End Function

    Public Function Factors(a As Long) As List(Of Long)
        Dim result = New List(Of Long)

        For t = 1 To CLng(Sqrt(a))
            If a Mod t = 0 Then
                result.Add(t)
                If Sqrt(a) <> a / t Then
                    result.Add(CLng(a / t))
                End If
            End If
        Next

        Return result
    End Function

    Public Function Primes(upperBound As Integer, Optional lowerBound As Integer = 2) As IEnumerable(Of Integer)
        If lowerBound < 0 Then lowerBound = 0
        If upperBound < 0 Then upperBound = 0

        Dim isPrime(upperBound + 1) As Boolean

        For i = 2 To upperBound
            isPrime(i) = True
        Next

        For i = 2 To upperBound
            If isPrime(i) Then
                For j = 2 * i To upperBound Step i
                    isPrime(j) = False
                Next
            End If
        Next

        Dim p = New List(Of Integer)
        
        For i = lowerBound To upperBound
            If isPrime(i) Then p.Add(i)
        Next

        Return p
    End Function

    Public Function Factorial(n As Double) As Double
        If n <= 1 Then
            Return 1
        Else
            Factorial = 1
            For i = 1 To n
                Factorial *= i
            Next
            Return Factorial
        End If
    End Function

    Public Function Choose(total As Long, chosen As Long) As Long
        If chosen > total Then Return 0

        If chosen > total - chosen Then
            Return FactorialPiece(total, chosen) \ CInt(Factorial(total - chosen))
        Else
            Return FactorialPiece(total, lowerBoundMinusOne:=total - chosen) \ CInt(Factorial(chosen))
        End If
    End Function

    Public Function FactorialPiece(upperBound As Long, lowerBoundMinusOne As Long) As Long
        If upperBound <= 1 Or upperBound < lowerBoundMinusOne Then Return 1

        FactorialPiece = 1
        For i = lowerBoundMinusOne + 1 To upperBound
            FactorialPiece *= i
        Next
        Return FactorialPiece
    End Function

    Public Function ChoosingPossibilityCount(total As Long,
                                              chosen As Long,
                                              respectOrder As Boolean,
                                              respectDuplication As Boolean) As Long

        If total < 0 Then Return 0
        If chosen < 0 Then Return 0

        If respectDuplication Then
            If respectOrder Then
                Return CLng(total ^ chosen)
            Else
                Return FactorialPiece(total, total - chosen)
            End If
        Else
            If respectOrder Then
                Return Choose(total - 1 + chosen, chosen)
            Else
                Return Choose(total, chosen)
            End If
        End If
    End Function

    Public Function Peak(position As Double, height As Double, width As Double) As Func(Of Double, Double)
        Return Function(x)
                   If x < position - width / 2 Then Return 0
                   If x > position + width / 2 Then Return 0
                   Return height
               End Function
    End Function

    ''' <summary>
    ''' The modulo result is greater than or equal to 0 and smaller than modulo.
    ''' </summary>
    Public Function NonnegativeMod(number As Double, modulo As Double) As Double
        Dim m = number Mod modulo
        Return If(m < 0, m + modulo, m)
    End Function

    Public Function NonnegativeNormalizedMod(number As Double, modulo As Double) As Double
        Return NonnegativeMod(number, modulo) / modulo
    End Function

End Module
