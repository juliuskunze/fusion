Public Class NamedParameter
    Inherits NamedAndTypedObject

    Public Sub New(name As String, type As NamedType)
        MyBase.New(name:=name, type:=type)
        _ParameterExpression = Expression.Parameter(type:=type.SystemType, name:=name)
    End Sub

    Private ReadOnly _ParameterExpression As ParameterExpression
    Public ReadOnly Property ParameterExpression As ParameterExpression
        Get
            Return _ParameterExpression
        End Get
    End Property

    Public ReadOnly Property Definition As String
        Get
            Return _Type.Name & " " & _Name
        End Get
    End Property

End Class
