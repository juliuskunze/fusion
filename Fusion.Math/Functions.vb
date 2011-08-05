Public Module Functions

    Public Function Gcd(ByVal a As Long, ByVal b As Long) As Long
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

    Public Function Lcm(ByVal a As Long, ByVal b As Long) As Long
        If a = 0 AndAlso b = 0 Then Return 0

        Return Abs(a * b) \ Gcd(a, b)
    End Function

    Public Function Factors(ByVal a As Long) As List(Of Long)
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

    Public Function Primes(ByVal upperBound As Integer, Optional ByVal lowerBound As Integer = 2) As List(Of Integer)
        If lowerBound < 0 Then lowerBound = 0
        If upperBound < 0 Then upperBound = 0

        Primes = New List(Of Integer)

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

        For i = lowerBound To upperBound
            If isPrime(i) Then Primes.Add(i)
        Next

        Return Primes
    End Function

    Public Function Factorial(ByVal n As Long) As Long
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

    Public Function Choose(ByVal total As Long, ByVal chosen As Long) As Long
        If chosen > total Then Return 0

        If chosen > total - chosen Then
            Return FactorialPiece(total, chosen) \ Factorial(total - chosen)
        Else
            Return FactorialPiece(total, lowerBoundMinusOne:=total - chosen) \ Factorial(chosen)
        End If
    End Function

    Public Function FactorialPiece(ByVal upperBound As Long, ByVal lowerBoundMinusOne As Long) As Long
        If upperBound <= 1 Or upperBound < lowerBoundMinusOne Then Return 1

        FactorialPiece = 1
        For i = lowerBoundMinusOne + 1 To upperBound
            FactorialPiece *= i
        Next
        Return FactorialPiece
    End Function

    Public Function ChoosingPossibilityCount(ByVal total As Long,
                                             ByVal chosen As Long,
                                             ByVal respectOrder As Boolean,
                                             ByVal respectDuplication As Boolean) As Long

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

End Module
