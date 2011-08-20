Public Class FunctionInstance

    Private ReadOnly _Name As String
    Public ReadOnly Property Name As String
        Get
            Return _Name
        End Get
    End Property

    Private ReadOnly _DelegateType As DelegateType
    Public ReadOnly Property DelegateType As DelegateType
        Get
            Return _DelegateType
        End Get
    End Property

    Private ReadOnly _ExpressionBuilder As FunctionCallExpressionBuilder
    Public ReadOnly Property ExpressionBuilder As FunctionCallExpressionBuilder
        Get
            Return _ExpressionBuilder
        End Get
    End Property

    Public ReadOnly Property Expression(arguments As IEnumerable(Of Expression)) As Expression
        Get
            Return _ExpressionBuilder(arguments)
        End Get
    End Property

    Private ReadOnly _InvokableExpression As Expression
    Public ReadOnly Property InvokableExpression As Expression
        Get
            Return _InvokableExpression
        End Get
    End Property

    Public Sub New(name As String, delegateType As DelegateType, invokableExpression As Expression)
        _Name = name
        _DelegateType = DelegateType
        _InvokableExpression = invokableExpression
        _ExpressionBuilder = GetDynamicFunctionCallExpressionBuilder(invokableExpression:=invokableExpression)
    End Sub

    Public Shared Function NewFromLambda(Of TDelegate)(name As String,
                                                       type As DelegateType,
                                                       lambdaExpression As Expressions.Expression(Of TDelegate)) As FunctionInstance
        Return New FunctionInstance(name:=name, DelegateType:=type, InvokableExpression:=lambdaExpression)
    End Function

    Friend Shared Function GetFunctionCallExpressionBuilder(Of TDelegate)(lambdaExpression As Expression(Of TDelegate)) As FunctionCallExpressionBuilder
        Return GetDynamicFunctionCallExpressionBuilder(InvokableExpression:=lambdaExpression)
    End Function

    Friend Shared Function GetDynamicFunctionCallExpressionBuilder(invokableExpression As Expression) As FunctionCallExpressionBuilder
        Return Function(arguments) Expressions.Expression.Invoke(expression:=InvokableExpression, arguments:=arguments)
    End Function

End Class

Public Delegate Function FunctionCallExpressionBuilder(arguments As IEnumerable(Of Expression)) As Expression