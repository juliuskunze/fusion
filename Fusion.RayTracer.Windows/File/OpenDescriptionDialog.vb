Public Class OpenDescriptionDialog
    Inherits OpenFileDialog

    Public Sub New(owner As Window,
                   initialDirectory As DirectoryInfo)
        MyBase.New(owner:=owner,
                   initialDirectory:=initialDirectory,
                   FileFilters:=DescriptionFileHelper.OpenFileFilters,
                   defaultFilter:=DescriptionFileHelper.AllFilesFilter)
    End Sub

    Public ReadOnly Property Mode As CompileMode
        Get
            Select Case Me.File.Extension
                Case ".pic" : Return CompileMode.Picture
                Case ".vid" : Return CompileMode.Video
                Case Else
                    Throw New InvalidOperationException
            End Select
        End Get
    End Property

    Private ReadOnly Property IsModeValid As Boolean
        Get
            Return {".pic", ".vid"}.Contains(Me.File.Extension)
        End Get
    End Property

    Public Function OpenDescription() As String
        Dim mode As CompileMode
        Try
            mode = Me.Mode
        Catch ex As ArgumentOutOfRangeException
            mode = CompileMode.Picture
        End Try

        Try
            Using streamReader = New IO.StreamReader(Me.File.FullName)
                Return streamReader.ReadToEnd()
            End Using
        Catch ex As IOException
            MessageBox.Show(ex.Message)
            Return ""
        End Try
    End Function

    Public Overrides Function Show() As Boolean
        If Not MyBase.Show Then Return False
        If Not Me.IsModeValid Then
            MessageBox.Show(messageBoxText:="The selected file is not a description file (.pic or .vid).", caption:="No description")
            Return False
        End If

        Return True
    End Function


End Class
