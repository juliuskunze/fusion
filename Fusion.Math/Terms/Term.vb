Public Class Term
    Inherits TermBase

    Private _CharIsInBrackets As Boolean()

    Public Sub New(ByVal term As String, ByVal userContext As TermContext)
        Me.New(termWithoutBlanks:=New String((term.Where(Function(c) Not Char.IsWhiteSpace(c))).ToArray),
               context:=TermContext.DefaultContext.Merge(userContext), obsolete_signatureDifferParameter:=False)
    End Sub

    Friend Sub New(ByVal termWithoutBlanks As String,
                    ByVal context As TermContext, ByVal obsolete_signatureDifferParameter As Boolean)
        MyBase.New(Term:=termWithoutBlanks, context:=context)
    End Sub

    Public Overrides Function GetExpression() As Expression
        If _Term = "" Then Throw New InvalidTermException(_Term)

        Dim parsedDouble As Double
        If Double.TryParse(_Term, result:=parsedDouble) AndAlso Not _Term.StartsWith("("c) Then Return Expression.Constant(parsedDouble)

        Dim constants = From constant In _Context.Constants Where String.Equals(_Term, constant.Name, StringComparison.OrdinalIgnoreCase)
        If constants.Any Then Return constants.Single.ConstantExpression

        Dim parameters = From parameter In _Context.Parameters Where String.Equals(_Term, parameter.Name, StringComparison.OrdinalIgnoreCase)
        If parameters.Any Then Return parameters.Single

        Me.InitializeCharIsInBracketsArray()
        If TermIsInBrackets(startIndex:=0, endIndex:=_Term.Length - 1) Then Return SubstringExpression(_Term.Substring(startIndex:=1, length:=_Term.Length - 2))

        Dim startingFunction = Me.GetStartingFunction
        If startingFunction IsNot Nothing AndAlso
           TermIsInBrackets(startIndex:=startingFunction.Name.Length, endIndex:=_Term.Length - 1) Then
            Dim arguments =
                From argument
                In _Term.Substring(startingFunction.Name.Length + 1, length:=_Term.Length - 2 - startingFunction.Name.Length).Split(","c)
                Select SubExpression = SubstringExpression(argument)
            Return startingFunction.Expression(arguments:=arguments)
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

        If Char.IsLetter(_Term(0)) AndAlso _Term.All(Function(c) Char.IsLetterOrDigit(c)) Then Throw New InvalidTermException(_Term & " is not defined in this context.")

        Throw New InvalidTermException(_Term)
    End Function

    Private Function GetStartingFunction() As NamedExpression
        Dim functions = From functionExpression In _Context.Functions Where _Term.StartsWith(functionExpression.Name, StringComparison.OrdinalIgnoreCase)
        If functions.Any Then Return functions.Single

        Return Nothing
    End Function

    Private Sub InitializeCharIsInBracketsArray()
        ReDim _CharIsInBrackets(_Term.Length - 1)
        Dim bracketDepth As Integer = 0

        For i = 0 To _Term.Length - 1
            If _Term.Chars(i) = "("c Then
                bracketDepth += 1
            End If

            If bracketDepth < 0 Then
                Throw New InvalidTermException(_Term, message:="Missing ""("".")
            End If
            If bracketDepth > 0 Then
                _CharIsInBrackets(i) = True
            End If

            If _Term.Chars(i) = ")"c Then
                bracketDepth -= 1
            End If
        Next

        If bracketDepth <> 0 Then
            If bracketDepth > 0 Then Throw New InvalidTermException(_Term, message:="Missing "")"".")
        End If
    End Sub


    Private ReadOnly Property BeforeIndexExpression(ByVal index As Integer) As Expression
        Get
            Return Me.SubstringExpression(0, index)
        End Get
    End Property

    Private ReadOnly Property AfterIndexExpression(ByVal index As Integer) As Expression
        Get
            Return Me.SubstringExpression(index + 1, _Term.Length - 1 - index)
        End Get
    End Property

    Private ReadOnly Property SubstringExpression(ByVal startIndex As Integer, ByVal length As Integer) As Expression
        Get
            Return Me.SubstringExpression(_Term.Substring(startIndex, length))
        End Get
    End Property

    Private ReadOnly Property SubstringExpression(ByVal term As String) As Expression
        Get
            Return New Term(termWithoutBlanks:=term, context:=_Context, obsolete_signatureDifferParameter:=False).GetExpression
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

End Class