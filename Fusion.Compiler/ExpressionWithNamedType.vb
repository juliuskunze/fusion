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
        If Not namedType.IsFunctionType AndAlso Not namedType.TypeArguments.Any AndAlso Not namedType.SystemType.IsAssignableFrom(expression.Type) Then Throw New ArgumentException(String.Format("Named type '{0}' is not assignable from expression type '{1}'.", namedType.Name, expression.Type))
        _Expression = expression
        _NamedType = namedType
    End Sub

End Class