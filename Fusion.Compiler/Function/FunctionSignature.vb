Public Class FunctionSignature
    Implements ISignature

    Protected ReadOnly _Name As String
    Public ReadOnly Property Name As String Implements ISignature.Name
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

    Private ReadOnly _Description As String
    Public ReadOnly Property Description As String Implements ISignature.Description
        Get
            Return _Description
        End Get
    End Property

    Public Function AsNamedFunctionType() As NamedType
        Return New NamedType(Name:=_Name, [function]:=_FunctionType)
    End Function

    Public Sub New(name As String, functionType As FunctionType, Optional description As String = Nothing)
        _Name = name
        _FunctionType = functionType
        _Description = description
    End Sub

    Public Shared Function FromString(s As LocatedString, typeContext As NamedTypes) As FunctionSignature
        Dim rest As LocatedString = Nothing
        Dim typeAndName = CompilerTools.GetStartingTypedAndNamedVariable(text:=s, types:=typeContext, out_rest:=rest)

        Dim parameters = CompilerTools.GetParameters(parametersInBrackets:=rest.Trim).Select(Function(parameterText) NamedParameter.FromText(text:=parameterText, typeContext:=typeContext)).ToArray

        Return New FunctionSignature(Name:=typeAndName.Name, functionType:=New FunctionType(ResultType:=typeAndName.Type, parameters:=parameters))
    End Function

    Public Overrides Function ToString() As String Implements ISignature.GetSignatureString
        Return Me.FunctionType.ResultType.Name & " " & Me.Name & String.Join(", ", Me.FunctionType.Parameters.Select(Function(parameter) parameter.Signature.ToString)).InBrackets
    End Function

    Public Sub CheckForConflicts(other As FunctionSignature)
        If Me.Name = other.Name AndAlso Me.FunctionType.Parameters.Count = other.FunctionType.Parameters.Count Then Throw New CompilerException(String.Format("Function '{0}' with parameter count {1} is already defined.", other.Name, other.FunctionType.Parameters.Count))
    End Sub
End Class
