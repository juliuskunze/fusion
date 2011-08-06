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

    Public Sub New(name As String, lambdaExpression As LambdaExpression)
        Me.New(name:=name, ExpressionBuilder:=GetDynamicFunctionExpressionBuilder(lambdaExpression:=lambdaExpression))
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
                   Try
                       Return unsafeExpressionBuilder(arguments)
                   Catch ex As InvalidOperationException
                       Throw New ArgumentException("Wrong argument count.")
                   End Try
               End Function
    End Function


    Public Delegate Function ExpressionBuilder(arguments As IEnumerable(Of Expression)) As Expression

End Class