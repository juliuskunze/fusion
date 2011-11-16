Public Class SaveVideoDialog
    Inherits SaveFileDialog

    Public Sub New(owner As Window, initialDirectory As DirectoryInfo)
        MyBase.New(owner:=owner,
                   initialDirectory:=initialDirectory,
                   DefaultExtension:=".avi",
                   FileFilters:=DescriptionFileHelper.SaveVideoFilters,
                   initalFileName:="Ray tracer video")
    End Sub

End Class
