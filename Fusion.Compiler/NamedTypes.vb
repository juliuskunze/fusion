Public Class NamedTypes
    Inherits List(Of NamedType)

    Public Sub New(types As IEnumerable(Of NamedType))
        MyBase.New(types)
    End Sub

    Public Function Parse(name As LocatedString) As NamedType
        Dim matchingTypes = Where(Function(type) IdentifierEquals(type.Name, name.ToString)).ToArray

        If Not matchingTypes.Any Then Throw New LocatedCompilerException(name, message:=String.Format("Type '{0}' is not defined in this context.", name))

        Return matchingTypes.Single
    End Function

    Private Shared ReadOnly _Default As NamedTypes = New NamedTypes(types:={NamedType.Boolean, NamedType.Real, NamedType.Vector3D, NamedType.Set})
    Public Shared ReadOnly Property [Default] As NamedTypes
        Get
            Return _Default
        End Get
    End Property

    Private Shared ReadOnly _Empty As New NamedTypes(types:={})
    Public Shared ReadOnly Property Empty As NamedTypes
        Get
            Return _Empty
        End Get
    End Property

    Public Function Merge(second As NamedTypes) As NamedTypes
        For Each newType In second
            For Each type In Me
                newType.CheckForSignatureConflicts(type)
            Next
        Next

        Return New NamedTypes(Concat(second))
    End Function
End Class
