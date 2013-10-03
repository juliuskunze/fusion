Imports Microsoft

Public Class SaveFileDialog
    Private ReadOnly _Owner As Window
    Private ReadOnly _SaveFileDialog As Win32.SaveFileDialog

    Public Sub New(owner As Window,
                   initialDirectory As DirectoryInfo,
                   fileFilters As FileFilters,
                   defaultExtension As String,
                   initialFileName As String)
        _Owner = owner
        _SaveFileDialog = New Win32.SaveFileDialog With
            {
            .DefaultExt = defaultExtension,
            .Filter = fileFilters.ToString,
            .FileName = initialFileName,
            .InitialDirectory = initialDirectory.FullName
            }
    End Sub

    Public Sub New(owner As Window,
               initialDirectory As DirectoryInfo,
               defaultExtension As String,
               initialFileName As String)
        Me.New(owner:=owner, initialDirectory:=initialDirectory, defaultExtension:=defaultExtension, initialFileName:=initialFileName, FileFilters:=New FileFilters({}))
    End Sub

    Public Property InitialDirectory As DirectoryInfo
        Get
            Return New DirectoryInfo(_SaveFileDialog.InitialDirectory)
        End Get
        Set(value As DirectoryInfo)
            _SaveFileDialog.InitialDirectory = value.FullName
        End Set
    End Property

    Public Property File As FileInfo
        Get
            Return New FileInfo(_SaveFileDialog.FileName)
        End Get
        Set(value As FileInfo)
            _SaveFileDialog.FileName = value.FullName
        End Set
    End Property

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
