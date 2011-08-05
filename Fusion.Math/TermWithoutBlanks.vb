Imports System.Linq.Expressions

Public Class TermWithoutBlanks

    Private Shared ReadOnly _KnownFunctions As NamedMethodExpression() = {New NamedMethodExpression("Exp", NamedMethodExpression.GetSystemMathMethodCallExpression(name:="Exp")),
                                                                          New NamedMethodExpression("Sin", NamedMethodExpression.GetSystemMathMethodCallExpression(name:="Sin")),
                                                                          New NamedMethodExpression("Cos", NamedMethodExpression.GetSystemMathMethodCallExpression(name:="Cos")),
                                                                          New NamedMethodExpression("Tan", NamedMethodExpression.GetSystemMathMethodCallExpression(name:="Tan")),
                                                                          New NamedMethodExpression("Asin", NamedMethodExpression.GetSystemMathMethodCallExpression(name:="Asin")),
                                                                          New NamedMethodExpression("Acos", NamedMethodExpression.GetSystemMathMethodCallExpression(name:="Acos"))}

    Private Shared ReadOnly _KnownConstants As NamedConstantExpression() = {New NamedConstantExpression("Pi", System.Math.PI),
                                                                            New NamedConstantExpression("E", System.Math.E)}

    Private ReadOnly _Term As String
    Private _CharIsInBrackets As Boolean()
    Private ReadOnly _DoubleParameters As IEnumerable(Of ParameterExpression)

    Private Sub New(ByVal termWithoutBlanks As String, ByVal doubleParameters As IEnumerable(Of ParameterExpression))
        _Term = termWithoutBlanks
        _DoubleParameters = doubleParameters
    End Sub

    Public Sub New(ByVal termWithoutBlanks As String, ByVal doubleParameterNames As IEnumerable(Of String))
        _Term = termWithoutBlanks
        _DoubleParameters = doubleParameterNames.Select(Function(name) Expression.Parameter(type:=GetType(Double), name:=name)).ToArray
    End Sub

    Public Function TryGetDelegate() As System.Delegate
        Try
            Return Me.GetDelegate
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function GetDelegate() As System.Delegate
        Return Expression.Lambda(body:=Me.GetExpression, parameters:=_DoubleParameters).Compile
    End Function


    Public Function TryGetDelegate(Of TDelegate)() As TDelegate
        Try
            Return Me.GetDelegate(Of TDelegate)()
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function GetDelegate(Of TDelegate)() As TDelegate
        Dim lambda = Expression.Lambda(Of TDelegate)(body:=Me.GetExpression, parameters:=_DoubleParameters)

        Return lambda.Compile
    End Function

    Public Function GetExpression() As Expression
        If _Term = "" Then Throw New InvalidTermException(_Term)

        Dim parsedDouble As Double
        If Double.TryParse(_Term, result:=parsedDouble) AndAlso Not _Term.Contains("("c) Then Return Expression.Constant(parsedDouble)

        Dim equalKnownNamedConstants = From constant In _KnownConstants Where String.Equals(_Term, constant.Name, StringComparison.OrdinalIgnoreCase)
        If equalKnownNamedConstants.Any Then Return equalKnownNamedConstants.Single.ConstantExpression

        Dim equalParameters = From parameter In _DoubleParameters Where String.Equals(_Term, parameter.Name, StringComparison.OrdinalIgnoreCase)
        If equalParameters.Any Then Return equalParameters.Single

        Me.InitializeCharIsInBracketsArray()
        If TermIsInBrackets(startIndex:=0, endIndex:=_Term.Length - 1) Then Return SubExpression(_Term.Substring(startIndex:=1, length:=_Term.Length - 2))

        Dim startingFunction = Me.GetStartingFunction
        If startingFunction IsNot Nothing AndAlso TermIsInBrackets(startIndex:=startingFunction.Name.Length, endIndex:=_Term.Length - 1) Then
            Return startingFunction.Expression(From argument In _Term.Substring(startingFunction.Name.Length + 1, length:=_Term.Length - 2 - startingFunction.Name.Length).Split(","c) Select SubExpression(argument))
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

    Private Function GetStartingFunction() As NamedMethodExpression
        Dim startingMethodExpressions = From namedMethodExpression In _KnownFunctions Where _Term.StartsWith(namedMethodExpression.Name, StringComparison.OrdinalIgnoreCase)

        If Not startingMethodExpressions.Any Then Return Nothing

        Return startingMethodExpressions.Single
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
            Return New TermWithoutBlanks(_Term.Substring(startIndex, length), doubleParameters:=_DoubleParameters).GetExpression
        End Get
    End Property


    Private ReadOnly Property SubExpression(ByVal term As String) As Expression
        Get
            Return New TermWithoutBlanks(term, doubleParameters:=_DoubleParameters).GetExpression
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

    Private Class NamedMethodExpression

        Private ReadOnly _Name As String
        Public ReadOnly Property Name As String
            Get
                Return _Name
            End Get
        End Property

        Private ReadOnly _MethodCallExpressionBuilder As MethodCallExpressionBuilder
        Public ReadOnly Property Expression(ByVal arguments As IEnumerable(Of Expression)) As Expression
            Get
                Return _MethodCallExpressionBuilder(arguments)
            End Get
        End Property

        Public Sub New(ByVal name As String, ByVal methodCallExpressionBuilder As MethodCallExpressionBuilder)
            _Name = name
            _MethodCallExpressionBuilder = methodCallExpressionBuilder
        End Sub

        Public Shared Function GetSystemMathMethodCallExpression(ByVal name As String) As MethodCallExpressionBuilder
            Return Function(arguments) Expressions.Expression.Call(method:=GetType(System.Math).GetMethod(name:=name), arguments:=arguments)
        End Function

    End Class

    Private Class NamedConstantExpression

        Private ReadOnly _Name As String
        Public ReadOnly Property Name As String
            Get
                Return _Name
            End Get
        End Property

        Private ReadOnly _ConstantExpression As ConstantExpression
        Public ReadOnly Property ConstantExpression As ConstantExpression
            Get
                Return _ConstantExpression
            End Get
        End Property

        Public Sub New(ByVal name As String, ByVal value As Double)
            _Name = name
            _ConstantExpression = Expression.Constant(value)
        End Sub

    End Class

    Private Delegate Function MethodCallExpressionBuilder(ByVal arguments As IEnumerable(Of Expression)) As Expression

End Class

Public Class InvalidTermException
    Inherits ArgumentException

    Public Sub New(ByVal term As String)
        MyBase.New(Message:="The term is invalid: " & term)
    End Sub

End Class