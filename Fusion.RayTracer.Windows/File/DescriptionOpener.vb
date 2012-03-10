Public Class DescriptionOpener
    Private ReadOnly _File As FileInfo

    Public Sub New(file As FileInfo)
        _File = file
    End Sub

    Public ReadOnly Property Mode As CompileMode
        Get
            Select Case _File.Extension
                Case ".pic" : Return CompileMode.Picture
                Case ".vid" : Return CompileMode.Video
                Case Else
                    Throw New InvalidOperationException
            End Select
        End Get
    End Property

    Private ReadOnly Property IsModeValid As Boolean
        Get
            Return {".pic", ".vid"}.Contains(_File.Extension)
        End Get
    End Property

    Public Function OpenDescription() As String
        Dim compileMode As CompileMode
        Try
            compileMode = Mode
        Catch ex As ArgumentOutOfRangeException
            compileMode = compileMode.Picture
        End Try

        Try
            Using streamReader = New StreamReader(_File.FullName)
                Return streamReader.ReadToEnd()
            End Using
        Catch ex As IOException
            MessageBox.Show(ex.Message)
            Return ""
        End Try
    End Function

    Public Function Check() As Boolean
        If Not IsModeValid Then
            MessageBox.Show(messageBoxText:="The selected file is not a description file (.pic or .vid).", caption:="No description")
            Return False
        End If

        Return True
    End Function
End Class
