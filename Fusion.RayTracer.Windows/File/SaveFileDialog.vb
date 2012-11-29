Imports Microsoft

Public Class SaveFileDialog
    Private ReadOnly _Owner As Window
    Protected _FileFilters As FileFilters
    Private ReadOnly _SaveFileDialog As Win32.SaveFileDialog

    Public Sub New(owner As Window,
                   initialDirectory As DirectoryInfo,
                   fileFilters As FileFilters,
                   defaultExtension As String,
                   initalFileName As String)
        _Owner = owner
        _FileFilters = fileFilters
        _SaveFileDialog = New Win32.SaveFileDialog With
            {
            .DefaultExt = defaultExtension,
            .Filter = fileFilters.ToString,
            .FileName = initalFileName,
            .InitialDirectory = initialDirectory.FullName
            }
    End Sub

    Public Sub New(owner As Window,
               initialDirectory As DirectoryInfo,
               defaultExtension As String,
               initalFileName As String)
        Me.New(owner:=owner, initialDirectory:=initialDirectory, defaultExtension:=defaultExtension, initalFileName:=initalFileName, FileFilters:=New FileFilters({}))
    End Sub

    Public Property File As FileInfo
        Get
            Return New FileInfo(_SaveFileDialog.FileName)
        End Get
        Set(value As FileInfo)
            _SaveFileDialog.FileName = value.FullName
        End Set
    End Property

    'Private _File As FileInfo
    'Public Property File As FileInfo
    '    Get
    '        Return _File
    '    End Get
    '    Set(value As FileInfo)
    '        _File = value

    '        _SaveFileDialog.FileName = value.Name
    '        _SaveFileDialog.InitialDirectory = value.DirectoryName
    '    End Set
    'End Property

    Protected Property DefaultExtension As String
        Get
            Return _SaveFileDialog.DefaultExt
        End Get
        Set(value As String)
            _SaveFileDialog.DefaultExt = value
        End Set
    End Property

    Public Function Show() As Boolean
        Return _SaveFileDialog.ShowDialog(owner:=_Owner).Value
    End Function

End Class
