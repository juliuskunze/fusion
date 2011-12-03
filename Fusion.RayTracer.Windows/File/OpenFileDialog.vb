Imports Microsoft

Public Class OpenFileDialog

    Private ReadOnly _Owner As Window
    Private ReadOnly _FileFilters As FileFilters

    Public Property File As FileInfo
        Get
            Return New FileInfo(_OpenFileDialog.FileName)
        End Get
        Set(value As FileInfo)
            _OpenFileDialog.FileName = value.FullName
        End Set
    End Property

    Public ReadOnly Property DefaultExtension As String
        Get
            Return _OpenFileDialog.DefaultExt
        End Get
    End Property

    Private ReadOnly _OpenFileDialog As Win32.OpenFileDialog

    Public Sub New(owner As Window,
                   fileFilters As FileFilters,
                   defaultFilter As FileFilter,
                   initialDirectory As DirectoryInfo)
        _Owner = owner
        _FileFilters = fileFilters
        _OpenFileDialog = New Win32.OpenFileDialog With
            {
            .Filter = fileFilters.ToString,
            .InitialDirectory = initialDirectory.Name
            }
    End Sub

    Public Overridable Function Show() As Boolean
        Return _OpenFileDialog.ShowDialog(owner:=_Owner).Value
    End Function

End Class
