Public Class BinaryOperatorOverload

    Private ReadOnly _ArgumentType1 As NamedType

    Public ReadOnly Property ArgumentType1 As NamedType
        Get
            Return _ArgumentType1
        End Get
    End Property

    Private ReadOnly _ArgumentType2 As NamedType
    Public ReadOnly Property ArgumentType2 As NamedType
        Get
            Return _ArgumentType2
        End Get
    End Property

    Private ReadOnly _ArgumentTypesInformation As BinaryOperatorArgumentTypesInformation
    Public ReadOnly Property ArgumentTypesInformation As BinaryOperatorArgumentTypesInformation
        Get
            Return _ArgumentTypesInformation
        End Get
    End Property

    Private ReadOnly _ResultType As NamedType
    Public ReadOnly Property ResultType As NamedType
        Get
            Return _ResultType
        End Get
    End Property

    Private ReadOnly _ExpressionBuilder As BinaryOperatorExpressionBuilder
    Public ReadOnly Property ExpressionBuilder As BinaryOperatorExpressionBuilder
        Get
            Return _ExpressionBuilder
        End Get
    End Property

    Public Function GetExpressionWithNamedType(e1 As Expression, e2 As Expression) As ExpressionWithNamedType
        Return _ExpressionBuilder(e1, e2).WithNamedType(_ResultType)
    End Function

    Public Sub New(argumentType1 As NamedType, argumentType2 As NamedType, resultType As NamedType, expressionBuilder As BinaryOperatorExpressionBuilder)
        _ArgumentType1 = argumentType1
        _ArgumentType2 = argumentType2
        _ArgumentTypesInformation = New BinaryOperatorArgumentTypesInformation(New TypeInformation(_ArgumentType1), New TypeInformation(_ArgumentType2))
        _ResultType = resultType
        _ExpressionBuilder = expressionBuilder
    End Sub

End Class

Public Delegate Function BinaryOperatorExpressionBuilder(e1 As Expression, e2 As Expression) As Expression