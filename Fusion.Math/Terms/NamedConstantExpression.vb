Public Class NamedConstantExpression
    Inherits NamedFunctionExpression

    Public ReadOnly Property ConstantExpression As ConstantExpression
        Get
            Return CType(MyBase.Expression(arguments:=New Expression() {}), ConstantExpression)
        End Get
    End Property

    Public Sub New(name As String, type As NamedType, value As Object)
        MyBase.New(name:=name, type:=type, Parameters:={}, ExpressionBuilder:=Function(arguments) Expressions.Expression.Constant(value:=value, type:=value.GetType))
    End Sub

End Class
