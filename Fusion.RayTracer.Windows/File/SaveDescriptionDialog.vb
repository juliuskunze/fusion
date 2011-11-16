Public Class SaveDescriptionDialog
    Inherits SaveFileDialog

    Public Property FileAccepted As Boolean

    Public Property Mode As CompileMode

    Public Sub New(owner As Window, initialDirectory As DirectoryInfo)
        MyBase.New(owner:=owner,
                   initialDirectory:=initialDirectory,
                   DefaultExtension:=".pic",
                   initalFileName:="Ray tracer picture scene description")
    End Sub

    Private ReadOnly Property SelectedFilterIndex As Integer
        Get
            Select Case _Mode
                Case CompileMode.Picture : Return 1
                Case CompileMode.Video : Return 2
                Case Else : Throw New ArgumentOutOfRangeException
            End Select
        End Get
    End Property

    Private Function SelectedFileExtensionFilter() As FileFilter
        Return DescriptionFileHelper.GetFileFilter(_Mode)
    End Function

    Public Sub ShowAndTrySave(description As String)
        Me.DefaultExtension = Me.ModeExtension

        If Not MyBase.Show Then Return

        Me.Save(description)
    End Sub

    Private ReadOnly Property ModeExtension As String
        Get
            Select Case _Mode
                Case CompileMode.Picture
                    Return ".pic"
                Case CompileMode.Video
                    Return ".vid"
                Case Else
                    Throw New ArgumentOutOfRangeException
            End Select
        End Get
    End Property

    Public Sub TrySave(description As String)
        If _FileAccepted Then
            Me.Save(description:=description)
        Else
            Me.ShowAndTrySave(description:=description)
        End If
    End Sub

    Private Sub Save(description As String)
        Try
            Using streamWriter = New IO.StreamWriter(Me.File.FullName)
                streamWriter.Write(description)
            End Using
        Catch ex As IO.IOException
            MessageBox.Show(ex.Message)
            Return
        End Try

        Me.FileAccepted = True

        RaiseEvent Saved()
    End Sub

    Public Event Saved()

End Class
