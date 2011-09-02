Public Class ExpressionWithNamedType

    Private ReadOnly _Expression As Expression
    Public ReadOnly Property Expression As Expression
        Get
            Return _Expression
        End Get
    End Property

    Private ReadOnly _NamedType As NamedType
    Public ReadOnly Property NamedType As NamedType
        Get
            Return _NamedType
        End Get
    End Property

    Public Sub New(expression As Expression, expressionType As NamedType)
        If Not expressionType.IsDelegate AndAlso Not expressionType.TypeArguments.Any AndAlso expression.Type IsNot expressionType.SystemType Then Throw New CompilerException("Expression type does not match with named type.")
        _Expression = expression
        _NamedType = NamedType
    End Sub

End Class