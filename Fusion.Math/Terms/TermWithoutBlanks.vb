Public Class TermWithoutBlanks

    Private Shared ReadOnly _PredefinedConstants As NamedConstantExpression() = {New NamedConstantExpression("Pi", System.Math.PI),
                                                                             New NamedConstantExpression("E", System.Math.E)}

    Private ReadOnly _Parameters As IEnumerable(Of ParameterExpression)

    Private Shared ReadOnly _PredefinedFunctions As NamedExpression() = {New NamedExpression("Exp", NamedExpression.GetSystemMathMethodCallExpression(name:="Exp")),
                                                                     New NamedExpression("Sin", NamedExpression.GetSystemMathMethodCallExpression(name:="Sin")),
                                                                     New NamedExpression("Cos", NamedExpression.GetSystemMathMethodCallExpression(name:="Cos")),
                                                                     New NamedExpression("Tan", NamedExpression.GetSystemMathMethodCallExpression(name:="Tan")),
                                                                     New NamedExpression("Asin", NamedExpression.GetSystemMathMethodCallExpression(name:="Asin")),
                                                                     New NamedExpression("Acos", NamedExpression.GetSystemMathMethodCallExpression(name:="Acos"))}
    Private ReadOnly _UserFunctions As IEnumerable(Of NamedExpression)

    Private ReadOnly _Term As String
    Private _CharIsInBrackets As Boolean()

    Private Sub New(ByVal termWithoutBlanks As String, ByVal parameters As IEnumerable(Of ParameterExpression), ByVal userFunctions As IEnumerable(Of NamedExpression))
        _Term = termWithoutBlanks
        _Parameters = parameters
        _UserFunctions = userFunctions
    End Sub

    Public Sub New(ByVal termWithoutBlanks As String, ByVal parameterNames As IEnumerable(Of String), ByVal userFunctions As IEnumerable(Of NamedExpression))
        Me.New(termWithoutBlanks:=termWithoutBlanks,
               parameters:=parameterNames.Select(Function(name) Expression.Parameter(type:=GetType(Double), name:=name)).ToArray,
               userFunctions:=userFunctions)
    End Sub

    Public Function TryGetDelegate() As System.Delegate
        Try
            Return Me.GetDelegate
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function GetDelegate() As System.Delegate
        Return Expression.Lambda(body:=Me.GetExpression, parameters:=_Parameters).Compile
    End Function


    Public Function TryGetDelegate(Of TDelegate)() As TDelegate
        Try
            Return Me.GetDelegate(Of TDelegate)()
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function GetDelegate(Of TDelegate)() As TDelegate
        Dim lambda = Expression.Lambda(Of TDelegate)(body:=Me.GetExpression, parameters:=_Parameters)

        Return lambda.Compile
    End Function

    Public Function GetExpression() As Expression
        If _Term = "" Then Throw New InvalidTermException(_Term)

        Dim parsedDouble As Double
        If Double.TryParse(_Term, result:=parsedDouble) AndAlso Not _Term.Contains("("c) Then Return Expression.Constant(parsedDouble)

        Dim constants = From constant In _PredefinedConstants Where String.Equals(_Term, constant.Name, StringComparison.OrdinalIgnoreCase)
        If constants.Any Then Return constants.Single.ConstantExpression

        Dim parameters = From parameter In _Parameters Where String.Equals(_Term, parameter.Name, StringComparison.OrdinalIgnoreCase)
        If parameters.Any Then Return parameters.Single

        Me.InitializeCharIsInBracketsArray()
        If TermIsInBrackets(startIndex:=0, endIndex:=_Term.Length - 1) Then Return SubExpression(_Term.Substring(startIndex:=1, length:=_Term.Length - 2))

        Dim startingFunction = Me.GetStartingFunction
        If startingFunction IsNot Nothing AndAlso
           TermIsInBrackets(startIndex:=startingFunction.Name.Length, endIndex:=_Term.Length - 1) Then
            Dim arguments = From argument In _Term.Substring(startingFunction.Name.Length + 1, length:=_Term.Length - 2 - startingFunction.Name.Length).Split(","c) Select SubExpression(argument)
            Return Expression.Invoke(startingFunction.Expression(arguments:=arguments), arguments)
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

    Private Function GetStartingFunction() As NamedExpression
        Dim functions = From functionExpression In _PredefinedFunctions.Concat(_UserFunctions) Where _Term.StartsWith(functionExpression.Name, StringComparison.OrdinalIgnoreCase)

        If Not functions.Any Then Return Nothing

        Return functions.Single
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
            Return New TermWithoutBlanks(_Term.Substring(startIndex, length), parameters:=_Parameters, userFunctions:=_UserFunctions).GetExpression
        End Get
    End Property


    Private ReadOnly Property SubExpression(ByVal term As String) As Expression
        Get
            Return New TermWithoutBlanks(term, parameters:=_Parameters, userFunctions:=_UserFunctions).GetExpression
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


End Class

Public Class InvalidTermException
    Inherits ArgumentException

    Public Sub New(ByVal term As String)
        MyBase.New(Message:="The term is invalid: " & term)
    End Sub

End Class