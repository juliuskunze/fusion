Public Class NamedFunctionExpression
    Inherits NamedAndTypedObject

    Private ReadOnly _Parameters As IEnumerable(Of NamedParameter)
    Public ReadOnly Property Parameters As IEnumerable(Of NamedParameter)
        Get
            Return _Parameters
        End Get
    End Property

    Private ReadOnly _ExpressionBuilder As ExpressionBuilder
    Public ReadOnly Property Expression(arguments As IEnumerable(Of Expression)) As Expression
        Get
            Return _ExpressionBuilder(arguments)
        End Get
    End Property

    Public Sub New(name As String, type As NamedType, parameters As IEnumerable(Of NamedParameter), expressionBuilder As ExpressionBuilder)
        MyBase.New(name:=name, type:=type)
        _Parameters = parameters
        _ExpressionBuilder = expressionBuilder
    End Sub

    Public Sub New(name As String, type As NamedType, parameters As IEnumerable(Of NamedParameter), lambdaExpression As LambdaExpression)
        Me.New(name:=name, type:=type, parameters:=parameters, ExpressionBuilder:=GetDynamicFunctionExpressionBuilder(lambdaExpression:=lambdaExpression))
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