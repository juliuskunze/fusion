Public Class NamedType
    Implements ISignature

    Private ReadOnly _Description As String
    Public ReadOnly Property Description As String Implements ISignature.Description
        Get
            Return _Description
        End Get
    End Property

    Private ReadOnly _Name As String
    Public ReadOnly Property Name As String Implements ISignature.Name
        Get
            Return _Name
        End Get
    End Property

    Private ReadOnly _SystemType As Type
    Public ReadOnly Property SystemType As Type
        Get
            If IsFunctionType Then
                Return _Function.SystemType
            Else
                Return _SystemType
            End If
        End Get
    End Property

    Private ReadOnly _Function As FunctionType
    Public ReadOnly Property [Function] As FunctionType
        Get
            If Not _IsFunctionType Then Throw New InvalidOperationException("The type must be a function type.")

            Return _Function
        End Get
    End Property

    Private ReadOnly _IsFunctionType As Boolean
    Public ReadOnly Property IsFunctionType As Boolean
        Get
            Return _IsFunctionType
        End Get
    End Property

    Private ReadOnly _TypeArguments As IEnumerable(Of NamedType)
    Public ReadOnly Property TypeArguments As IEnumerable(Of NamedType)
        Get
            Return _TypeArguments
        End Get
    End Property

    Public Sub New(name As String, systemType As Type, Optional description As String = Nothing)
        Me.New(name:=name, systemType:=systemType, TypeArguments:={}, description:=description)
    End Sub

    Private Sub New(name As String, systemType As Type, typeArguments As IEnumerable(Of NamedType), Optional description As String = Nothing)
        If typeArguments Is Nothing Then Throw New ArgumentNullException("typeArguments")

        _IsFunctionType = False
        _Name = name
        _SystemType = systemType
        _TypeArguments = typeArguments
        _Description = description
    End Sub

    Public Sub New(name As String, [function] As FunctionType, Optional description As String = Nothing)
        _IsFunctionType = True
        _Name = name
        _Function = [function]
        _Description = description
    End Sub

    Public Function MakeGenericType(typeArguments As IEnumerable(Of NamedType)) As NamedType
        If _TypeArguments.Any Then Throw New CompilerException("Only types that have not already generic arguments can get new type arguments.")
        If _SystemType.GetGenericArguments.Count <> typeArguments.Count Then Throw New CompilerException(String.Format("Wrong type argument count for type '{0}'.", _Name))

        Return New NamedType(_Name, _SystemType.MakeGenericType((From namedType In typeArguments Select namedType.SystemType).ToArray), typeArguments)
    End Function

    Public Sub CheckIsAssignableFrom(other As NamedType)
        If Not Me.IsAssignableFrom(other) Then Me.ThrowNotAssignableFromException(other.Name)
    End Sub

    Public Function IsAssignableFrom(other As NamedType) As Boolean
        If _IsFunctionType Then
            If Not other.IsFunctionType Then Return False

            If Not Me.[Function].IsAssignableFrom(other.[Function]) Then Return False
        Else
            If Not Me.SystemType.IsAssignableFrom(other.SystemType) Then Return False
        End If

        Return True
    End Function

    Public Shared Function NamedFunctionTypeFromString(s As LocatedString, typeContext As NamedTypes) As NamedType
        Dim trimmed = s.Trim
        If Not trimmed.ToString.StartsWith(Keywords.FunctionType, StringComparison.OrdinalIgnoreCase) Then Throw New LocatedCompilerException(s, "Invalid function type definition.")
        Dim rest = trimmed.Substring(startIndex:=Keywords.FunctionType.Count)
        Dim signature = FunctionSignature.FromString(s:=rest, typeContext:=typeContext)

        Return signature.AsNamedFunctionType
    End Function

    Private Sub ThrowNotAssignableFromException(otherName As String)
        Throw New CompilerException(String.Format("Type '{0}' is not assignable to type '{1}'.", otherName, Name))
    End Sub

    Private Shared ReadOnly _Boolean As New NamedType("Boolean", GetType(Boolean))
    Public Shared ReadOnly Property [Boolean] As NamedType
        Get
            Return _Boolean
        End Get
    End Property

    Private Shared ReadOnly _Real As New NamedType("Real", GetType(Double), "Represents a real number.")
    Public Shared ReadOnly Property Real() As NamedType
        Get
            Return _Real
        End Get
    End Property

    Private Shared ReadOnly _Vector3D As New NamedType("Vector", GetType(Vector3D), "Represents a 3D vector.")
    Public Shared ReadOnly Property Vector3D() As NamedType
        Get
            Return _Vector3D
        End Get
    End Property

    Private Shared ReadOnly _Set As New NamedType("Set", GetType(IEnumerable(Of )), "Represents a set of elements of a specified type.")
    Public Shared ReadOnly Property [Set]() As NamedType
        Get
            Return _Set
        End Get
    End Property

    Public Function GetSignatureString() As String Implements ISignature.GetSignatureString
        If IsFunctionType Then
            Return [Function].ResultType.GetSignatureString & " " & Name & String.Join(", ", [Function].Parameters.Select(Function(parameter) parameter.Signature.ToString)).InBrackets(CompilerTools.ParameterBracketType)
        Else
            Return "Type " & NameWithTypeArguments
        End If
    End Function

    Public ReadOnly Property NameWithTypeArguments As String
        Get
            Return If(Me.IsFunctionType,
                      Me.Name,
                      Me.Name & If(Me.TypeArguments.Any,
                                   String.Join(", ", Me.TypeArguments.Select(Function(typeArgument) typeArgument.NameWithTypeArguments)).InBrackets(CompilerTools.TypeArgumentBracketType),
                                   ""
                                   )
                      )
        End Get
    End Property

    Public Sub CheckForSignatureConflicts(other As NamedType)
        If CompilerTools.IdentifierEquals(Me.Name, other.Name) Then Throw New CompilerException(String.Format("Type '{0}' is already defined.", other.Name))
    End Sub
End Class
