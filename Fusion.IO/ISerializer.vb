Public Interface ISerializer(Of T)
    Sub Serialize(objectToTranslate As T, filepath As String)
    Function Deserialize(filepath As String) As T
End Interface



