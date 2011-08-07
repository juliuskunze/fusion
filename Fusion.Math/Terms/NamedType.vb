Public Class NamedType

    Private ReadOnly _Name As String
    Public ReadOnly Property Name As String
        Get
            Return _Name
        End Get
    End Property

    Private ReadOnly _Type As Type
    Public ReadOnly Property Type As Type
        Get
            Return _Type
        End Get
    End Property

    Public Sub New(name As String, type As System.Type)
        _Name = name
        _Type = type
    End Sub

    Public Shared ReadOnly Property Real() As NamedType
        Get
            Return New NamedType("Real", GetType(Double))
        End Get
    End Property

    Public Shared ReadOnly Property Vector3D() As NamedType
        Get
            Return New NamedType("Vector", GetType(Vector3D))
        End Get
    End Property

    Public Shared ReadOnly Property DefaultTypes As IEnumerable(Of NamedType)
        Get
            Return {Real, Vector3D}
        End Get
    End Property

End Class
