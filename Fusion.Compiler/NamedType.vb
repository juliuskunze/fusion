Public Class NamedType

    Private ReadOnly _Name As String
    Public ReadOnly Property Name As String
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

    Public Sub New(name As String, systemType As System.Type)
        Me.New(name:=name, systemType:=systemType, TypeArguments:={})
    End Sub

    Private Sub New(name As String, systemType As System.Type, typeArguments As IEnumerable(Of NamedType))
        _IsDelegate = False
        _Name = name
        _SystemType = systemType
        _TypeArguments = typeArguments
    End Sub

    Public Function MakeGenericType(typeArguments As IEnumerable(Of NamedType)) As NamedType
        If _TypeArguments.Any Then Throw New InvalidOperationException("Only types that have not already generic arguments can get new type arguments.")
        If _SystemType.GetGenericArguments.Count <> typeArguments.Count Then Throw New ArgumentException(String.Format("Wrong type argument count for type '{0}'.", _Name))

        Return New NamedType(_Name, _SystemType.MakeGenericType((From namedType In typeArguments Select namedType.SystemType).ToArray), typeArguments)
    End Function

    Public Sub New(name As String, [delegate] As DelegateType)
        _IsDelegate = True
        _Name = name
        _Delegate = [delegate]
    End Sub

    Public Sub CheckIsAssignableFrom(other As NamedType)
        If _IsDelegate Then
            If Not other.IsDelegate Then Me.ThrowNotAssignableFromException(other.Name)

            Me.Delegate.CheckIsAssignableFrom(other.Delegate)
        Else
            If Not Me.SystemType.IsAssignableFrom(other.SystemType) Then Me.ThrowNotAssignableFromException(other.Name)
        End If
    End Sub

    Private Const _KeyWord = "delegate"

    Public Shared Function NamedDelegateTypeFromText(text As String, typeContext As NamedTypes) As NamedType
        Dim trimmed = text.Trim
        If Not trimmed.StartsWith(_KeyWord, StringComparison.OrdinalIgnoreCase) Then Throw New ArgumentException("text", "Invalid delegate declaration.")
        Dim rest = trimmed.Substring(startIndex:=_KeyWord.Count)
        Dim signature = FunctionSignature.FromText(text:=rest, typeContext:=typeContext)

        Return signature.AsNamedDelegateType
    End Function

    Private Function ThrowNotAssignableFromException(otherName As String) As ArgumentException
        Throw New ArgumentException(String.Format("Type '{0}' is not assignable to type '{1}'.", otherName, Me.Name))
    End Function

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

    Private Shared ReadOnly _Collection As New NamedType("Collection", GetType(IEnumerable(Of )))
    Public Shared ReadOnly Property Collection() As NamedType
        Get
            Return _Collection
        End Get
    End Property

End Class
