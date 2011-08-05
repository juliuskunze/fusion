Imports System.Linq.Expressions

Public Class Term(Of TDelegate)

    Private Shared ReadOnly _DisplayableFunctions As DoubleSingleParameterFunction() = {New DoubleSingleParameterFunction("Exp", AddressOf System.Math.Exp),
                                                                   New DoubleSingleParameterFunction("Sin", AddressOf System.Math.Sin),
                                                                   New DoubleSingleParameterFunction("Cos", AddressOf System.Math.Cos),
                                                                   New DoubleSingleParameterFunction("Tan", AddressOf System.Math.Tan),
                                                                   New DoubleSingleParameterFunction("Asin", AddressOf System.Math.Asin),
                                                                   New DoubleSingleParameterFunction("Acos", AddressOf System.Math.Acos)}

    Private ReadOnly _Term As String
    Private _CharIsInBrackets As Boolean()
    Private ReadOnly _DoubleParameterNames As IEnumerable(Of String)

    Public Sub New(ByVal termWithoutBlanks As String, ByVal doubleParameterNames As IEnumerable(Of String))
        _Term = termWithoutBlanks
        _DoubleParameterNames = doubleParameterNames
    End Sub

    Public Function TryGetDelegate() As TDelegate
        Try
            Return Me.GetDelegate
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function GetDelegate() As TDelegate
        Dim parameters = From parameterName In _DoubleParameterNames Select Expression.Parameter(GetType(Double), parameterName)

        Return Expression.Lambda(Of TDelegate)(body:=Me.GetExpression,
                                               parameters:=parameters).Compile
    End Function

    Public Function GetExpression() As Expression
        If _Term = "" Then Throw New InvalidTermException(_Term)

        If String.Equals(_Term, "pi", StringComparison.OrdinalIgnoreCase) Then Return Expression.Constant(System.Math.PI)
        If String.Equals(_Term, "e", StringComparison.OrdinalIgnoreCase) Then Return Expression.Constant(System.Math.E)

        Dim parsedDouble As Double
        If Double.TryParse(_Term, result:=parsedDouble) AndAlso Not _Term.Contains("("c) Then Return Expression.Constant(parsedDouble)

        Me.InitializeCharIsInBracketsArray()
        If TermIsInBrackets(startIndex:=0, endIndex:=_Term.Length - 1) Then Return SubstringExpression(startIndex:=1, length:=_Term.Length - 2)

        Dim startingFunction = Me.GetStartingFunction
        If startingFunction IsNot Nothing AndAlso TermIsInBrackets(startIndex:=startingFunction.Name.Length, endIndex:=_Term.Length - 1) Then

            Return startingFunction.Expression(AfterIndexExpression(startingFunction.Name.Length - 1))
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
                    Return SubstringExpression(startIndex:=notSignIndex, length:=_Term.Length - notSignIndex)
                Else
                    For i = notSignIndex To _Term.Length - 1
                        If Not _CharIsInBrackets(i) AndAlso _Term.Chars(i) = "+"c Then Return Expression.AddChecked(Expression.NegateChecked(SubstringExpression(startIndex:=notSignIndex, length:=i - notSignIndex)), AfterIndexExpression(i))
                    Next

                    For i = notSignIndex To _Term.Length - 1
                        If Not _CharIsInBrackets(i) AndAlso _Term.Chars(i) = "-"c Then Return Expression.SubtractChecked(Expression.NegateChecked(SubstringExpression(startIndex:=notSignIndex, length:=i - notSignIndex)), AfterIndexExpression(i))
                    Next

                    Return Expression.NegateChecked(SubstringExpression(startIndex:=notSignIndex, length:=_Term.Length - notSignIndex))
                End If
        End Select

        For i = 0 To _Term.Length - 1
            If Not _CharIsInBrackets(i) AndAlso _Term.Chars(i) = "+"c Then Return Expression.AddChecked(BeforeIndexExpression(i), AfterIndexExpression(i))
        Next

        For i = 0 To _Term.Length - 1
            If Not _CharIsInBrackets(i) AndAlso _Term.Chars(i) = "-"c Then Return Expression.SubtractChecked(BeforeIndexExpression(i), AfterIndexExpression(i))
        Next

        For i = 0 To _Term.Length - 1
            If Not _CharIsInBrackets(i) AndAlso _Term.Chars(i) = "*"c Then Return Expression.MultiplyChecked(BeforeIndexExpression(i), AfterIndexExpression(i))
        Next

        For i = 0 To _Term.Length - 1
            If Not _CharIsInBrackets(i) AndAlso _Term.Chars(i) = "/"c Then Return Expression.Divide(BeforeIndexExpression(i), AfterIndexExpression(i))
        Next

        For i = 0 To _Term.Length - 1
            If Not _CharIsInBrackets(i) AndAlso _Term.Chars(i) = "^"c Then Return Expression.Power(BeforeIndexExpression(i), AfterIndexExpression(i))
        Next

        Throw New InvalidTermException(_Term)
    End Function

    Private Function GetStartingFunction() As DoubleSingleParameterFunction
        Dim startingFunctionNames = From displayableFunction In _DisplayableFunctions Where _Term.StartsWith(displayableFunction.Name, StringComparison.OrdinalIgnoreCase)

        If Not startingFunctionNames.Any Then Return Nothing

        Return startingFunctionNames.Single
    End Function

    Private Sub InitializeCharIsInBracketsArray()
        ReDim _CharIsInBrackets(_Term.Length - 1)
        Dim bracketDepth As Integer = 0

        For i = 0 To _Term.Length - 1
            If _Term.Chars(i) = "("c Then
                bracketDepth += 1
            End If

            If bracketDepth < 0 Then
                Throw New InvalidTermException(_Term)
            End If
            If bracketDepth > 0 Then
                _CharIsInBrackets(i) = True
            End If

            If _Term.Chars(i) = ")"c Then
                bracketDepth -= 1
            End If
        Next

        If bracketDepth <> 0 Then Throw New InvalidTermException(_Term)
    End Sub

    Private ReadOnly Property BeforeIndexExpression(ByVal index As Integer) As Expression
        Get
            Return SubstringExpression(0, index)
        End Get
    End Property

    Private ReadOnly Property AfterIndexExpression(ByVal index As Integer) As Expression
        Get
            Return SubstringExpression(index + 1, _Term.Length - 1 - index)
        End Get
    End Property

    Private ReadOnly Property SubstringExpression(ByVal startIndex As Integer, ByVal length As Integer) As Expression
        Get
            Return New Term(Of TDelegate)(_Term.Substring(startIndex, length), doubleParameterNames:=_DoubleParameterNames).GetExpression
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

    Private Class DoubleSingleParameterFunction

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

        Public ReadOnly Property Expression(ByVal argument As Expression) As Expression
            Get
                Return Expressions.Expression.Call(method:=GetType(System.Math).GetMethod(name:=Me.Name), arguments:={argument})
            End Get
        End Property

        Public Sub New(ByVal name As String, ByVal f As Func(Of Double, Double))
            _Name = name
            _F = f
        End Sub

    End Class

End Class

Public Class InvalidTermException
    Inherits ArgumentException

    Public Sub New(ByVal term As String)
        MyBase.New(Message:="The term is invalid: " & term)
    End Sub

End Class