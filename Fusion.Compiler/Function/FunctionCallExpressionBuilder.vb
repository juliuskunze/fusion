Public Class FunctionCallExpressionBuilder

    Private ReadOnly _ExpressionBuilder As Func(Of IEnumerable(Of Expression), Expression)

    Public Sub New(invokableExpression As Expression)
        _ExpressionBuilder = Function(arguments) Expressions.Expression.Invoke(expression:=invokableExpression, arguments:=arguments)
    End Sub

    Public Function Run(arguments As IEnumerable(Of Expression)) As Expression
        Return _ExpressionBuilder(arguments)
    End Function

End Class

Public Class FunctionCallExpressionBuilder(Of TDelegate)
    Inherits FunctionCallExpressionBuilder

    Public Sub New(lambdaExpression As Expression(Of TDelegate))
        MyBase.New(invokableExpression:=lambdaExpression)
    End Sub

End Class