Public Class SavePictureDialog
    Inherits SaveFileDialog

    Public Sub New(owner As Window, initalDirectory As DirectoryInfo)
        MyBase.New(owner:=owner,
            FileFilters:=New FileFilters(
                {
                    New FileFilter("*.png", "Portable Network Graphics"),
                    New FileFilter("*.bmp", "Bitmap")
                }),
            DefaultExtension:=".png",
            initialDirectory:=initalDirectory,
            initalFileName:="Ray tracer picture")
    End Sub

End Class
