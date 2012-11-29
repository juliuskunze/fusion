Public Class FileFilters
    Private ReadOnly _FileFilters As IEnumerable(Of FileFilter)
    Public ReadOnly Property FileFilters As IEnumerable(Of FileFilter)
        Get
            Return _FileFilters
        End Get
    End Property

    Public Sub New(fileFilters As IEnumerable(Of FileFilter))
        _FileFilters = fileFilters
    End Sub

    Public Overrides Function ToString() As String
        Return String.Join("|", _FileFilters.Select(Function(fileFilter) fileFilter.ToString))
    End Function
End Class
