Public Class NamedParameter
    Private ReadOnly _Signature As ConstantSignature
    Public ReadOnly Property Signature As ConstantSignature
        Get
            Return _Signature
        End Get
    End Property

    Public Sub New(name As String, type As NamedType)
        Me.New(New ConstantSignature(name, type))
    End Sub

    Public Sub New(signature As ConstantSignature)
        _Signature = signature
        _Expression = Expressions.Expression.Parameter(type:=signature.Type.SystemType, name:=signature.Name)
    End Sub

    Public Shared Function FromText(text As LocatedString, typeContext As NamedTypes) As NamedParameter
        Return New NamedParameter(ConstantSignature.FromText(text:=text, typeContext:=typeContext))
    End Function

    Private ReadOnly _Expression As ParameterExpression
    Public ReadOnly Property Expression As ParameterExpression
        Get
            Return _Expression
        End Get
    End Property

    Public Function ToFunctionInstance() As FunctionInstance
        Return New FunctionInstance(Signature:=_Signature.ToFunctionSignature, invokableExpression:=_Expression)
    End Function

    Friend Function ToExpressionWithNamedType() As ExpressionWithNamedType
        Return Me.Expression.WithNamedType(Me.Signature.Type)
    End Function
End Class
