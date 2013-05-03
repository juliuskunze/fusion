Public Class OpenDescriptionDialog
    Inherits OpenFileDialog

    Public Sub New(owner As Window,
                   initialDirectory As DirectoryInfo)
        MyBase.New(owner:=owner,
                   initialDirectory:=initialDirectory,
                   FileFilters:=DescriptionFileHelper.OpenFileFilters)
    End Sub

    Public Shadows Function Show() As Boolean
        If Not MyBase.Show Then Return False

        Return DescriptionOpener.Check
    End Function

    Private ReadOnly Property DescriptionOpener As DescriptionOpener
        Get
            Return New DescriptionOpener(File)
        End Get
    End Property
End Class
