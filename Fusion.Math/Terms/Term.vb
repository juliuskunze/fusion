Public Class Term
    Inherits TermBase(Of Double)

    Private _CharIsInBrackets As Boolean()

    Public Sub New(term As String, userContext As TermContext)
        Me.New(term:=term,
               context:=TermContext.DefaultContext.Merge(userContext), obsolete_signatureDifferParameter:=0)
    End Sub

    Private Sub New(termWithoutBlanks As String,
                    context As TermContext, obsolete_signatureDifferParameter As Boolean)
        MyBase.New(termWithoutBlanks:=termWithoutBlanks, context:=context)
    End Sub

    Private Sub New(term As String, context As TermContext, obsolete_signatureDifferParameter As Byte)
        MyBase.New(termWithoutBlanks:=term.WithoutBlanks, context:=context)
    End Sub

    Public Overrides Function GetExpression() As Expression
        Dim baseExpression = MyBase.TryGetConstantOrParameterExpression
        If baseExpression IsNot Nothing Then Return baseExpression

        Dim parsedDouble As Double
        If Double.TryParse(_TermWithoutBlanks, result:=parsedDouble) AndAlso Not _TermWithoutBlanks.StartsWith("("c) Then Return Expression.Constant(parsedDouble)

        _CharIsInBrackets = _TermWithoutBlanks.GetCharIsInBracketsArray

        If TermIsInBrackets(startIndex:=0, endIndex:=_TermWithoutBlanks.Length - 1) Then Return Me.SubstringExpression(_TermWithoutBlanks.Substring(startIndex:=1, length:=_TermWithoutBlanks.Length - 2))

        Dim functionCall = Me.TryGetFunctionCall
        If functionCall IsNot Nothing Then
            Dim matchingFunctions = From functionExpression In _Context.Functions Where functionCall.FunctionName.Equals(functionExpression.Name, StringComparison.OrdinalIgnoreCase)
            If Not matchingFunctions.Any Then Throw New ArgumentException("Function '" & functionCall.FunctionName & "' is not defined in this context.")
            Dim matchingFunction = matchingFunctions.Single
            Dim arguments = functionCall.Arguments.Select(Function(argument) Me.SubstringExpression(argument))

            Return matchingFunction.Expression(arguments:=arguments)
        End If

        Select Case _TermWithoutBlanks.Chars(0)
            Case "+"c, "-"c
                Dim minusCountAtStart = 0
                Dim notSignIndex As Integer
                For i = 0 To _TermWithoutBlanks.Length - 1
                    Select Case _TermWithoutBlanks(i)
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
                    Return SubstringExpression(startIndex:=notSignIndex, length:=_TermWithoutBlanks.Length - notSignIndex)
                Else
                    For i = notSignIndex To _TermWithoutBlanks.Length - 1
                        If Not _CharIsInBrackets(i) AndAlso _TermWithoutBlanks.Chars(i) = "+"c Then Return Expression.Add(Expression.Negate(Me.SubstringExpression(startIndex:=notSignIndex, length:=i - notSignIndex)), AfterIndexExpression(i))
                    Next

                    For i = notSignIndex To _TermWithoutBlanks.Length - 1
                        If Not _CharIsInBrackets(i) AndAlso _TermWithoutBlanks.Chars(i) = "-"c Then Return Expression.Subtract(Expression.Negate(Me.SubstringExpression(startIndex:=notSignIndex, length:=i - notSignIndex)), AfterIndexExpression(i))
                    Next

                    Return Expression.NegateChecked(Me.SubstringExpression(startIndex:=notSignIndex, length:=_TermWithoutBlanks.Length - notSignIndex))
                End If
        End Select

        For i = 0 To _TermWithoutBlanks.Length - 1
            If Not _CharIsInBrackets(i) AndAlso _TermWithoutBlanks.Chars(i) = "+"c Then Return Expression.Add(Me.BeforeIndexExpression(i), Me.AfterIndexExpression(i))
        Next

        For i = 0 To _TermWithoutBlanks.Length - 1
            If Not _CharIsInBrackets(i) AndAlso _TermWithoutBlanks.Chars(i) = "-"c Then Return Expression.Subtract(Me.BeforeIndexExpression(i), Me.AfterIndexExpression(i))
        Next

        For i = 0 To _TermWithoutBlanks.Length - 1
            If Not _CharIsInBrackets(i) AndAlso _TermWithoutBlanks.Chars(i) = "*"c Then Return Expression.Multiply(Me.BeforeIndexExpression(i), Me.AfterIndexExpression(i))
        Next

        For i = 0 To _TermWithoutBlanks.Length - 1
            If Not _CharIsInBrackets(i) AndAlso _TermWithoutBlanks.Chars(i) = "/"c Then Return Expression.Divide(Me.BeforeIndexExpression(i), Me.AfterIndexExpression(i))
        Next

        For i = 0 To _TermWithoutBlanks.Length - 1
            If Not _CharIsInBrackets(i) AndAlso _TermWithoutBlanks.Chars(i) = "^"c Then Return Expression.Power(Me.BeforeIndexExpression(i), Me.AfterIndexExpression(i))
        Next

        If _TermWithoutBlanks.IsValidVariableName Then Throw New InvalidTermException("Constant '" & _TermWithoutBlanks & "' is not defined in this context.")
        For functionNameLength = _TermWithoutBlanks.Length To 1 Step -1

        Next

        Throw New InvalidTermException(_TermWithoutBlanks)
    End Function


    Private ReadOnly Property BeforeIndexExpression(index As Integer) As Expression
        Get
            Return Me.SubstringExpression(0, index)
        End Get
    End Property

    Private ReadOnly Property AfterIndexExpression(index As Integer) As Expression
        Get
            Return Me.SubstringExpression(index + 1, _TermWithoutBlanks.Length - 1 - index)
        End Get
    End Property

    Private ReadOnly Property SubstringExpression(startIndex As Integer, length As Integer) As Expression
        Get
            Return Me.SubstringExpression(_TermWithoutBlanks.Substring(startIndex, length))
        End Get
    End Property

    Private ReadOnly Property SubstringExpression(term As String) As Expression
        Get
            Return New Term(termWithoutBlanks:=term, context:=_Context, obsolete_signatureDifferParameter:=False).GetExpression
        End Get
    End Property

    Private ReadOnly Property TermIsInBrackets(startIndex As Integer, endIndex As Integer) As Boolean
        Get
            For i = startIndex To endIndex
                If Not _CharIsInBrackets(i) Then Return False
            Next

            Return True
        End Get
    End Property

End Class