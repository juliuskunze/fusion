Public Class ConstantSignatureDefinition
    Inherits NamedAndTypedObject

    Public Sub New(name As String, type As NamedType)
        MyBase.New(name, type)
    End Sub

    Public Shared Function FromText(definition As String, types As NamedTypes) As ConstantSignatureDefinition
        Dim typeName = CompilerTools.GetStartingValidVariableName(definition.TrimStart)
        Dim type = types.Parse(typeName)

        Dim rest = definition.Substring(startIndex:=typeName.Length)
        Dim constantName = CompilerTools.GetStartingValidVariableName(rest.TrimStart)
        'If Not constantName.IsValidVariableName Then Throw New ArgumentException("""" & _Left & """ is not a valid constant name.")

        Return New ConstantSignatureDefinition(Name:=constantName, type:=type)
    End Function

End Class
