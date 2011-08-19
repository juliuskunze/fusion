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
        If Not type.IsDelegateType Then
            _Expression = Expressions.Expression.Parameter(type:=type.SystemType, name:=name)
        End If
    End Sub

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

End Class
