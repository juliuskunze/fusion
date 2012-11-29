Public Class TypeDictionary
    Inherits Dictionary(Of Type, NamedType)

    Public Sub New(dictionary As IDictionary(Of Type, NamedType))
        MyBase.New(dictionary)
    End Sub

    Public Sub New(keyValuePairs As IEnumerable(Of KeyValuePair(Of Type, NamedType)))
        MyBase.New(GetDictionary(keyValuePairs))
    End Sub

    Private Shared Function GetDictionary(keyValuePairs As IEnumerable(Of KeyValuePair(Of Type, NamedType))) As Dictionary(Of Type, NamedType)
        Return keyValuePairs.ToDictionary(Function(keyValuePair) keyValuePair.Key, Function(keyValuePair) keyValuePair.Value)
    End Function

    Public Sub New(namedTypes As IEnumerable(Of NamedType))
        MyBase.New(namedTypes.ToDictionary(Of Type)(Function(namedType) namedType.SystemType))
    End Sub

    Public Function GetNamedType(type As Type) As NamedType
        Dim namedType As NamedType = Nothing
        If Not MyBase.TryGetValue(key:=type, value:=namedType) Then
            If type.IsGenericType Then
                Dim generic = type.GetGenericTypeDefinition

                If Not MyBase.TryGetValue(key:=generic, value:=namedType) Then ThrowTypeNotInDictionaryException(type)

                Return namedType.MakeGenericType(type.GetGenericArguments.Select(Function(subType) Me.GetNamedType(subType)))
            End If

            ThrowTypeNotInDictionaryException(type)
        End If

        Return namedType
    End Function

    Private Shared Sub ThrowTypeNotInDictionaryException(type As Type)
        Throw New InvalidOperationException(String.Format("Type '{0}' is not contained in the type dictionary.", type.Name))
    End Sub

    Private Shared ReadOnly _Default As New TypeDictionary(NamedTypes.Default)
    Public Shared ReadOnly Property [Default] As TypeDictionary
        Get
            Return _Default
        End Get
    End Property
End Class
