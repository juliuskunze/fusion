Public Class NamedTypes

    Private ReadOnly _Types As IEnumerable(Of NamedType)

    Public Sub New(types As IEnumerable(Of NamedType))
        _Types = types
    End Sub

    Public Function Parse(name As String) As NamedType
        Return _Types.Where(Function(type) type.Name = name).Single
    End Function

    Private Shared ReadOnly _DefaultTypes As NamedTypes = New NamedTypes(types:={NamedType.Real, NamedType.Vector3D})
    Public Shared ReadOnly Property DefaultTypes As NamedTypes
        Get
            Return _DefaultTypes
        End Get
    End Property

    Private Shared ReadOnly _Empty As New NamedTypes(types:={})
    Public Shared ReadOnly Property Empty As NamedTypes
        Get
            Return _Empty
        End Get
    End Property

End Class
