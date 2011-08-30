Public Class Term

    Private ReadOnly _LocatedString As LocatedString
    Private ReadOnly _Context As TermContext
    Private ReadOnly _TypeInformation As TypeInformation

    Private _CharIsInBrackets As Boolean()

    Public Sub New(term As String, type As NamedType, context As TermContext)
        Me.New(term:=term,
               TypeInformation:=New TypeInformation(type),
               context:=context)
    End Sub

    Public Sub New(term As String, typeInformation As TypeInformation, context As TermContext)
        If Not context.Types.Contains(NamedType.Real) Then Throw New CompilerException("Type Real must be defined in this context.")
        If Not context.Types.Contains(NamedType.Vector3D) Then Throw New CompilerException("Type Vector3D must be defined in this context.")
        If Not context.Types.Contains(NamedType.Collection) Then Throw New CompilerException("Type Collection must be defined in this context.")

        _LocatedString = New AnalizedString(term, AllowedBracketTypes:=CompilerTools.AllowedBracketTypes).ToLocated.Trim
        _Context = context
        _TypeInformation = typeInformation
    End Sub

    Public Sub New(term As LocatedString, type As NamedType, context As TermContext)
        Me.New(term:=term,
               TypeInformation:=New TypeInformation(type),
               context:=context)
    End Sub

    Public Sub New(term As LocatedString, typeInformation As TypeInformation, context As TermContext)
        If Not context.Types.Contains(NamedType.Real) Then Throw New CompilerException("Type Real must be defined in this context.")
        If Not context.Types.Contains(NamedType.Vector3D) Then Throw New CompilerException("Type Vector3D must be defined in this context.")
        If Not context.Types.Contains(NamedType.Collection) Then Throw New CompilerException("Type Collection must be defined in this context.")

        _LocatedString = term.Trim
        _Context = context
        _TypeInformation = typeInformation
    End Sub

    Private Function TryGetConstantOrParameterExpression() As ExpressionWithNamedType
        If _LocatedString.ToString = "" Then Throw New InvalidTermException(_LocatedString, message:="Expression expected.")
        If Not _LocatedString.ToString.IsValidIdentifier Then Return Nothing

        Dim constantExpression = Me.TryGetConstantExpression()
        If constantExpression IsNot Nothing Then Return constantExpression

        Dim parameterExpression = Me.TryGetParameterExpression()
        If parameterExpression IsNot Nothing Then Return parameterExpression

        Return Nothing
    End Function

    Private Function TryGetConstantExpression() As ExpressionWithNamedType
        Dim matchingConstant = _Context.TryParseConstant(_LocatedString.ToString)
        If matchingConstant Is Nothing Then Return Nothing

        Me.CheckTypeMatchIfNotInfer(type:=matchingConstant.Signature.Type)

        Return matchingConstant.ToExpressionWithNamedType
    End Function

    Private Function TryGetParameterExpression() As ExpressionWithNamedType
        Dim matchingParameter = _Context.TryParseParameter(_LocatedString.ToString)
        If matchingParameter Is Nothing Then Return Nothing

        Me.CheckTypeMatchIfNotInfer(type:=matchingParameter.Type)

        Return matchingParameter.ToExpressionWithNamedType
    End Function

    Public Function GetDelegate() As System.Delegate
        Return Expression.Lambda(body:=Me.GetExpression, parameters:=_Context.Parameters.Select(Function(p) p.Expression)).Compile
    End Function

    Public Function GetDelegate(Of TDelegate)() As TDelegate
        Dim lambda = Expression.Lambda(Of TDelegate)(body:=Me.GetExpression, parameters:=_Context.Parameters.Select(Function(p) p.Expression))

        Return lambda.Compile
    End Function

    Public Function TryGetDelegate() As System.Delegate
        Try
            Return Me.GetDelegate
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function TryGetDelegate(Of TDelegate)() As TDelegate
        Try
            Return Me.GetDelegate(Of TDelegate)()
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Private Function TryGetFunctionCall() As FunctionCall
        Try
            Return New FunctionCall(_LocatedString)
        Catch ex As LocatedCompilerException
            Return Nothing
        End Try
    End Function

    Private Sub CheckTypeMatchIfNotInfer(type As NamedType)
        If _TypeInformation.IsInfer Then Return

        If Not _TypeInformation.Type.SystemType.IsAssignableFrom(type.SystemType) Then Throw New InvalidTermException(_LocatedString, message:=String.Format("Type '{0}' is not compatible to type '{1}'.", type.Name, _TypeInformation.Type.Name))
    End Sub

    Private Sub CheckDelegateTypeMatch(delegateType As DelegateType)
        If _TypeInformation.IsInfer Then Return

        _TypeInformation.Type.Delegate.CheckIsAssignableFrom(delegateType)
    End Sub

    Public Function GetExpression() As System.Linq.Expressions.Expression
        Return Me.GetExpressionWithNamedType.Expression
    End Function

    Friend Function GetExpressionWithNamedType() As ExpressionWithNamedType
        Dim baseExpression = Me.TryGetConstantOrParameterExpression
        If baseExpression IsNot Nothing Then Return baseExpression

        Dim parsedDouble As Double
        If Double.TryParse(_LocatedString.ToString, result:=parsedDouble) AndAlso Not _LocatedString.ToString.StartsWith("("c) Then
            Return Me.GetRealExpression(parsedDouble:=parsedDouble)
        End If

        If _LocatedString.IsInBrackets(CompilerTools.VectorBracketType) Then
            Return Me.GetVector3DExpression
        End If

        If _LocatedString.IsInBrackets(CompilerTools.CollectionBracketType) Then
            Return Me.GetCollectionExpression()
        End If

        If Not _TypeInformation.IsInfer AndAlso _TypeInformation.Type.IsDelegate AndAlso _LocatedString.ToString.IsValidIdentifier Then Return _Context.ParseSingleFunctionWithName(_LocatedString).InvokableExpression.WithNamedType(_TypeInformation.Type)

        _CharIsInBrackets = _LocatedString.GetCharIsInBracketsArray

        If Me.TermIsInBrackets(startIndex:=0, endIndex:=_LocatedString.Length - 1) Then Return Me.SubstringExpression(_LocatedString.Substring(startIndex:=1, length:=_LocatedString.Length - 2), typeInformation:=_TypeInformation)
        Dim functionCallExpression = Me.TryGetFunctionCallExpression()
        If functionCallExpression IsNot Nothing Then Return functionCallExpression

        Dim orExpression = Me.TryGetBinaryOperatorExpression(NamedBinaryOperator.Or)
        If orExpression IsNot Nothing Then Return orExpression

        Dim andExpression = Me.TryGetBinaryOperatorExpression(NamedBinaryOperator.And)
        If andExpression IsNot Nothing Then Return andExpression

        Dim notEqualExpression = Me.TryGetBinaryOperatorExpression(NamedBinaryOperator.NotEqual)
        If notEqualExpression IsNot Nothing Then Return notEqualExpression

        Dim greaterThanOrEqualExpression = Me.TryGetBinaryOperatorExpression(NamedBinaryOperator.GreaterThanOrEqual)
        If greaterThanOrEqualExpression IsNot Nothing Then Return greaterThanOrEqualExpression

        Dim greaterThanExpression = Me.TryGetBinaryOperatorExpression(NamedBinaryOperator.GreaterThan)
        If greaterThanExpression IsNot Nothing Then Return greaterThanExpression

        Dim lessThanOrEqualExpression = Me.TryGetBinaryOperatorExpression(NamedBinaryOperator.LessThanOrEqual)
        If lessThanOrEqualExpression IsNot Nothing Then Return lessThanOrEqualExpression

        Dim lessThanExpression = Me.TryGetBinaryOperatorExpression(NamedBinaryOperator.LessThan)
        If lessThanExpression IsNot Nothing Then Return lessThanExpression

        Dim equalExpression = Me.TryGetBinaryOperatorExpression(NamedBinaryOperator.Equal)
        If equalExpression IsNot Nothing Then Return equalExpression

        Select Case _LocatedString.Chars(0)
            Case "+"c, "-"c
                Dim firstNotSignIndex As Integer
                If MinusCountAtStartIsEven(out_firstNotSignIndex:=firstNotSignIndex) Then
                    Return SubstringExpression(startIndex:=firstNotSignIndex, length:=_LocatedString.Length - firstNotSignIndex, typeInformation:=New TypeInformation(NamedType.Real))
                Else
                    Dim negateAndAddExpression = Me.TryGetBinaryOperatorExpression(NamedBinaryOperator.NegateFirstAndAddSecond, startIndex:=firstNotSignIndex)
                    If negateAndAddExpression IsNot Nothing Then Return negateAndAddExpression

                    Dim negateAndSubtractExpression = Me.TryGetBinaryOperatorExpression(NamedBinaryOperator.NegateFirstAndSubtractSecond, startIndex:=firstNotSignIndex)
                    If negateAndSubtractExpression IsNot Nothing Then Return negateAndSubtractExpression

                    Dim inner = Me.SubstringExpression(startIndex:=firstNotSignIndex, length:=_LocatedString.Length - firstNotSignIndex, typeInformation:=New TypeInformation(NamedType.Real))

                    Return New ExpressionWithNamedType(Expression.Negate(inner.Expression), inner.NamedType)
                End If
        End Select

        Dim addExpression = Me.TryGetBinaryOperatorExpression(NamedBinaryOperator.Add)
        If addExpression IsNot Nothing Then Return addExpression

        Dim subtractExpression = Me.TryGetBinaryOperatorExpression(NamedBinaryOperator.Subtract)
        If subtractExpression IsNot Nothing Then Return subtractExpression

        Dim multiplyExpression = Me.TryGetBinaryOperatorExpression(NamedBinaryOperator.Multiply)
        If multiplyExpression IsNot Nothing Then Return multiplyExpression

        Dim divideExpression = Me.TryGetBinaryOperatorExpression(NamedBinaryOperator.Divide)
        If divideExpression IsNot Nothing Then Return divideExpression

        Dim powerExpression = Me.TryGetBinaryOperatorExpression(NamedBinaryOperator.Power)
        If powerExpression IsNot Nothing Then Return powerExpression

        If _LocatedString.Chars(0) = "!" Then
            Dim inner = Me.SubstringExpression(startIndex:=1, length:=_LocatedString.Length - 1, typeInformation:=New TypeInformation(NamedType.Boolean))

            Return New ExpressionWithNamedType(Expression.Not(inner.Expression), inner.NamedType)
        End If

        If _LocatedString.ToString.IsValidIdentifier Then Throw New InvalidTermException(Term:=_LocatedString, message:=String.Format("'{0}' is not defined in this context.", _LocatedString))

        Throw New InvalidTermException(_LocatedString)
    End Function

    Private Function TryGetFunctionCallExpression() As ExpressionWithNamedType
        Dim functionCall = Me.TryGetFunctionCall
        If functionCall Is Nothing Then Return Nothing

        Return Me.GetFunctionCallExpression(functionCall)
    End Function

    Private Function GetRealExpression(ByVal parsedDouble As Double) As ExpressionWithNamedType
        Me.CheckTypeMatchIfNotInfer(NamedType.Real)
        Return Expression.Constant(type:=GetType(Double), value:=parsedDouble).WithNamedType(NamedType.Real)
    End Function

    Private Function GetFunctionCallExpression(ByVal functionCall As FunctionCall) As ExpressionWithNamedType
        Dim argumentStrings = functionCall.Arguments

        If CompilerTools.IdentifierEquals(functionCall.FunctionName.ToString, Keywords.Cases) Then
            Return Me.GetCasesExpression(argumentStrings)
        End If

        Dim functionInstance = _Context.ParseFunction(functionCall)

        Dim parameters = functionInstance.Signature.DelegateType.Parameters

        Dim arguments = New List(Of Expression)
        For parameterIndex = 0 To parameters.Count - 1
            Dim parameter = parameters(parameterIndex)
            Dim argumentString = argumentStrings(parameterIndex)

            Dim parts = argumentString.SplitIfSeparatorIsNotInBrackets(":"c, bracketTypes:=CompilerTools.AllowedBracketTypes)
            Dim argumentTerm = GetArgumentTerm(argumentString, parts, parameter)

            arguments.Add(Me.SubstringExpression(argumentTerm, typeInformation:=New TypeInformation(parameter.Type)).Expression)
        Next

        Return New ExpressionWithNamedType(functionInstance.CallExpressionBuilder.Run(arguments:=arguments), _TypeInformation.Type)
    End Function

    Private Function GetCollectionExpression() As ExpressionWithNamedType
        Dim collectionArgumentStrings = CompilerTools.GetCollectionArguments(_LocatedString)

        Dim elementTypeInformation As TypeInformation
        If _TypeInformation.IsInfer Then
            elementTypeInformation = TypeInformation.Infer
        Else
            Dim type = _TypeInformation.Type
            'TODO: Check collection type match
            elementTypeInformation = New TypeInformation(type.TypeArguments.Single)

            Me.CheckTypeMatchIfNotInfer(type:=NamedType.Collection.MakeGenericType(typeArguments:=type.TypeArguments))
        End If

        Dim arguments = collectionArgumentStrings.Select(Function(argumentString) New Term(Term:=argumentString, TypeInformation:=elementTypeInformation, context:=_Context).GetExpressionWithNamedType)

        Dim resultElementType As NamedType
        If _TypeInformation.IsInfer Then
            resultElementType = arguments.First.NamedType
        Else
            resultElementType = _TypeInformation.Type.TypeArguments.Single
        End If

        Return Expression.NewArrayInit(type:=resultElementType.SystemType, initializers:=arguments.Select(Function(argument) argument.Expression)).WithNamedType(NamedType.Collection.MakeGenericType({resultElementType}))
    End Function

    Private Function MinusCountAtStartIsEven(ByRef out_firstNotSignIndex As Integer) As Boolean
        Dim i As Integer

        Dim minusCountAtStart = 0
        For i = 0 To _LocatedString.Length - 1
            Select Case _LocatedString.Chars(i)
                Case "+"c
                Case "-"c
                    minusCountAtStart -= 1
                Case Else
                    out_firstNotSignIndex = i
                    Exit For
            End Select
        Next

        Return (minusCountAtStart Mod 2 = 0)
    End Function

    Private Function TryGetBinaryOperatorExpression(namedOperator As NamedBinaryOperator, Optional startIndex As Integer = 0) As ExpressionWithNamedType
        For i = startIndex To _LocatedString.Length - namedOperator.Name.Length - startIndex
            If Not _CharIsInBrackets(i) AndAlso
               _LocatedString.Substring(i, namedOperator.Name.Length).ToString = namedOperator.Name Then

                Dim argumentTypesInformation As BinaryOperatorArgumentTypesInformation

                Try
                    argumentTypesInformation = namedOperator.GetArgumentTypesInformation(resultTypeInformation:=_TypeInformation)
                Catch ex As CompilerException
                    Throw ex.Locate(locatedString:=_LocatedString)
                End Try
                Dim term1 = Me.BeforeIndexExpression(i, typeInformation:=argumentTypesInformation.Argument1TypeInformation, startIndex:=startIndex)
                Dim term2 = Me.AfterIndexExpression(i + namedOperator.Name.Length - 1, typeInformation:=argumentTypesInformation.Argument2TypeInformation)


                Dim operatorOverload As BinaryOperatorOverload
                Try
                    operatorOverload = namedOperator.ParseOverload(argumentType1:=term1.NamedType, argumentType2:=term2.NamedType, resultTypeInformation:=_TypeInformation)
                Catch ex As CompilerException
                    Throw ex.Locate(_LocatedString)
                End Try

                Return operatorOverload.GetExpressionWithNamedType(term1.Expression, term2.Expression)
            End If
        Next

        Return Nothing
    End Function

    Private Function GetCasesExpression(ByVal argumentStrings As IEnumerable(Of LocatedString)) As ExpressionWithNamedType
        Dim casesExpression As Expression = Nothing
        Dim typeOfFirstTerm As NamedType = Nothing

        For index = argumentStrings.Count - 1 To 0 Step -1
            Dim argumentString = argumentStrings(index)
            Dim parts = argumentString.SplitIfSeparatorIsNotInBrackets(":"c, bracketTypes:=CompilerTools.AllowedBracketTypes)
            If parts.Count <> 2 Then Throw New InvalidTermException(_LocatedString, String.Format("Invalid case: '{0}'.", argumentString))

            Dim conditionPart = parts.First
            Dim termPart = parts.Last

            Dim termExpression = New Term(parts.Last, _TypeInformation, _Context).GetExpressionWithNamedType

            If index = 0 Then typeOfFirstTerm = termExpression.NamedType

            If index = argumentStrings.Count - 1 Then
                If Not CompilerTools.IdentifierEquals(conditionPart.Trim.ToString, Keywords.Else) Then Throw New InvalidTermException(conditionPart, "Last case must be case else.")
                casesExpression = termExpression.Expression
                Continue For
            End If

            Dim conditionExpression = New Term(conditionPart, NamedType.Boolean, _Context).GetExpression

            casesExpression = Expression.Condition(conditionExpression, ifTrue:=termExpression.Expression, ifFalse:=casesExpression)
        Next

        Return casesExpression.WithNamedType(typeOfFirstTerm)
    End Function

    Private Function GetArgumentTerm(ByVal argumentString As LocatedString, ByVal parts As IEnumerable(Of LocatedString), ByVal parameter As NamedParameter) As LocatedString
        Select Case parts.Count
            Case 1
                Return argumentString
            Case 2
                Dim parameterName = parts.First.Trim
                If Not CompilerTools.IdentifierEquals(parameter.Name, parameterName.ToString) Then Throw New InvalidTermException(parameterName, String.Format("Wrong parameter name: '{0}'; '{1}' expected.", parameterName, parameter.Name))
                Return parts.Last
            Case Else
                Throw New InvalidTermException(_LocatedString, String.Format("Invalid argument expression: '{0}'.", argumentString))
        End Select
    End Function

    Private ReadOnly Property BeforeIndexExpression(index As Integer, typeInformation As TypeInformation, Optional startIndex As Integer = 0) As ExpressionWithNamedType
        Get
            Return Me.SubstringExpression(startIndex:=startIndex, length:=index - startIndex, typeInformation:=typeInformation)
        End Get
    End Property

    Private ReadOnly Property AfterIndexExpression(index As Integer, typeInformation As TypeInformation) As ExpressionWithNamedType
        Get
            Return Me.SubstringExpression(index + 1, _LocatedString.Length - 1 - index, typeInformation)
        End Get
    End Property

    Private ReadOnly Property SubstringExpression(startIndex As Integer, length As Integer, typeInformation As TypeInformation) As ExpressionWithNamedType
        Get
            Return Me.SubstringExpression(_LocatedString.Substring(startIndex, length), typeInformation)
        End Get
    End Property

    Private ReadOnly Property SubstringExpression(term As LocatedString, typeInformation As TypeInformation) As ExpressionWithNamedType
        Get
            Return New Term(term:=term, typeInformation:=typeInformation, context:=_Context).GetExpressionWithNamedType
        End Get
    End Property

    Private Function GetVector3DExpression() As ExpressionWithNamedType
        Me.CheckTypeMatchIfNotInfer(NamedType.Vector3D)

        Dim components = CompilerTools.GetArguments(_LocatedString.Trim, BracketType:=CompilerTools.VectorBracketType)

        If components.Count <> 3 Then Throw New InvalidTermException(_LocatedString, "The component count of a 3D-vector must be 3.")

        Dim xExpression = New Term(Term:=components(0), context:=_Context, Type:=NamedType.Real).GetExpression
        Dim yExpression = New Term(Term:=components(1), context:=_Context, Type:=NamedType.Real).GetExpression
        Dim zExpression = New Term(Term:=components(2), context:=_Context, Type:=NamedType.Real).GetExpression

        If xExpression.Type <> GetType(Double) OrElse
           yExpression.Type <> GetType(Double) OrElse
           zExpression.Type <> GetType(Double) Then Throw New InvalidTermException(_LocatedString, message:="The components of a vector must be real numbers.")

        Dim expression = New FunctionCallExpressionBuilder(Of Vector3DConstructor)(LambdaExpression:=Function(x, y, z) New Vector3D(x, y, z)).Run(arguments:={xExpression, yExpression, zExpression})

        Return expression.WithNamedType(NamedType.Vector3D)
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