Public Class NamedBinaryOperator

    Private ReadOnly _Name As String
    Public ReadOnly Property Name As String
        Get
            Return _Name
        End Get
    End Property

    Private ReadOnly _Overloads As IEnumerable(Of BinaryOperatorOverload)
    Public ReadOnly Property [Overloads] As IEnumerable(Of BinaryOperatorOverload)
        Get
            Return _Overloads
        End Get
    End Property


    Public Sub New(name As String,
                   [overloads] As IEnumerable(Of BinaryOperatorOverload))
        _Name = name
        _Overloads = [overloads]
    End Sub

    Public Function GetArgumentTypesInformation(resultTypeInformation As TypeInformation) As BinaryOperatorArgumentTypesInformation
        If resultTypeInformation.IsInfer Then
            Return BinaryOperatorArgumentTypesInformation.Infer
        Else
            Dim neededResultType = resultTypeInformation.Type

            Dim matchingOverloads = _Overloads.Where(Function(overload) neededResultType.IsAssignableFrom(overload.ResultType))

            If Not matchingOverloads.Any Then Throw New InvalidOperationException(String.Format("There is no binary operator '{0}' with return type '{1}'.", Me.Name, neededResultType.Name))
            If matchingOverloads.Count = 1 Then Return matchingOverloads.Single.ArgumentTypesInformation

            Return BinaryOperatorArgumentTypesInformation.Infer
        End If
    End Function

    Public Function ParseOverload(argumentType1 As NamedType, argumentType2 As NamedType, resultTypeInformation As TypeInformation) As BinaryOperatorOverload
        Dim matchingOverloads = _Overloads.Where(Function(overload) overload.ArgumentType1.IsAssignableFrom(argumentType1) AndAlso overload.ArgumentType2.IsAssignableFrom(argumentType2))

        If Not matchingOverloads.Any Then Throw New InvalidOperationException(String.Format("There is no binary operator '{0}' that accepts argument types '{1}' and '{2}'.", Me.Name, argumentType1.Name, argumentType2.Name))

        If resultTypeInformation.IsInfer Then
            If matchingOverloads.Count <> 1 Then Throw New InvalidOperationException(String.Format("There are different operators '{0}' that accepts argument types '{1}' and '{2}'.", Me.Name, argumentType1.Name, argumentType2.Name))

            Return matchingOverloads.Single
        Else
            Dim neededResultType = resultTypeInformation.Type
            matchingOverloads = matchingOverloads.Where(Function(overload) resultTypeInformation.Type.IsAssignableFrom(overload.ResultType))

            If Not matchingOverloads.Any Then Throw New InvalidOperationException(String.Format("There is no binary operator '{0}' with return type '{1}' that accepts argument types '{2}' and '{3}'.", Me.Name, neededResultType.Name, argumentType1.Name, argumentType2.Name))
            If matchingOverloads.Count <> 1 Then Throw New InvalidOperationException(String.Format("Ther are different operators '{0}' with return type '{1}' that accepts argument types '{2}' and '{3}'.", Me.Name, neededResultType.Name, argumentType1.Name, argumentType2.Name))

            Return matchingOverloads.Single
        End If
    End Function

    Private Shared ReadOnly _AddExpressionBuilder As BinaryOperatorExpressionBuilder = Function(e1, e2) Expressions.Expression.Add(e1, e2)
    Public Shared ReadOnly Add As New NamedBinaryOperator(
        "+"c,
        {New BinaryOperatorOverload(NamedType.Real, NamedType.Real, resultType:=NamedType.Real, expressionBuilder:=_AddExpressionBuilder),
         New BinaryOperatorOverload(NamedType.Vector3D, NamedType.Vector3D, resultType:=NamedType.Vector3D, expressionBuilder:=_AddExpressionBuilder)})

    Private Shared ReadOnly _SubtractExpressionBuilder As BinaryOperatorExpressionBuilder = Function(e1, e2) Expressions.Expression.Subtract(e1, e2)
    Public Shared ReadOnly Subtract As New NamedBinaryOperator(
        "-"c,
        {New BinaryOperatorOverload(NamedType.Real, NamedType.Real, resultType:=NamedType.Real, expressionBuilder:=_SubtractExpressionBuilder),
         New BinaryOperatorOverload(NamedType.Vector3D, NamedType.Vector3D, resultType:=NamedType.Vector3D, expressionBuilder:=_SubtractExpressionBuilder)})

    Private Shared ReadOnly _MultiplyExpressionBuilder As BinaryOperatorExpressionBuilder = Function(e1, e2) Expressions.Expression.Multiply(e1, e2)
    Public Shared ReadOnly Multiply As New NamedBinaryOperator(
        "*"c,
        {New BinaryOperatorOverload(NamedType.Real, NamedType.Real, resultType:=NamedType.Real, expressionBuilder:=_MultiplyExpressionBuilder),
         New BinaryOperatorOverload(NamedType.Vector3D, NamedType.Real, resultType:=NamedType.Vector3D, expressionBuilder:=_MultiplyExpressionBuilder),
         New BinaryOperatorOverload(NamedType.Real, NamedType.Vector3D, resultType:=NamedType.Vector3D, expressionBuilder:=_MultiplyExpressionBuilder),
         New BinaryOperatorOverload(NamedType.Vector3D, NamedType.Vector3D, resultType:=NamedType.Real, expressionBuilder:=_MultiplyExpressionBuilder)})

    Private Shared ReadOnly _DivideExpressionBuilder As BinaryOperatorExpressionBuilder = Function(e1, e2) Expressions.Expression.Divide(e1, e2)
    Public Shared ReadOnly Divide As New NamedBinaryOperator(
        "/"c,
        {New BinaryOperatorOverload(NamedType.Real, NamedType.Real, resultType:=NamedType.Real, expressionBuilder:=_DivideExpressionBuilder)})

    Private Shared ReadOnly _PowerExpressionBuilder As BinaryOperatorExpressionBuilder = Function(e1, e2) Expressions.Expression.Power(e1, e2)
    Public Shared ReadOnly Power As New NamedBinaryOperator(
        "^"c,
        {New BinaryOperatorOverload(NamedType.Real, NamedType.Real, resultType:=NamedType.Real, expressionBuilder:=_PowerExpressionBuilder)})

    Private Shared ReadOnly _OrExpressionBuilder As BinaryOperatorExpressionBuilder = Function(e1, e2) Expressions.Expression.OrElse(e1, e2)
    Public Shared ReadOnly [Or] As New NamedBinaryOperator(
        "|"c,
        {New BinaryOperatorOverload(NamedType.Boolean, NamedType.Boolean, resultType:=NamedType.Boolean, expressionBuilder:=_OrExpressionBuilder)})

    Private Shared ReadOnly _AndExpressionBuilder As BinaryOperatorExpressionBuilder = Function(e1, e2) Expressions.Expression.AndAlso(e1, e2)
    Public Shared ReadOnly [And] As New NamedBinaryOperator(
        "&"c,
        {New BinaryOperatorOverload(NamedType.Boolean, NamedType.Boolean, resultType:=NamedType.Boolean, expressionBuilder:=_AndExpressionBuilder)})

    Private Shared ReadOnly _NotEqualExpressionBuilder As BinaryOperatorExpressionBuilder = Function(e1, e2) Expressions.Expression.NotEqual(e1, e2)
    Public Shared ReadOnly NotEqual As New NamedBinaryOperator(
        "<>",
        {New BinaryOperatorOverload(NamedType.Boolean, NamedType.Boolean, resultType:=NamedType.Boolean, expressionBuilder:=_NotEqualExpressionBuilder),
         New BinaryOperatorOverload(NamedType.Real, NamedType.Real, resultType:=NamedType.Boolean, expressionBuilder:=_NotEqualExpressionBuilder),
         New BinaryOperatorOverload(NamedType.Vector3D, NamedType.Vector3D, resultType:=NamedType.Boolean, expressionBuilder:=_NotEqualExpressionBuilder)})

    Private Shared ReadOnly _EqualExpressionBuilder As BinaryOperatorExpressionBuilder = Function(e1, e2) Expressions.Expression.Equal(e1, e2)
    Public Shared ReadOnly Equal As New NamedBinaryOperator(
        "=",
        {New BinaryOperatorOverload(NamedType.Boolean, NamedType.Boolean, resultType:=NamedType.Boolean, expressionBuilder:=_EqualExpressionBuilder),
         New BinaryOperatorOverload(NamedType.Real, NamedType.Real, resultType:=NamedType.Boolean, expressionBuilder:=_EqualExpressionBuilder),
         New BinaryOperatorOverload(NamedType.Vector3D, NamedType.Vector3D, resultType:=NamedType.Boolean, expressionBuilder:=_EqualExpressionBuilder)})

    Private Shared ReadOnly _LessThanExpressionBuilder As BinaryOperatorExpressionBuilder = Function(e1, e2) Expressions.Expression.LessThan(e1, e2)
    Public Shared ReadOnly LessThan As New NamedBinaryOperator(
        "<",
        {New BinaryOperatorOverload(NamedType.Real, NamedType.Real, resultType:=NamedType.Boolean, expressionBuilder:=_LessThanExpressionBuilder)})

    Private Shared ReadOnly _LessThanOrEqualExpressionBuilder As BinaryOperatorExpressionBuilder = Function(e1, e2) Expressions.Expression.LessThanOrEqual(e1, e2)
    Public Shared ReadOnly LessThanOrEqual As New NamedBinaryOperator(
        "<=",
        {New BinaryOperatorOverload(NamedType.Real, NamedType.Real, resultType:=NamedType.Boolean, expressionBuilder:=_LessThanOrEqualExpressionBuilder)})

    Private Shared ReadOnly _GreaterThanExpressionBuilder As BinaryOperatorExpressionBuilder = Function(e1, e2) Expressions.Expression.GreaterThan(e1, e2)
    Public Shared ReadOnly GreaterThan As New NamedBinaryOperator(
        ">",
        {New BinaryOperatorOverload(NamedType.Real, NamedType.Real, resultType:=NamedType.Boolean, expressionBuilder:=_GreaterThanExpressionBuilder)})

    Private Shared ReadOnly _GreaterThanOrEqualExpressionBuilder As BinaryOperatorExpressionBuilder = Function(e1, e2) Expressions.Expression.GreaterThanOrEqual(e1, e2)
    Public Shared ReadOnly GreaterThanOrEqual As New NamedBinaryOperator(
        ">=",
        {New BinaryOperatorOverload(NamedType.Real, NamedType.Real, resultType:=NamedType.Boolean, expressionBuilder:=_GreaterThanOrEqualExpressionBuilder)})

    Private Shared ReadOnly _NegateFirstAndAddSecondExpressionBuilder As BinaryOperatorExpressionBuilder = Function(e1, e2) Expression.Add(Expression.Negate(e1), e2)
    Public Shared ReadOnly NegateFirstAndAddSecond As New NamedBinaryOperator(
        "+"c,
        {New BinaryOperatorOverload(NamedType.Real, NamedType.Real, NamedType.Real, _NegateFirstAndAddSecondExpressionBuilder),
         New BinaryOperatorOverload(NamedType.Vector3D, NamedType.Vector3D, NamedType.Vector3D, _NegateFirstAndAddSecondExpressionBuilder)})

    Private Shared ReadOnly _NegateFirstAndSubtractSecondExpressionBuilder As BinaryOperatorExpressionBuilder = Function(e1, e2) Expression.Subtract(Expression.Negate(e1), e2)
    Public Shared ReadOnly NegateFirstAndSubtractSecond As New NamedBinaryOperator(
        "-"c,
        {New BinaryOperatorOverload(NamedType.Real, NamedType.Real, NamedType.Real, _NegateFirstAndSubtractSecondExpressionBuilder),
         New BinaryOperatorOverload(NamedType.Vector3D, NamedType.Vector3D, NamedType.Vector3D, _NegateFirstAndSubtractSecondExpressionBuilder)})

End Class
