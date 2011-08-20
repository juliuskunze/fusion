Public Class TypeNamedTypeDictionary
    Inherits Dictionary(Of Type, NamedType)

    Public Sub New(namedTypes As IEnumerable(Of NamedType))
        For Each namedType In namedTypes
            Me.Add(namedType.SystemType, namedType)
        Next
    End Sub

    Public Function GetNamedType(type As Type) As NamedType
        Dim namedType As NamedType = Nothing
        If Not MyBase.TryGetValue(key:=type, value:=namedType) Then ThrowTypeNotInDictionaryException(type)

        Return namedType
    End Function
   

    Private Shared Sub ThrowTypeNotInDictionaryException(type As Type)
        Throw New InvalidOperationException("'" & type.Name & "' is not contained in type typeNamedTypeDictionary.")
    End Sub

End Class
