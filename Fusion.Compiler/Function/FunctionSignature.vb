Public Class FunctionSignature
    Implements ISignature

    Protected ReadOnly _Name As String
    Public ReadOnly Property Name As String Implements ISignature.Name
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

    Public Shared Function FromString(s As LocatedString, typeContext As NamedTypes) As FunctionSignature
        Dim rest As LocatedString = Nothing
        Dim typeAndName = CompilerTools.GetStartingTypedAndNamedVariable(text:=s, types:=typeContext, out_rest:=rest)

        Dim parameters = CompilerTools.GetParameters(parametersInBrackets:=rest.Trim).Select(Function(parameterText) NamedParameter.FromText(text:=parameterText, typeContext:=typeContext)).ToArray

        Return New FunctionSignature(Name:=typeAndName.Name, DelegateType:=New DelegateType(ResultType:=typeAndName.Type, parameters:=parameters))
    End Function

    Public Overrides Function ToString() As String Implements ISignature.GetSignatureString
        Return Me.DelegateType.ResultType.Name & " " & Me.Name & String.Join(", ", Me.DelegateType.Parameters.Select(Function(parameter) parameter.Signature.ToString)).InBrackets
    End Function

    Public Sub CheckForConflicts(other As FunctionSignature)
        If Me.Name = other.Name AndAlso Me.DelegateType.Parameters.Count = other.DelegateType.Parameters.Count Then Throw New CompilerException(String.Format("Function '{0}' with parameter count {1} is already defined.", other.Name, other.DelegateType.Parameters.Count))
    End Sub

End Class
