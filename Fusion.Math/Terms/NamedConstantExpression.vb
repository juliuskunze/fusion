Public Class NamedConstantExpression
    Inherits NamedFUnctionExpression

    Public ReadOnly Property ConstantExpression As ConstantExpression
        Get
            Return CType(MyBase.Expression(arguments:=New Expression() {}), ConstantExpression)
        End Get
    End Property

    Public Sub New(name As String, value As Double)
        MyBase.New(name:=name, ExpressionBuilder:=Function(arguments) Expressions.Expression.Constant(value:=value, type:=GetType(Double)))
    End Sub

End Class
