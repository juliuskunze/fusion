Public Class NamedFunctionExpression

    Private ReadOnly _Name As String
    Public ReadOnly Property Name As String
        Get
            Return _Name
        End Get
    End Property

    Private ReadOnly _ExpressionBuilder As ExpressionBuilder
    Public ReadOnly Property Expression(arguments As IEnumerable(Of Expression)) As Expression
        Get
            Return _ExpressionBuilder(arguments)
        End Get
    End Property

    Public Sub New(name As String, expressionBuilder As ExpressionBuilder)
        _Name = name
        _ExpressionBuilder = expressionBuilder
    End Sub

    Public Shared Function GetSystemMathFunctionExpressionBuilder(name As String) As ExpressionBuilder
        Return Function(arguments) Expressions.Expression.Call(method:=GetType(System.Math).GetMethod(name:=name), arguments:=arguments)
    End Function

    Public Shared Function GetFunctionExpressionBuilder(Of TDelegate)(userFunction As Expression(Of TDelegate)) As ExpressionBuilder
        Return GetFunctionExpressionBuilder(userFunctionExpression:=userFunction)
    End Function

    Private Shared Function GetFunctionExpressionBuilder(userFunctionExpression As Expression) As ExpressionBuilder
        Return Function(arguments) Expressions.Expression.Invoke(userFunctionExpression, arguments:=arguments)
    End Function

    Public Delegate Function ExpressionBuilder(arguments As IEnumerable(Of Expression)) As Expression

End Class