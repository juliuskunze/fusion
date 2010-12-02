Public Class ParticleSystem2DSerializer
    Implements ISerializer(Of ParticleSystem2D)

    Public Sub Serialize(ByVal particleSystem As ParticleSystem2D, ByVal filepath As String) Implements ISerializer(Of Physics.ParticleSystem2D).Serialize
        Dim formatter = New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
        Using stream = New System.IO.FileStream(filepath, System.IO.FileMode.OpenOrCreate)
            formatter.Serialize(stream, particleSystem)
        End Using
    End Sub

    Public Function Deserialize(ByVal filename As String) As ParticleSystem2D Implements ISerializer(Of Physics.ParticleSystem2D).Deserialize
        Dim formatter = New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
        Using stream = New System.IO.FileStream(filename, System.IO.FileMode.Open)
            Return DirectCast(formatter.Deserialize(stream), ParticleSystem2D)
        End Using
    End Function

End Class
