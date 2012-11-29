Public Class FileFilter
    Private ReadOnly _FileFilter As String
    Private ReadOnly _Description As String

    Public Sub New(fileFilter As String, description As String)
        _FileFilter = fileFilter
        _Description = description
    End Sub

    Public ReadOnly Property FileFilter As String
        Get
            Return _FileFilter
        End Get
    End Property

    Public ReadOnly Property Description As String
        Get
            Return _Description
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return _Description & "|" & _FileFilter
    End Function
End Class
