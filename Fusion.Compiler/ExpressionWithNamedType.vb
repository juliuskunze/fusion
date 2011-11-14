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

    Public Sub New(expression As Expression, namedType As NamedType)
        If Not namedType.IsDelegate AndAlso Not namedType.TypeArguments.Any AndAlso Not namedType.SystemType.IsAssignableFrom(expression.Type) Then Throw New ArgumentException("Named type is not assignable from expression type.")
        _Expression = expression
        _NamedType = namedType
    End Sub

End Class