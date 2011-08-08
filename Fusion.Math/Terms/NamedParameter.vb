Public Class NamedParameter
    Inherits NamedAndTypedObject

    Public Sub New(name As String, type As NamedType)
        MyBase.New(name:=name, type:=type)
        _ParameterExpression = Expression.Parameter(type:=type.Type, name:=name)
    End Sub

    Private ReadOnly _ParameterExpression As ParameterExpression
    Public ReadOnly Property ParameterExpression As ParameterExpression
        Get
            Return _ParameterExpression
        End Get
    End Property

End Class
