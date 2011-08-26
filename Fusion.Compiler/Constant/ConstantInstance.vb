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

    Friend Function ToExpressionWithNamedType() As ExpressionWithNamedType
        Return Me.Expression.WithNamedType(Me.Signature.Type)
    End Function

End Class

Public Class ConstantInstance(Of T)
    Inherits ConstantInstance

    Public Sub New(name As String, value As T, typeNamedTypeDictionary As TypeNamedTypeDictionary)
        MyBase.New(New ConstantSignature(name:=name, Type:=typeNamedTypeDictionary.GetNamedType(GetType(T))), value:=value)
    End Sub

End Class
