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

    Private ReadOnly _IsConstant As Boolean
    Public ReadOnly Property IsConstant As Boolean
        Get
            Return _IsConstant
        End Get
    End Property

    Public Sub New(name As String, type As NamedType)
        _Name = name
        _Type = type
        _IsConstant = Not TypeOf type Is NamedType
        If _IsConstant Then
            _Expression = Expressions.Expression.Parameter(type:=type.SystemType, name:=name)
        End If
    End Sub

    Public Shared Function FromText(definition As String, types As NamedTypes) As NamedParameter
        If Signature.IsConstantSignatureDefinition(definition:=definition) Then
            Dim constantI = ConstantDeclaration.FromText(definition:=definition, types:=types)
            Return New NamedParameter(Name:=constantI.Name, Type:=constantI.Type)
        Else
            Dim functionI = FunctionDeclaration.FromText(definition:=definition, types:=types)
            Return New NamedParameter(Name:=functionI.Name, Type:=functionI.FunctionType)
        End If
    End Function

    Private ReadOnly _Expression As ParameterExpression
    Public ReadOnly Property Expression As ParameterExpression
        Get
            Return _Expression
        End Get
    End Property

End Class
