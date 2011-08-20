Public Class FunctionSignature

    Protected ReadOnly _Name As String
    Public ReadOnly Property Name As String
        Get
            Return _Name
        End Get
    End Property

    Private ReadOnly _DelegateType As DelegateType
    Public ReadOnly Property DelegateType As DelegateType
        Get
            Return _DelegateType
        End Get
    End Property

    Public Function AsNamedDelegateType() As NamedType
        Return New NamedType(Name:=_Name, [delegate]:=_DelegateType)
    End Function

    Public Sub New(name As String, delegateType As DelegateType)
        _Name = name
        _DelegateType = delegateType
    End Sub

    Public Shared Function FromText(text As String, typeContext As NamedTypes) As FunctionSignature
        Dim rest As String = Nothing
        Dim typeAndName = CompilerTools.GetStartingTypedAndNamedVariable(text:=text, types:=typeContext, out_rest:=rest)

        Dim parameters = CompilerTools.GetParameters(parametersInBrackets:=rest.Trim).Select(Function(parameterText) NamedParameter.FromText(text:=parameterText, typeContext:=typeContext)).ToArray

        Return New FunctionSignature(Name:=typeAndName.Name, DelegateType:=New DelegateType(ResultType:=typeAndName.Type, parameters:=parameters))
    End Function

End Class
