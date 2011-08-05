Public Class NamedExpression

    Private ReadOnly _Name As String
    Public ReadOnly Property Name As String
        Get
            Return _Name
        End Get
    End Property

    Private ReadOnly _ExpressionBuilder As ExpressionBuilder
    Public ReadOnly Property Expression(ByVal arguments As IEnumerable(Of Expression)) As Expression
        Get
            Return _ExpressionBuilder(arguments)
        End Get
    End Property

    Public Sub New(ByVal name As String, ByVal expressionBuilder As ExpressionBuilder)
        _Name = name
        _ExpressionBuilder = expressionBuilder
    End Sub

    Public Shared Function GetSystemMathFunctionExpressionBuilder(ByVal name As String) As ExpressionBuilder
        Return Function(arguments) Expressions.Expression.Call(method:=GetType(System.Math).GetMethod(name:=name), arguments:=arguments)
    End Function

    Public Shared Function GetFunctionExpressionBuilder(Of TDelegate)(ByVal userFunction As Expression(Of TDelegate)) As ExpressionBuilder
        Return GetFunctionExpressionBuilder(userFunctionExpression:=userFunction)
    End Function

    Private Shared Function GetFunctionExpressionBuilder(ByVal userFunctionExpression As Expression) As ExpressionBuilder
        Return Function(arguments) Expressions.Expression.Invoke(userFunctionExpression, arguments:=arguments)
    End Function

    Public Delegate Function ExpressionBuilder(ByVal arguments As IEnumerable(Of Expression)) As Expression

End Class