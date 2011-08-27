Public Class ConstantSignature

    Private ReadOnly _Name As String
    Public ReadOnly Property Name As String
        Get
            Return _Name
        End Get
    End Property

    Private ReadOnly _Type As NamedType
    Public ReadOnly Property Type As NamedType
        Get
            Return _Type
        End Get
    End Property

    Public Sub New(name As String, type As NamedType)
        _Name = name
        _Type = type
    End Sub

    Shared Function FromText(text As String, typeContext As NamedTypes) As ConstantSignature
        Dim rest As String = Nothing
        Dim typeAndName = CompilerTools.GetStartingTypedAndNamedVariable(text:=text, types:=typeContext, out_rest:=rest)
        If rest.Trim <> "" Then Throw New CompilerException("End of constant definition expected.")

        Return New ConstantSignature(Name:=typeAndName.Name, Type:=typeAndName.Type)
    End Function

End Class