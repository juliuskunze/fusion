Public Class Term

    Private Shared ReadOnly _DisplayableFunctions As DisplayableFunction() = {New DisplayableFunction("exp", AddressOf System.Math.Exp),
                                                                   New DisplayableFunction("sin", AddressOf System.Math.Sin),
                                                                   New DisplayableFunction("cos", AddressOf System.Math.Cos),
                                                                   New DisplayableFunction("tan", AddressOf System.Math.Tan),
                                                                   New DisplayableFunction("asin", AddressOf System.Math.Asin),
                                                                   New DisplayableFunction("acos", AddressOf System.Math.Acos)}
    Private Shared ReadOnly _InvalidTermException As New ArgumentException("The term is invalid.")

    Private ReadOnly _Term As String
    Private _CharIsInBrackets As Boolean()

    Public Sub New(ByVal termWithoutBlanks As String)
        _Term = termWithoutBlanks
    End Sub

    Public Function TryGetValue(ByVal out_result As Double) As Boolean
        Try
            out_result = Me.GetValue
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function GetValue() As Double
        If _Term = "" Then Throw _InvalidTermException

        If String.Equals(_Term, "pi", StringComparison.OrdinalIgnoreCase) Then Return System.Math.PI
        If String.Equals(_Term, "e", StringComparison.OrdinalIgnoreCase) Then Return System.Math.E

        Dim parsedDouble As Double
        If Double.TryParse(_Term, result:=parsedDouble) AndAlso Not _Term.Contains("("c) Then Return parsedDouble

        _CharIsInBrackets = Me.GetCharIsInBracketsArray
        If TermIsInBrackets(startIndex:=0, endIndex:=_Term.Length - 1) Then Return New Term(_Term.Substring(startIndex:=1, length:=_Term.Length - 2)).GetValue

        Dim startingFunction = Me.GetStartingFunction
        If startingFunction IsNot Nothing AndAlso TermIsInBrackets(startIndex:=startingFunction.Name.Length, endIndex:=_Term.Length - 1) Then
            Return startingFunction.F(ValueAfterIndex(startingFunction.Name.Length - 1))
        End If

        Select Case _Term.Chars(0)
            Case "+"c, "-"c
                Dim minusCountAtStart = 0
                Dim notSignIndex As Integer
                For i = 0 To _Term.Length - 1
                    Select Case _Term(i)
                        Case "+"c
                        Case "-"c
                            minusCountAtStart -= 1
                        Case Else
                            notSignIndex = i
                            Exit For
                    End Select
                Next

                Dim termIsPositve = (minusCountAtStart Mod 2 = 0)

                If termIsPositve Then
                    Return ValueOfSubstring(startIndex:=notSignIndex, length:=_Term.Length - notSignIndex)
                Else
                    For i = notSignIndex To _Term.Length - 1
                        If Not _CharIsInBrackets(i) AndAlso _Term.Chars(i) = "+"c Then Return -ValueOfSubstring(startIndex:=notSignIndex, length:=i - notSignIndex) + ValueAfterIndex(i)
                    Next

                    For i = notSignIndex To _Term.Length - 1
                        If Not _CharIsInBrackets(i) AndAlso _Term.Chars(i) = "-"c Then Return -ValueOfSubstring(startIndex:=notSignIndex, length:=i - notSignIndex) - ValueAfterIndex(i)
                    Next

                    Return -ValueOfSubstring(startIndex:=notSignIndex, length:=_Term.Length - notSignIndex)
                End If
        End Select

        For i = 0 To _Term.Length - 1
            If Not _CharIsInBrackets(i) AndAlso _Term.Chars(i) = "+"c Then Return ValueBeforeIndex(i) + ValueAfterIndex(i)
        Next

        For i = 0 To _Term.Length - 1
            If Not _CharIsInBrackets(i) AndAlso _Term.Chars(i) = "-"c Then Return ValueBeforeIndex(i) - ValueAfterIndex(i)
        Next

        For i = 0 To _Term.Length - 1
            If Not _CharIsInBrackets(i) AndAlso _Term.Chars(i) = "*"c Then Return ValueBeforeIndex(i) * ValueAfterIndex(i)
        Next

        For i = 0 To _Term.Length - 1
            If Not _CharIsInBrackets(i) AndAlso _Term.Chars(i) = "/"c Then Return ValueBeforeIndex(i) / ValueAfterIndex(i)
        Next

        For i = 0 To _Term.Length - 1
            If Not _CharIsInBrackets(i) AndAlso _Term.Chars(i) = "^"c Then Return ValueBeforeIndex(i) ^ ValueAfterIndex(i)
        Next

        Throw _InvalidTermException
    End Function

    Private Function GetStartingFunction() As DisplayableFunction
        Dim startingFunctionNames = From displayableFunction In _DisplayableFunctions Where _Term.StartsWith(displayableFunction.Name, StringComparison.OrdinalIgnoreCase)

        If Not startingFunctionNames.Any Then Return Nothing

        Return startingFunctionNames.Single
    End Function

    Private Function GetCharIsInBracketsArray() As Boolean()
        Dim out_charIsInBrackets(_Term.Length - 1) As Boolean
        Dim bracketDepth As Integer = 0

        For i = 0 To _Term.Length - 1
            If _Term.Chars(i) = "("c Then
                bracketDepth += 1
            End If

            If bracketDepth < 0 Then
                Throw _InvalidTermException
            End If
            If bracketDepth > 0 Then
                out_charIsInBrackets(i) = True
            End If

            If _Term.Chars(i) = ")"c Then
                bracketDepth -= 1
            End If
        Next

        If bracketDepth <> 0 Then Throw _InvalidTermException


        Return out_charIsInBrackets
    End Function

    Private ReadOnly Property ValueBeforeIndex(ByVal index As Integer) As Double
        Get
            Return ValueOfSubstring(0, index)
        End Get
    End Property

    Private ReadOnly Property ValueAfterIndex(ByVal index As Integer) As Double
        Get
            Return ValueOfSubstring(index + 1, _Term.Length - 1 - index)
        End Get
    End Property

    Private ReadOnly Property ValueOfSubstring(ByVal startIndex As Integer, ByVal length As Integer) As Double
        Get
            Return New Term(_Term.Substring(startIndex, length)).GetValue
        End Get
    End Property

    Private ReadOnly Property TermIsInBrackets(ByVal startIndex As Integer, ByVal endIndex As Integer) As Boolean
        Get
            For i = startIndex To endIndex
                If Not _CharIsInBrackets(i) Then Return False
            Next

            Return True
        End Get
    End Property

    Private Class DisplayableFunction

        Private ReadOnly _Name As String
        Public ReadOnly Property Name As String
            Get
                Return _Name
            End Get
        End Property

        Private ReadOnly _F As Func(Of Double, Double)
        Public ReadOnly Property F As Func(Of Double, Double)
            Get
                Return _F
            End Get
        End Property

        Public Sub New(ByVal name As String, ByVal f As Func(Of Double, Double))
            _Name = name
            _F = f
        End Sub

    End Class

End Class
