Public Class ConstExpression

    Private ReadOnly _Instance As ConstantDeclaration
    Public ReadOnly Property Instance As ConstantDeclaration
        Get
            Return _Instance
        End Get
    End Property

    Private ReadOnly _Expression As ConstantExpression
    Public ReadOnly Property Expression As ConstantExpression
        Get
            Return _Expression
        End Get
    End Property

    Public Sub New(instance As ConstantDeclaration, value As Object)
        _Instance = instance
        _Expression = Expressions.Expression.Constant(value:=value, type:=instance.Type.SystemType)
    End Sub

End Class
