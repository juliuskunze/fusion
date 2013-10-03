Public Class TypeInformation
    Private ReadOnly _Type As NamedType
    Public ReadOnly Property Type As NamedType
        Get
            If IsInfer Then Throw New InvalidOperationException("Type is unknown when IsInfer is True.")

            Return _Type
        End Get
    End Property

    Public Sub New(type As NamedType)
        If type Is Nothing Then Throw New ArgumentNullException("type")
        _Type = type
    End Sub

    Private Sub New()
        _Type = Nothing
    End Sub

    Private Shared ReadOnly _Infer As New TypeInformation
    Public Shared ReadOnly Property Infer As TypeInformation
        Get
            Return _Infer
        End Get
    End Property

    Public ReadOnly Property IsInfer As Boolean
        Get
            Return _Type Is Nothing
        End Get
    End Property

    Public Sub CheckIsAssignableFrom(type As NamedType)
        If Me.IsInfer Then Return

        Me.Type.CheckIsAssignableFrom(type)
    End Sub
End Class
