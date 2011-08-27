Public Class NamedParameter

    Protected ReadOnly _Name As String
    Public ReadOnly Property Name As String
        Get
            Return _Name
        End Get
    End Property

    Protected ReadOnly _Type As NamedType
    Public ReadOnly Property Type As NamedType
        Get
            Return _Type
        End Get
    End Property

    Public Sub New(name As String, type As NamedType)
        _Name = name
        _Type = type
        _Expression = Expressions.Expression.Parameter(type:=type.SystemType, name:=name)
    End Sub

    Public Function ToFunctionInstance() As FunctionInstance
        If Not _Type.IsDelegate Then Throw New CompilerException("Parameter must be a delegate.")

        Return New FunctionInstance(Name:=_Name, DelegateType:=_Type.Delegate, invokableExpression:=_Expression)
    End Function

    Public Shared Function FromText(text As String, typeContext As NamedTypes) As NamedParameter
        Dim signature = ConstantSignature.FromText(text:=text, typeContext:=typeContext)
        Return New NamedParameter(Name:=signature.Name, Type:=signature.Type)
    End Function

    Private ReadOnly _Expression As ParameterExpression
    Public ReadOnly Property Expression As ParameterExpression
        Get
            Return _Expression
        End Get
    End Property

    Friend Function ToExpressionWithNamedType() As ExpressionWithNamedType
        Return Me.Expression.WithNamedType(Me.Type)
    End Function

End Class
