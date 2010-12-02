Public Class StringInitializer
    Implements IInitializer(Of String)

    Public Function Initialize() As String Implements IInitializer(Of String).Initialize
        Return "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
    End Function
End Class
