Public Class Term
    Inherits TermBase

    Private _CharIsInBrackets As Boolean()

    Public Sub New(term As String, type As NamedType, userContext As TermContext)
        Me.New(term:=term, type:=type,
               context:=TermContext.DefaultContext.Merge(userContext), obsolete_signatureDifferParameter:=0)
    End Sub

    Private Sub New(term As String, type As NamedType,
                    context As TermContext, obsolete_signatureDifferParameter As Boolean)
        MyBase.New(term:=term, type:=type, context:=context)
    End Sub

    Private Sub New(term As String, type As NamedType, context As TermContext, obsolete_signatureDifferParameter As Byte)
        MyBase.New(term:=term.WithoutBlanks, type:=type, context:=context)
    End Sub

    Public Overrides Function GetExpression() As Expression
        Dim baseExpression = MyBase.TryGetConstantOrParameterExpression
        If baseExpression IsNot Nothing Then Return baseExpression

        Dim parsedDouble As Double
        If Double.TryParse(_Term, result:=parsedDouble) AndAlso Not _Term.StartsWith("("c) Then
            Me.CheckTypeMatch(type:=NamedType.Real)
            Return Expression.Constant(type:=GetType(Double), value:=parsedDouble)
        End If

        If _TrimmedTerm.IsInBrackets(bracketTypes:={BracketType.Inequality}) Then
            Me.CheckTypeMatch(type:=NamedType.Vector3D)
            Return Me.GetVector3DExpression
        End If

        _CharIsInBrackets = _Term.GetCharIsInBracketsArray

        If TermIsInBrackets(startIndex:=0, endIndex:=_Term.Length - 1) Then Return Me.SubstringExpression(_Term.Substring(startIndex:=1, length:=_Term.Length - 2), type:=_Type)

        Dim functionCall = Me.TryGetFunctionCall
        If functionCall IsNot Nothing Then
            Dim matchingFunctions = From functionExpression In _Context.Functions Where functionCall.FunctionName.Equals(functionExpression.Type.Name, StringComparison.OrdinalIgnoreCase)
            If Not matchingFunctions.Any Then Throw New ArgumentException("Function '" & functionCall.FunctionName & "' is not defined in this context.")
            Dim matchingFunction = matchingFunctions.Single
            Dim parameters = matchingFunction.Type.Parameters
            Dim argumentStrings = functionCall.Arguments
            If parameters.Count <> argumentStrings.Count Then Throw New ArgumentException("Wrong argument count.")

            Dim arguments = New List(Of Expression)
            For parameterIndex = 0 To parameters.Count - 1
                Dim parameter = parameters(parameterIndex)
                Dim argument = argumentStrings(parameterIndex)
                arguments.Add(Me.SubstringExpression(argument, type:=parameter.Type))
            Next
            Return matchingFunction.Expression(arguments:=arguments)
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
                    Return SubstringExpression(startIndex:=notSignIndex, length:=_Term.Length - notSignIndex, type:=NamedType.Real)
                Else
                    For i = notSignIndex To _Term.Length - 1
                        If Not _CharIsInBrackets(i) AndAlso _Term.Chars(i) = "+"c Then Return Expression.Add(Expression.Negate(Me.SubstringExpression(startIndex:=notSignIndex, length:=i - notSignIndex, type:=NamedType.Real)), AfterIndexExpression(i, type:=NamedType.Real))
                    Next

                    For i = notSignIndex To _Term.Length - 1
                        If Not _CharIsInBrackets(i) AndAlso _Term.Chars(i) = "-"c Then Return Expression.Subtract(Expression.Negate(Me.SubstringExpression(startIndex:=notSignIndex, length:=i - notSignIndex, type:=NamedType.Real)), AfterIndexExpression(i, type:=NamedType.Real))
                    Next

                    Return Expression.NegateChecked(Me.SubstringExpression(startIndex:=notSignIndex, length:=_Term.Length - notSignIndex, type:=NamedType.Real))
                End If
        End Select

        For i = 0 To _Term.Length - 1
            If Not _CharIsInBrackets(i) AndAlso _Term.Chars(i) = "+"c Then Return Expression.Add(Me.BeforeIndexExpression(i, type:=NamedType.Real), Me.AfterIndexExpression(i, type:=NamedType.Real))
        Next

        For i = 0 To _Term.Length - 1
            If Not _CharIsInBrackets(i) AndAlso _Term.Chars(i) = "-"c Then Return Expression.Subtract(Me.BeforeIndexExpression(i, type:=NamedType.Real), Me.AfterIndexExpression(i, type:=NamedType.Real))
        Next

        For i = 0 To _Term.Length - 1
            If Not _CharIsInBrackets(i) AndAlso _Term.Chars(i) = "*"c Then Return Expression.Multiply(Me.BeforeIndexExpression(i, type:=NamedType.Real), Me.AfterIndexExpression(i, type:=NamedType.Real))
        Next

        For i = 0 To _Term.Length - 1
            If Not _CharIsInBrackets(i) AndAlso _Term.Chars(i) = "/"c Then Return Expression.Divide(Me.BeforeIndexExpression(i, type:=NamedType.Real), Me.AfterIndexExpression(i, type:=NamedType.Real))
        Next

        For i = 0 To _Term.Length - 1
            If Not _CharIsInBrackets(i) AndAlso _Term.Chars(i) = "^"c Then Return Expression.Power(Me.BeforeIndexExpression(i, type:=NamedType.Real), Me.AfterIndexExpression(i, type:=NamedType.Real))
        Next

        If _Term.IsValidVariableName Then Throw New InvalidTermException(Term:=_Term, message:="Constant '" & _Term & "' is not defined in this context.")

        Throw New InvalidTermException(_Term)
    End Function


    Private ReadOnly Property BeforeIndexExpression(index As Integer, type As NamedType) As Expression
        Get
            Return Me.SubstringExpression(0, index, type)
        End Get
    End Property

    Private ReadOnly Property AfterIndexExpression(index As Integer, type As NamedType) As Expression
        Get
            Return Me.SubstringExpression(index + 1, _Term.Length - 1 - index, type)
        End Get
    End Property

    Private ReadOnly Property SubstringExpression(startIndex As Integer, length As Integer, type As NamedType) As Expression
        Get
            Return Me.SubstringExpression(_Term.Substring(startIndex, length), type)
        End Get
    End Property

    Private ReadOnly Property SubstringExpression(term As String, type As NamedType) As Expression
        Get
            Return New Term(term:=term, type:=type, context:=_Context, obsolete_signatureDifferParameter:=False).GetExpression
        End Get
    End Property

    Private Function GetVector3DExpression() As System.Linq.Expressions.Expression
        Dim baseExpression = MyBase.TryGetConstantOrParameterExpression
        If baseExpression IsNot Nothing Then Return baseExpression

        Dim components = CompilerTools.GetArgumentsOrParameters(_Term.Trim, bracketTypes:={BracketType.Inequality})

        If components.Count <> 3 Then Throw New InvalidTermException(_Term, "The component count of a 3D-vector must be 3.")

        Dim xExpression = New Term(Term:=components(0), userContext:=_Context, Type:=NamedType.Real).GetExpression
        Dim yExpression = New Term(Term:=components(1), userContext:=_Context, Type:=NamedType.Real).GetExpression
        Dim zExpression = New Term(Term:=components(2), userContext:=_Context, Type:=NamedType.Real).GetExpression

        If xExpression.Type <> GetType(Double) OrElse
           yExpression.Type <> GetType(Double) OrElse
           zExpression.Type <> GetType(Double) Then Throw New InvalidTermException(_Term, message:="The components of a vector must be real numbers.")

        Return FunctionExpression.GetFunctionExpressionBuilder(Of Vector3DConstructor)(lambdaExpression:=Function(x, y, z) New Vector3D(x, y, z)).Invoke(arguments:={xExpression, yExpression, zExpression})
    End Function

    Private Delegate Function Vector3DConstructor(x As Double, y As Double, z As Double) As Vector3D

    Private ReadOnly Property TermIsInBrackets(startIndex As Integer, endIndex As Integer) As Boolean
        Get
            For i = startIndex To endIndex
                If Not _CharIsInBrackets(i) Then Return False
            Next

            Return True
        End Get
    End Property

End Class