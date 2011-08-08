Public Class FunctionDeclaration

    Protected ReadOnly _Name As String
    Public ReadOnly Property Name As String
        Get
            Return _Name
        End Get
    End Property

    Private ReadOnly _FunctionType As FunctionType
    Public ReadOnly Property FunctionType As FunctionType
        Get
            Return _FunctionType
        End Get
    End Property

    Public Sub New(name As String, functionType As FunctionType)
        _Name = name
        _FunctionType = functionType
    End Sub

    Public Shared Function FromText(definition As String, types As NamedTypes) As FunctionDeclaration
        Dim rest As String = Nothing
        Dim typeAndName = CompilerTools.GetStartingTypedAndNamedVariable(definition:=definition, types:=types, out_rest:=rest)

        Dim parameters = CompilerTools.GetParameters(parametersInBrackets:=rest.Trim).Select(Function(parameterText) NamedParameter.FromText(definition:=parameterText, types:=types)).ToArray

        Return New FunctionDeclaration(Name:=typeAndName.Name, FunctionType:=New FunctionType(ResultType:=typeAndName.Type, parameters:=parameters))
    End Function

End Class
