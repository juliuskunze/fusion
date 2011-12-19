Public Class ConstantSignature
    Implements ISignature

    Private ReadOnly _Name As String
    Public ReadOnly Property Name As String Implements ISignature.Name
        Get
            Return _Name
        End Get
    End Property

    Private ReadOnly _Description As String
    Public ReadOnly Property Description As String Implements ISignature.Description
        Get
            Return _Description
        End Get
    End Property

    Private ReadOnly _Type As NamedType
    Public ReadOnly Property Type As NamedType
        Get
            Return _Type
        End Get
    End Property

    Public Sub New(name As String, type As NamedType, Optional description As String = Nothing)
        _Name = name
        _Type = type
        _Description = description
    End Sub

    Shared Function FromText(text As LocatedString, typeContext As NamedTypes) As ConstantSignature
        Dim rest As LocatedString = Nothing
        Dim typeAndName = CompilerTools.GetStartingTypedAndNamedVariable(text:=text, types:=typeContext, out_rest:=rest)
        If rest.Trim.ToString <> "" Then Throw New LocatedCompilerException(rest.Trim, "End of constant definition expected.")

        Return New ConstantSignature(Name:=typeAndName.Name, Type:=typeAndName.Type)
    End Function

    Public Overrides Function ToString() As String Implements ISignature.GetSignatureString
        Return Me.Type.NameWithTypeArguments & " " & Me.Name
    End Function

    Public Sub CheckForConflicts(other As ConstantSignature)
        If CompilerTools.IdentifierEquals(Me.Name, other.Name) Then Throw New CompilerException(String.Format("Constant '{0}' is already defined.", other.Name))
    End Sub

    Public Function ToFunctionSignature() As FunctionSignature
        If Not Me.Type.IsFunctionType Then Throw New InvalidOperationException("Function type expected.")

        Return New FunctionSignature(Name:=Me.Name, FunctionType:=Me.Type.Function)
    End Function

End Class