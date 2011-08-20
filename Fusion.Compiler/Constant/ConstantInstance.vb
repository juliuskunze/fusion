Public Class ConstantInstance

    Private ReadOnly _Signature As ConstantSignature
    Public ReadOnly Property Signature As ConstantSignature
        Get
            Return _Signature
        End Get
    End Property

    Private ReadOnly _Expression As ConstantExpression
    Public ReadOnly Property Expression As ConstantExpression
        Get
            Return _Expression
        End Get
    End Property

    Public Sub New(signature As ConstantSignature, value As Object)
        _Signature = signature
        _Expression = Expressions.Expression.Constant(value:=value, Type:=signature.Type.SystemType)
    End Sub

End Class
