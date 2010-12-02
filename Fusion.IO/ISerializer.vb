Public Interface ISerializer(Of T)
    Sub Serialize(ByVal objectToTranslate As T, ByVal filepath As String)
    Function Deserialize(ByVal filepath As String) As T
End Interface



