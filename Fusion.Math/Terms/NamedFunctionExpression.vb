Public Class NamedFunctionExpression

    Private ReadOnly _DelegateType As NamedDelegateType
    Public ReadOnly Property DelegateType As NamedDelegateType
        Get
            Return _DelegateType
        End Get
    End Property

    Private ReadOnly _ExpressionBuilder As ExpressionBuilder
    Public ReadOnly Property Expression(arguments As IEnumerable(Of Expression)) As Expression
        Get
            Return _ExpressionBuilder(arguments)
        End Get
    End Property

    Public Sub New(delegateType As NamedDelegateType, expressionBuilder As ExpressionBuilder)
        _DelegateType = delegateType
        _ExpressionBuilder = expressionBuilder
    End Sub

    Public Sub New(DelegateType As NamedDelegateType, lambdaExpression As LambdaExpression)
        Me.New(delegateType:=DelegateType, ExpressionBuilder:=GetDynamicFunctionExpressionBuilder(lambdaExpression:=lambdaExpression))
    End Sub

    Public Shared Function GetSystemMathFunctionExpressionBuilder(name As String) As ExpressionBuilder
        Return GetSafeExpressionBuilder(unsafeExpressionBuilder:=Function(arguments) Expressions.Expression.Call(method:=GetType(System.Math).GetMethod(name:=name), arguments:=arguments))
    End Function

    Public Shared Function GetFunctionExpressionBuilder(Of TDelegate)(lambdaExpression As Expression(Of TDelegate)) As ExpressionBuilder
        Return GetDynamicFunctionExpressionBuilder(lambdaExpression:=lambdaExpression)
    End Function

    Private Shared Function GetDynamicFunctionExpressionBuilder(lambdaExpression As Expression) As ExpressionBuilder
        Return GetSafeExpressionBuilder(unsafeExpressionBuilder:=Function(arguments) Expressions.Expression.Invoke(expression:=lambdaExpression, arguments:=arguments))
    End Function

    Private Shared Function GetSafeExpressionBuilder(unsafeExpressionBuilder As ExpressionBuilder) As ExpressionBuilder
        Return Function(arguments)
                   Return unsafeExpressionBuilder(arguments)
               End Function
    End Function


    Public Delegate Function ExpressionBuilder(arguments As IEnumerable(Of Expression)) As Expression

End Class