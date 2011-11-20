﻿Public Class NamedType
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
            If Me.IsDelegate Then
                Return _Delegate.SystemType
            Else
                Return _SystemType
            End If
        End Get
    End Property

    Private ReadOnly _Delegate As DelegateType
    Public ReadOnly Property [Delegate] As DelegateType
        Get
            If Not _IsDelegate Then Throw New InvalidOperationException("The type must be a delegate type.")

            Return _Delegate
        End Get
    End Property

    Private ReadOnly _IsDelegate As Boolean
    Public ReadOnly Property IsDelegate As Boolean
        Get
            Return _IsDelegate
        End Get
    End Property

    Private ReadOnly _TypeArguments As IEnumerable(Of NamedType)
    Public ReadOnly Property TypeArguments As IEnumerable(Of NamedType)
        Get
            Return _TypeArguments
        End Get
    End Property

    Public Sub New(name As String, systemType As System.Type, Optional description As String = Nothing)
        Me.New(name:=name, systemType:=systemType, TypeArguments:={}, description:=description)
    End Sub

    Private Sub New(name As String, systemType As System.Type, typeArguments As IEnumerable(Of NamedType), Optional description As String = Nothing)
        If typeArguments Is Nothing Then Throw New ArgumentNullException("typeArguments")

        _IsDelegate = False
        _Name = name
        _SystemType = systemType
        _TypeArguments = typeArguments
        _Description = description
    End Sub

    Public Sub New(name As String, [delegate] As DelegateType, Optional description As String = Nothing)
        _IsDelegate = True
        _Name = name
        _Delegate = [delegate]
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
        If _IsDelegate Then
            If Not other.IsDelegate Then Return False

            If Not Me.Delegate.IsAssignableFrom(other.Delegate) Then Return False
        Else
            If Not Me.SystemType.IsAssignableFrom(other.SystemType) Then Return False
        End If

        Return True
    End Function

    Public Shared Function NamedDelegateTypeFromString(s As LocatedString, typeContext As NamedTypes) As NamedType
        Dim trimmed = s.Trim
        If Not trimmed.ToString.StartsWith(Keywords.Delegate, StringComparison.OrdinalIgnoreCase) Then Throw New LocatedCompilerException(s, "Invalid delegate declaration.")
        Dim rest = trimmed.Substring(startIndex:=Keywords.Delegate.Count)
        Dim signature = FunctionSignature.FromString(s:=rest, typeContext:=typeContext)

        Return signature.AsNamedDelegateType
    End Function

    Private Function ThrowNotAssignableFromException(otherName As String) As CompilerException
        Throw New CompilerException(String.Format("Type '{0}' is not assignable to type '{1}'.", otherName, Me.Name))
    End Function

    Private Shared ReadOnly _Boolean As New NamedType("Boolean", GetType(Boolean))
    Public Shared ReadOnly Property [Boolean] As NamedType
        Get
            Return _Boolean
        End Get
    End Property

    Private Shared ReadOnly _Real As New NamedType("Real", GetType(Double))
    Public Shared ReadOnly Property Real() As NamedType
        Get
            Return _Real
        End Get
    End Property

    Private Shared ReadOnly _Vector3D As New NamedType("Vector", GetType(Vector3D))
    Public Shared ReadOnly Property Vector3D() As NamedType
        Get
            Return _Vector3D
        End Get
    End Property

    Private Shared ReadOnly _Set As New NamedType("Set", GetType(IEnumerable(Of )))
    Public Shared ReadOnly Property [Set]() As NamedType
        Get
            Return _Set
        End Get
    End Property

    Public Function GetSignatureString() As String Implements ISignature.GetSignatureString
        If Me.IsDelegate Then
            Return Me.Delegate.ResultType.GetSignatureString & " " & Me.Name & String.Join(", ", Me.Delegate.Parameters.Select(Function(parameter) parameter.Signature.ToString)).InBrackets(CompilerTools.ParameterBracketType)
        Else
            Return "Type " & Me.NameWithTypeArguments
        End If
    End Function

    Public ReadOnly Property NameWithTypeArguments As String
        Get
            Return If(Me.IsDelegate,
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
