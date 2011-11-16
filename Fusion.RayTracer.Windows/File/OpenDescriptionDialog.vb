﻿Public Class OpenDescriptionDialog
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
                Case Else : Throw New ArgumentOutOfRangeException
            End Select
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


End Class
