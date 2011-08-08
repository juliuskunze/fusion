Public Class NamedConstantExpression
    Inherits NamedAndTypedObject

    Private ReadOnly _ConstantExpression As ConstantExpression
    Public ReadOnly Property ConstantExpression As ConstantExpression
        Get
            Return _ConstantExpression
        End Get
    End Property

    Public Sub New(name As String, type As NamedType, value As Object)
        MyBase.New(name:=name, type:=type)
        _ConstantExpression = Expressions.Expression.Constant(value:=value, type:=type.SystemType)
    End Sub

End Class
