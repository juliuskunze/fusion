Public Class NamedConstantExpression
    Inherits NamedExpression

    Public ReadOnly Property ConstantExpression As ConstantExpression
        Get
            Return CType(MyBase.Expression(arguments:=New Expression() {}), ConstantExpression)
        End Get
    End Property

    Public Sub New(ByVal name As String, ByVal value As Double)
        MyBase.New(name:=name, ExpressionBuilder:=Function(arguments) Expressions.Expression.Constant(value:=value, type:=GetType(Double)))
    End Sub

End Class
